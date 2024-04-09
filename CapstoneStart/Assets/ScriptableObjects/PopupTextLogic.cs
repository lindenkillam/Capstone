using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace UI.PopupText
{
    public class PopupTextLogic : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;
        private PopupTextData _textData;
        private PopupTextAssetData _assetData;
        private RectTransform _iconCanvasRect;
        private Image _icon;
        private Vector3 _startPos;
        private float _startPosOffsetX;
        private Color _textColor;
        private float _elapsedTime;
        private float _horizontalRandPos;
        private float _verticalRandPos;
        private int _toRight;
        private float _initialScale;
        private static int _sortingOrder;

        private const float CommonTextScale = 1.3f;
        private readonly Vector2 _positionRandomRange = new Vector2(0.6f, 1.3f);

        public void OnInit()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
            if (_textMeshPro == null)
            {
               Debug.LogError(message: "popup TextMeshPro Component is null, please check the prefab");
            }

            _iconCanvasRect = GetComponentInChildren<RectTransform>();
            _icon = _iconCanvasRect.GetComponentInChildren<Image>();
            if(_icon == null)
            {
                Debug.LogError(message: "popup Icon Image Component is null, please check the prefab");
            }
        }

        public void OnShow()
        {
            _assetData = _textData.AssetData;
            _textMeshPro.fontSize = _assetData.fontSize;
            _textMeshPro.SetText(_textData!.Text);
            if (_assetData.icon != null)
            {
                _icon.sprite = _assetData.icon;
                SetIcon();
            }
            else
            {
                _icon.enabled = false;
            }

            _elapsedTime = 0;
            _toRight = _textData.ToRight;
            _horizontalRandPos = Random.Range(_positionRandomRange.x, _positionRandomRange.y);
            _verticalRandPos = Random.Range(_positionRandomRange.x, _positionRandomRange.y);
            _initialScale = GetScaleChange(_textData.DamageValue);
            _textColor = _textData.AssetData.fontColor;
            _textMeshPro.color = _textColor;
            _textMeshPro.sortingOrder = ++_sortingOrder;
            _startPos = _textData.Position + new Vector3(_startPosOffsetX, 0, 0);
            //set rotation of text equal to camera.rotation
        }

        public void OnHide(bool isShutdown)
        {
            //check if shutdown?
            _startPosOffsetX = 0;
            _icon.enabled = false; 
        }

        public void OnUpdate(float elapsedSeconds, float relaElapsedSeconds)
        {
            UpdatePosition();
            UpdateScale();
            UpdateColorAlpha();
            _elapsedTime += elapsedSeconds;
            if(_elapsedTime > _assetData.EndTime)
            {
                //Hide this
            }
        }

        private void UpdatePosition()
        {
            var offsetX = _assetData.EvaluateHorizontal(_elapsedTime) * _horizontalRandPos;
            var offsetY = _assetData.EvaluateVertical(_elapsedTime) * _verticalRandPos;
            var offset = new Vector3(offsetX * _toRight, offsetY, 0);
            transform.position = _startPos + offset; 
        }

        protected void UpdateScale()
        {
            var scaleChange = 1 + _assetData.EvaluateScale(_elapsedTime);
            transform.localScale = Vector3.one * (scaleChange * _initialScale);
        }

        protected void UpdateColorAlpha()
        {
            _textColor.a = _assetData.EvaluateAlpha(_elapsedTime);
            _textMeshPro.color = _textColor; 
        }

        private static float GetScaleChange(int damage)
        {
            if(damage == 0)
            {
                return CommonTextScale;
            }

            const float maxScale = 1.4f;
            const float minScale = 1f;
            const float growthRate = 0.35f;
            const float baseValue = 0.5f;
            return Mathf.Clamp(minScale, baseValue + growthRate * Mathf.Log10(damage), maxScale); 
        }


        private void SetIcon()
        {
            var textWidth = _textMeshPro.GetPreferredValues().x;
            var iconWidth = _icon.rectTransform.sizeDelta.x;
            var iconOffset = (textWidth + iconWidth)/2;
            _icon.rectTransform.localPosition = new Vector2(-iconOffset, 0);
            _icon.enabled = true;
            _startPosOffsetX = iconWidth / 2; 
        }
    }

}

