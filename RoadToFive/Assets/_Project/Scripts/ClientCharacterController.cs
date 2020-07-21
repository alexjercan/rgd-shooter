using UnityEngine;

namespace _Project.Scripts
{
    public class ClientCharacterController : MonoBehaviour
    {
        public Vector2 RotationInput { get; set; }

        [SerializeField] private float clampMinXRotation = -90.0f;
        [SerializeField] private float clampMaxXRotation = 90.0f;
        [SerializeField] private float sensitivityX = 15.0f;
        [SerializeField] private float sensitivityY = 15.0f;
        [SerializeField] private float angleThreshold = 1.0f;
        [SerializeField] private Transform cameraTransform;

        private float _xRotation = 0.0f;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            var newXRotation = _xRotation - RotationInput.y * sensitivityX * Time.deltaTime;

            var deltaAbsXRotation = Mathf.Abs(newXRotation - _xRotation);
            var deltaYRotation = RotationInput.x * sensitivityY * Time.deltaTime;
            
            _xRotation = (deltaAbsXRotation <= angleThreshold * sensitivityX * Time.deltaTime) ? _xRotation : newXRotation;
            var newYRotation = Mathf.Abs(deltaYRotation) <= angleThreshold * sensitivityY * Time.deltaTime ? 0 : deltaYRotation;
            
            _xRotation = Mathf.Clamp(_xRotation, clampMinXRotation, clampMaxXRotation);
            
            cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);
            _transform.Rotate(Vector3.up * newYRotation);
        }
    }
}