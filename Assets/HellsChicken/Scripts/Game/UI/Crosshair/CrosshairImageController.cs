using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Crosshair
{
    public class CrosshairImageController : MonoBehaviour
    {
        private Image _image;
        public Animator anim;
        
        //CrossAir can be in 3 states: 
        // enabled
        // disabled
        // blocked

        private void Start()
        {
            _image = gameObject.GetComponent<Image>();
            SetToIdle();
        }
    
        public void StartAiming()
        {
            Color temp = _image.color;
            temp.a = 1f;
            _image.color = temp;
        }
        
        public void SetToIdle()
        {
            Color temp = _image.color;
            temp.a = 0.3f;
            _image.color = temp;
            anim.SetBool("isIdle", true);
            anim.SetBool("isCooldown", false);
        }

        public void StartCooldownAnimation(float duration)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isCooldown", true);
            anim.SetFloat("cooldownSpeed",2.25f/duration);
        }
    }
}
