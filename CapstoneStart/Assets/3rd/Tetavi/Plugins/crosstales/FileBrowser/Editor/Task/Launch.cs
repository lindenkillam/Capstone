﻿#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace Crosstales.FB.EditorTask
{
   /// <summary>Show the configuration window on the first launch.</summary>
   public class Launch : AssetPostprocessor
   {
      public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
      {
         if (importedAssets.Any(str => str.Contains(EditorUtil.EditorConstants.ASSET_UID.ToString())))
         {
            Common.EditorTask.SetupResources.Setup();
            SetupResources.Setup();

            EditorIntegration.ConfigWindow.ShowWindow(4);
         }
      }
   }
}
#endif
// © 2019-2021 crosstales LLC (https://www.crosstales.com)