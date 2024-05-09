using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static TetaviCAPI;

public class TetaviPlayer : TetaviPlayerBase
{
    protected override void CreateStreamDecoder()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            base.CreateStreamDecoder();
            return;
        }
#endif
        AudioSource audioPlayer = gameObject.GetComponent<AudioSource>();
        stream = audioPlayer ? new TetaviStreamCompositeWithAudio(true, audioPlayer) : new TetaviStreamComposite(true);
    }
    
}
