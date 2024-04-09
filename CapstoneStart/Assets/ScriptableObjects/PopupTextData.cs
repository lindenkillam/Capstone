using UnityEngine;

namespace UI.PopupText
{
    public class PopupTextData
    {
        public string Text { get; }
        public PopupTextAssetData AssetData { get; }
        public int DamageValue { get; private set; }
        public int ToRight { get; }
        public Vector3 Position { get; private set; }

        public PopupTextData(Vector3 position, string text, PopupTextAssetData assetData, int toRight)
        {
            Text = text;
            AssetData = assetData;
            ToRight = toRight;
            Position = position; 
        }

        public void SetDamageValue(int value)
        {
            DamageValue = value;
        }
    }
}