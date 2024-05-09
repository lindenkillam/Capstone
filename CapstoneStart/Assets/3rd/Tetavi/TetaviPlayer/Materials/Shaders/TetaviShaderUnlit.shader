// shader with "Unlit" writen in it will be considered unlit
Shader "Tetavi/TetaviDefaultShaderUnlit"
{
    Properties
    {
		[HideInInspector]_MS("Material ssegmentation", Float) = 0
        _TexY("Texture", 2D) = "white" {}
        _TexUV ("Texture", 2D) = "white" {}
        _AcceptShadows("Accept Shadows", Int) = 0
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #include "UnityCG.cginc"
            #include "AutoLight.cginc" // shadow helper functions and macros
            #pragma multi_compile_fwdbase
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                SHADOW_COORDS(1)
            };

            sampler2D _TexY;
            sampler2D _TexUV;
			float _MS;
            int _AcceptShadows;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				
				float2 uv = i.uv;
				
				//if (_MS) {
				//	uv.g = uv.g / 2 + 0.5;
				//}
				float y = tex2D(_TexY, uv).r;
				float2 UV_rg = tex2D(_TexUV, uv);
				float u = UV_rg.r * 0.872 - 0.436;
				float v = UV_rg.g * 1.230 - 0.615;
				
				float3 rgb;
				rgb.r = clamp(y + 1.13983 * v, 0.0, 1.0);
				rgb.g = clamp(y - 0.39465 * u - 0.58060 * v, 0.0, 1.0);
				rgb.b = clamp(y + 2.03211 * u, 0.0, 1.0);

				
				/*float gamma = 2.2;
				rgb = pow(rgb,  gamma);*/

				float4 col = float4(rgb, 1);
				return col;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
