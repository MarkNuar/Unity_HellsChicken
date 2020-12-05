using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class EggAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource EggExplosionAudioSource;
    
        [SerializeField] private AudioClip EggExplosionSound;

        private void Awake()
        {
            EventManager.StartListening("EggExplosion",EggExplosion);
        }
    
        private void OnDisable()
        {
            EventManager.StopListening("EggExplosion",EggExplosion);
        }

        private void EggExplosion()
        {
            EventManager.StartListening("EggExplosion",EggExplosion);
            EggExplosionAudioSource.clip = EggExplosionSound;
            EggExplosionAudioSource.Play();
            EventManager.StopListening("EggExplosion",EggExplosion);
        }
        
    }
}
