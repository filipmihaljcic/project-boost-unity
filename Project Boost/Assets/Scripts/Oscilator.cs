using UnityEngine;

namespace Project.Scripts
{
    public class Oscilator : MonoBehaviour
    {
        private Vector3 _startingPosition;
        [SerializeField] Vector3 _movementVector;
        [SerializeField] [Range(0, 1)] float _movementFactor;
        [SerializeField] float _period;
        private float _cycles;

        private Vector3 _offset;

        private void Start()
        {
            _startingPosition = transform.position;
        }
        
        private void Update()
        {
            SinWave();
        }
        
        private void SinWave()
        {
            _cycles = Time.time / _period; // growing over time 

            const float _tau = Mathf.PI * 2; // constant value of 6.283
            float _rawSinWave = Mathf.Sin(_cycles * _tau); // going from -1 to 1

            _movementFactor = (_rawSinWave + 1f) / 2f;

            _offset = _movementVector * _movementFactor; // how much it will move 
            transform.position = _startingPosition + _offset; // update position of object
        }
    }
}
