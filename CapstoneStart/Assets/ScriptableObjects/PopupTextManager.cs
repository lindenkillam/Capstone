using System;
using UnityEngine;
using UnityEditor; 
//using Definition.Constant;
//using Entity;
//using UnityGameFramework.Runtime;
//using GameEntry = Base.GameEntry;
using Random = UnityEngine.Random; 

namespace UI.PopupText
{
    public static class PopupTextManager
    {
        private const string PopupTextSettingPath = "Assets/ScriptableObjects/PopupTextSetting.asset";
        private const string PopupTextGroup = "PopupText";
        private const string PopupTextPrefabPath = "Assets/ScriptableObjects/PopupText.prefab";

        private static PopupTextSetting _popupTextSetting;
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _popupTextSetting = (PopupTextSetting)AssetDatabase.LoadAssetAtPath(PopupTextSettingPath, typeof(PopupTextSetting));
            if(_popupTextSetting == null)
            {
                Debug.LogError($"PopupTextSetting is null, please check the path: {PopupTextSettingPath}");
                return; 
            }

            _initialized = true;
            Debug.Log("PopupTextManager initialize complete"); 
        }

        /// <summary>
		/// Show text values
		/// </summary>
		/// <param name = "actorTransform">actor's transform</param>
		/// <param name = "damageValue">value of damage or healing</param>
		/// <param name = "textType">type of popupText</param>
		/// <param name = "hitVelocity">direction of the popupText, if not set then random</param>

        public static void ShowDamageText(Transform actorTransform, int damageValue, PopupTextType textType,
            Vector3 hitVelocity = default)
        {
            if(actorTransform == null)
            {
                Debug.LogError($"Can't show damage text because actorTransform is null");
                return;
            }

            var toRight = VelocityToRight(hitVelocity) ? 1 : -1;
            CreateDamagePopupText(damageValue, textType, actorTransform.position, toRight); 
        }

        public static void CreateDamagePopupText(int damage, PopupTextType textType, Vector3 position, int toRight)
        {
            var textAsset = textType switch
            {
                PopupTextType.Damage => _popupTextSetting.damageTextAsset,
                PopupTextType.CriticalDamage => _popupTextSetting.criticalDamageTextAsset,
                PopupTextType.Heal => _popupTextSetting.healTextAsset,
                _ => throw new ArgumentOutOfRangeException(nameof(textType), textType, "Unavailable DamagePopupType"),
            };

            var textData = new PopupTextData(position, damage.ToString(), textAsset, toRight);
            textData.SetDamageValue(damage); 
        }

        public static bool VelocityToRight(Vector3 velocity)
        {
            if(velocity == default)
                return Random.Range(0f, 1f) > 0.5f;
            return velocity.x >= 0; 
        }
    }
}