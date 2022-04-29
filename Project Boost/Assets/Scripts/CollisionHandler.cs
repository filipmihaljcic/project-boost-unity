using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

namespace Project.Scripts
{
    public class CollisionHandler : MonoBehaviour
    {
        [Header("Collision Settings")]

        [Tooltip("Ammount of time we wait after death.")]
        public float _crashTimeDelay = 1f;

        [Tooltip("Ammount of time we wait after finishing level.")]
        public float _loadTimeDelay = 1f;
  
        [Tooltip("Sound when player crashes.")]
        public AudioClip _crashSound;
    
        [Tooltip("Sound when player finishes level.")]
        public AudioClip _successSound;

        [Tooltip("Particle effect when level is finished.")]
        public ParticleSystem _successParticles;

        [Tooltip("Particle effect when our player crashes.")]
        public ParticleSystem _crashParticles;
    
        private AudioSource _audioSource;

        private bool _isTransitioning = false; // check if we are transitioning into next level
        
        private bool _isDisabled = false; // used for checking if collision is disabled

        private void OnCollisionEnter([NotNull]Collision _other) 
        {
            if(_isTransitioning || _isDisabled) return;

            switch(_other.gameObject.tag)
            {
                // if player is standing on launch pad
                case "Friendly":
                     break;
                // if player lands on landing pad
                case "Finish":
                    StartLoadSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }    
        }

        private void Start() 
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update() 
        {
            Cheats();
        }

        private void Cheats()
        {
            // cheat functionality
            if (Input.GetKey(KeyCode.L))
                LoadNextLevel();
            else if (Input.GetKey(KeyCode.C))
                _isDisabled = !_isDisabled;
        }

        private void StartLoadSequence()
        {
            _isTransitioning = true;
            _audioSource.Stop();
            _successParticles.Play();
            _audioSource.PlayOneShot(_successSound);
            //disable controls for our player 
            GetComponent<Movement>().enabled = false; 
            Invoke(nameof(LoadNextLevel), _loadTimeDelay);   
        }

        private void StartCrashSequence()
        {
            _isTransitioning = true;
            _crashParticles.Play();
            _audioSource.PlayOneShot(_crashSound);
            GetComponent<Movement>().enabled = false;
            Invoke(nameof(ReloadLevel), _crashTimeDelay);
        }

        private void LoadNextLevel()
        {
            int _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int _nextSceneIndex = _currentSceneIndex + 1;
            // check if next scene index is equal to total number of scenes in our game
            if (_nextSceneIndex == SceneManager.sceneCountInBuildSettings)
                // load first level
                _nextSceneIndex = 0;

            SceneManager.LoadScene(_nextSceneIndex);        
        }

        private void ReloadLevel()
        {
            // put current active scene index in variable for more elegant look
            int _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(_currentSceneIndex);
        }
    }
}
