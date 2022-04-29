using UnityEngine;

namespace Project.Scripts
{
    public class Movement : MonoBehaviour
    {
        [Header("Rocket settings")]

        [Tooltip("Speed of our rocket.")]
        public float _mainThrust = 10f;

        [Tooltip("Rotation speed of our rocket.")]
        public float _rotationThrust = 10f;

        [Tooltip("Thrust sound of our rocket.")]
        public AudioClip _mainEngineSound;

        [Tooltip("Particle effect for our rocket engine.")]
        public ParticleSystem _mainEngineParticles;

        [Tooltip("Particle effect for our left rocket thruster.")]
        public ParticleSystem _leftThrusterParticles;

        [Tooltip("Particle effect for our right rocket thruster.")]
        public ParticleSystem _rightThrusterParticles;

        Rigidbody _rb;
        
        AudioSource _audioSource;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();

            _mainEngineParticles.Stop();
            _leftThrusterParticles.Stop();
            _rightThrusterParticles.Stop();
        }


        private void Update()
        {
            ProcessThrust();
            ProcessRotation();
        }
    
        private void ProcessThrust()
        {
            if (Input.GetKey(KeyCode.Space))
                StartThrusting();
            else
                StopThrusting();
        } 

        private void ProcessRotation()
        {
            if (Input.GetKey(KeyCode.A))
                RotateLeft();
            else if (Input.GetKey(KeyCode.D))
                RotateRight();
            else 
                StopRotating();
        }

        private void ApplyRotation(float _rotation)
        {
            _rb.freezeRotation = true; // freezing rotation so we can manually rotate(because of physics system)
            transform.Rotate(Vector3.forward * _rotation * Time.deltaTime);  // rotate rocket around z-axis
            _rb.freezeRotation = false;
        }

        private void StartThrusting()
        {
            // force will be added in the direction that rocket is facing
            _rb.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
            if (!_audioSource.isPlaying)
                // play thrust sound that is attached to our rocket
                _audioSource.PlayOneShot(_mainEngineSound);     
            if (!_mainEngineParticles.isPlaying)
                _mainEngineParticles.Play();      
        }

        private void StopThrusting()
        {
            _audioSource.Stop();
            _mainEngineParticles.Stop();
        }

        private void RotateLeft()
        {
            ApplyRotation(_rotationThrust);

            if (!_rightThrusterParticles.isPlaying)
                _rightThrusterParticles.Play();
        }

        private void RotateRight()
        {
            ApplyRotation(-_rotationThrust);
            
            if (!_leftThrusterParticles.isPlaying)
                _leftThrusterParticles.Play();
        }

        private void StopRotating()
        {
            _rightThrusterParticles.Stop();
            _leftThrusterParticles.Stop();
        }
    }
}
