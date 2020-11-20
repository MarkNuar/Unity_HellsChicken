using UnityEngine;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Crosshair
{
    public class CrosshairImageController : MonoBehaviour
    {
        [SerializeField] private Sprite enabledCrosshair;
        [SerializeField] private Sprite blockedCrosshair;
        
        private Image _image;
        
        //CrossAir can be in 3 states: 
        // enabled
        // disabled
        // blocked

        private void Start()
        {
            _image = gameObject.GetComponent<Image>();
            SetCrosshairToIdle();
        }
    
        public void SetCrosshairToAiming()
        {
            _image.sprite = enabledCrosshair;
            Color temp = _image.color;
            temp.a = 1f;
            _image.color = temp;
        }

        public void SetCrosshairToIdle()
        {
            _image.sprite = enabledCrosshair;
            Color temp = _image.color;
            temp.a = 0.3f;
            _image.color = temp;
        }
    
        public void SetCrosshairToWaiting()
        {
            _image.sprite = blockedCrosshair;
            Color temp = _image.color;
            temp.a = 1f;
            _image.color = temp;
        }
    }
}
