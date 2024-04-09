using UnityEngine;

namespace UI.PopupText
{
    [CreateAssetMenu(fileName = "New PopupTextData",
        menuName = "PopupText/PopupTextData")]
    public class PopupTextAssetData : ScriptableObject
    {
        public Color fontColor;
        [Range(4, 7)] public float fontSize;

        public AnimationCurve scaleCurve;
        public AnimationCurve verticalCurve;
        public AnimationCurve horizontalCurve;
        public AnimationCurve alphaCurve;
        public Sprite icon;

        public float EndTime
        {
            get
            {
                if (scaleCurve == null)
                    return horizontalCurve.keys[^1].time;
                return horizontalCurve.keys[^1].time + scaleCurve.keys[^1].time; 
            }
        }

        public float EvaluateScale(float time)
        {
            if (scaleCurve == null)
                return 0;
            return scaleCurve.Evaluate(time);
        }

        public float EvaluateVertical(float time)
        {
            if (scaleCurve == null)
                return verticalCurve.Evaluate(time);

            if (time < scaleCurve.keys[^1].time)
                return 0;
            return verticalCurve.Evaluate(time - scaleCurve.keys[^1].time); 
        }

        public float EvaluateHorizontal(float time)
        {
            if (scaleCurve == null)
                return horizontalCurve.Evaluate(time);

            if (time < scaleCurve.keys[^1].time)
                return 0;
            return horizontalCurve.Evaluate(time - scaleCurve.keys[^1].time);
        }

        /// <summary>
		/// Fade away starts with the verticalCurve
        /// </summary>
		///

        public float EvaluateAlpha(float time)
        {
            if (scaleCurve == null)
                return Mathf.Clamp(alphaCurve.Evaluate(time), 0, 1);

            if (time < scaleCurve.keys[^1].time)
                return 1;
            return Mathf.Clamp(alphaCurve.Evaluate(time - scaleCurve.keys[^1].time), 0, 1);
        }
    }
}

