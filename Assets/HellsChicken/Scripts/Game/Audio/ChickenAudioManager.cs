using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class ChickenAudioManager : Singleton<ChickenAudioManager>
    {
        [SerializeField] private AudioSource flameThrowerAudioSource;
        [SerializeField] private AudioSource footstepsAudioSource;
        [SerializeField] private AudioSource wingsFlappingAudioSource;
        [SerializeField] private AudioSource chickenDamageAudioSource;
        [SerializeField] private AudioSource chickenDeathAudioSource;
        [SerializeField] private AudioSource chickenSpawnAudioSource;
        [SerializeField] private AudioSource chickenJumpAudioSource;
        [SerializeField] private AudioSource eggThrowAudioSource;
        [SerializeField] private AudioSource chickenLandAudioSource;


        [SerializeField] private AudioClip flameThrowerSound;
        [SerializeField] private AudioClip footstepsSound;
        [SerializeField] private AudioClip wingsFlappingSound;
        [SerializeField] private AudioClip chickenDamageSound;
        [SerializeField] private AudioClip chickenDeathSound;
        [SerializeField] private AudioClip chickenSpawnAudioSound;
        [SerializeField] private AudioClip chickenJumpSound;
        [SerializeField] private AudioClip eggThrowSound;
        [SerializeField] private AudioClip chickenLandSound;


        protected override void Awake() {
            EventManager.StartListening("flameThrower", FlameThrower);
            EventManager.StartListening("footsteps", Footsteps);
            EventManager.StartListening("stopFootsteps",StopFootsteps);
            EventManager.StartListening("wingsFlap", WingsFlap);
            EventManager.StartListening("chickenDamage",ChickenDamage);
            EventManager.StartListening("chickenDeath",ChickenDeath);
            EventManager.StartListening("chickenSpawnSound",ChickenSpawn);
            EventManager.StartListening("chickenJumpSound",ChickenJump);
            EventManager.StartListening("eggThrowSound",EggThrow);
            EventManager.StartListening("chickenLand",ChickenLand);

        }


        private void FlameThrower()
        {
            EventManager.StopListening("flameThrower", FlameThrower);
            flameThrowerAudioSource.clip = flameThrowerSound;
            flameThrowerAudioSource.Play();
            EventManager.StartListening("flameThrower", FlameThrower);
        }

        private void Footsteps()
        {
            EventManager.StopListening("footsteps", Footsteps);
            footstepsAudioSource.clip = footstepsSound;
            if (!footstepsAudioSource.isPlaying)
                footstepsAudioSource.Play();
            EventManager.StartListening("footSteps",Footsteps);
        }

        private void StopFootsteps()
        {
            EventManager.StopListening("stopFootsteps",StopFootsteps);
            footstepsAudioSource.Stop();
            EventManager.StartListening("stopFootsteps",StopFootsteps);
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
            chickenSpawnAudioSource.Play();
            EventManager.StartListening("chickenSpawnSound",ChickenSpawn);
        }

        public void ChickenJump()
        {
            EventManager.StopListening("chickenJumpSound",ChickenJump);
            chickenJumpAudioSource.clip = chickenJumpSound;
            chickenJumpAudioSource.Play();
            EventManager.StartListening("chickenJumpSound",ChickenJump);

        }

        public void EggThrow()
        {
            EventManager.StopListening("eggThrowSound",EggThrow);
            eggThrowAudioSource.clip = eggThrowSound;
            eggThrowAudioSource.Play();
            EventManager.StartListening("eggThrowSound",EggThrow);
        }

        public void ChickenLand()
        {
            EventManager.StopListening("chickenLand", ChickenLand);
            chickenLandAudioSource.clip = chickenLandSound;
            chickenLandAudioSource.Play();
            EventManager.StartListening("chickenLand",ChickenLand);
        }
    }
}
