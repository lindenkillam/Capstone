//
//  ConvertBGRG422ToRGB.metal
//  TetCodec
//
//  Created by Ofer Rubinstein on 10/05/2020.
//  Copyright © 2020 TetaviLTD. All rights reserved.
//

//const char * TetCodecLibraryVersion = "2020_05_10_B1";

#include <metal_stdlib>
#include "MetalDefines.h"
using namespace metal;

half4 RGBfromYUV(float Y, float U, float V)
{
    float R;
    float G;
    float B;
  Y*=255.0;
  U*=255.0;
  V*=255.0;
  Y -= 16;
  U -= 128;
  V -= 128;
  R = 1.164 * Y             + 1.596 * V;
  G = 1.164 * Y - 0.392 * U - 0.813 * V;
  B = 1.164 * Y + 2.017 * U;
    
    return half4(R/255.0, G/255.0, B/255.0, 1.0);
}

kernel void
YUVKernel(texture2d<half, access::read>  inTexture  [[texture(AAPLTextureIndexInput)]],
                texture2d<half, access::write> outTexture [[texture(AAPLTextureIndexOutput)]],
                uint2                          gid         [[thread_position_in_grid]])
{
    // Check if the pixel is within the bounds of the output texture
    if((gid.x >= outTexture.get_width()) || (gid.y >= outTexture.get_height()))
    {
        // Return early if the pixel is out of bounds
        return;
    }

    half4 inColor1  = inTexture.read(gid);
    outTexture.write(RGBfromYUV(inColor1.g, inColor1.b, inColor1.r), gid);
}
