using UnityEngine;

namespace UI.PopupText
{
    public class TriggerPopupText : MonoBehaviour
    {
        public PopupTextAssetData assetData;
        public PopupTextLogic _popupTextLogic;

        private void Start()
        {
            // Get the PopupTextLogic component attached to this GameObject
            if (_popupTextLogic == null)
            {
                Debug.LogError("PopupTextLogic component not found on this GameObject.");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_popupTextLogic != null)
            {
                // Call OnInit method when a collision occurs
                _popupTextLogic.OnInit();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_popupTextLogic != null)
            {
                // Call OnShow method when a trigger collision occurs
                _popupTextLogic.OnShow();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_popupTextLogic != null)
            {
                // Call OnHide method when exiting a trigger collision
                _popupTextLogic.OnHide(false); // Passing false to indicate it's not a shutdown
            }
        }

        private void Update()
        {
            if (_popupTextLogic != null)
            {
                // Call OnUpdate method every frame
                _popupTextLogic.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }
    }
}
