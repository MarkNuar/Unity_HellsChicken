using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class ChickenAudioManager : Singleton<ChickenAudioManager>
    {
        [SerializeField] private AudioSource centaurShotAudioSource;
        [SerializeField] private AudioSource flameThrowerAudioSource;
        [SerializeField] private AudioSource footStepsAudioSource;
        [SerializeField] private AudioSource wingsFlappingAudioSource;
        [SerializeField] private AudioSource chickenDamageAudioSource;
        [SerializeField] private AudioSource chickenDeathAudioSource;
        [SerializeField] private AudioSource chickenSpawnAudioSource;
        
        [SerializeField] private AudioClip centaurShotSound;
        [SerializeField] private AudioClip flameThrowerSound;
        [SerializeField] private AudioClip footStepsSound;
        [SerializeField] private AudioClip wingsFlappingSound;
        [SerializeField] private AudioClip chickenDamageSound;
        [SerializeField] private AudioClip chickenDeathSound;
        [SerializeField] private AudioClip chickenSpawnAudioSound;

        protected override void Awake() {
            EventManager.StartListening("centaurShot",CentaurShot);
            EventManager.StartListening("flameThrower", FlameThrower);
            EventManager.StartListening("footSteps", FootSteps);
            EventManager.StartListening("stopFootSteps",StopFootSteps);
            EventManager.StartListening("wingsFlap", WingsFlap);
            EventManager.StartListening("chickenDamage",ChickenDamage);
            EventManager.StartListening("chickenDeath",ChickenDeath);
            EventManager.StartListening("chickenSpawnSound",ChickenSpawn);
        }

        private void OnDisable() {
            EventManager.StopListening("centaurShot",CentaurShot);
        }
        
        private void CentaurShot() {
            EventManager.StopListening("centaurShot",CentaurShot);
            centaurShotAudioSource.clip = centaurShotSound;
            centaurShotAudioSource.Play();
            EventManager.StartListening("centaurShot",CentaurShot);
        }

        private void FlameThrower()
        {
            EventManager.StopListening("flameThrower", FlameThrower);
            flameThrowerAudioSource.clip = flameThrowerSound;
            flameThrowerAudioSource.Play();
            EventManager.StartListening("flameThrower", FlameThrower);
        }

        private void FootSteps()
        {
            EventManager.StopListening("footSteps", FootSteps);
            footStepsAudioSource.clip = footStepsSound;
            if (!footStepsAudioSource.isPlaying)
                footStepsAudioSource.Play();
            EventManager.StartListening("footSteps",FootSteps);
        }

        private void StopFootSteps()
        {
            EventManager.StopListening("stopFootSteps",StopFootSteps);
            footStepsAudioSource.Stop();
            EventManager.StartListening("stopFootSteps",StopFootSteps);
        }

        private void WingsFlap()
        {
            EventManager.StopListening("wingsFlap", WingsFlap);
            wingsFlappingAudioSource.clip = wingsFlappingSound;
            if(!wingsFlappingAudioSource.isPlaying)
                wingsFlappingAudioSource.Play();
            EventManager.StartListening("wingsFlap",WingsFlap);
        }

        private void ChickenDamage()
        {
            EventManager.StopListening("chickenDamage",ChickenDamage);
            chickenDamageAudioSource.clip = chickenDamageSound;
            chickenDamageAudioSource.Play();
            EventManager.StartListening("chickenDamage",ChickenDamage);
        }

        private void ChickenDeath()
        {
            EventManager.StopListening("chickenDeath",ChickenDeath);
            chickenDeathAudioSource.clip = chickenDeathSound;
            if(!chickenDeathAudioSource.isPlaying)
                chickenDeathAudioSource.Play();
            EventManager.StartListening("chickenDeath",ChickenDeath);
        }

        public void ChickenSpawn() {
            EventManager.StopListening("chickenSpawnSound",ChickenSpawn);
            chickenSpawnAudioSource.clip = chickenSpawnAudioSound;
            chickenSpawnAudioSource.time = 1f;
            chickenSpawnAudioSource.Play();
            EventManager.StartListening("chickenSpawnSound",ChickenSpawn);
        }
    }
}
