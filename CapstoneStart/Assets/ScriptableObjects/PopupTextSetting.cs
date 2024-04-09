using UnityEngine;


namespace UI.PopupText
{
    [CreateAssetMenu(fileName = "PopupTextSetting", menuName = "PopupText/PopupTextSetting")]

    public class PopupTextSetting : ScriptableObject
    {
        public PopupTextAssetData damageTextAsset;
        public PopupTextAssetData healTextAsset;
        public PopupTextAssetData criticalDamageTextAsset; 
    }
}


