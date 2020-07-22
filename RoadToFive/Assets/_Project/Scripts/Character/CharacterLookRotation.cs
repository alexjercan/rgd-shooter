using UnityEngine;

namespace _Project.Scripts.Character
{
    public class CharacterLookRotation
    {
        private Vector3 _targetAngles;
        private Vector3 _followAngles;
        private readonly float _baseCameraFieldOfView;
        private Vector3 _cameraCurrentVelocity;
        private readonly float _originalYRotation;

        public CharacterLookRotation(float baseCameraFieldOfView, float originalYRotation)
        {
            _originalYRotation = originalYRotation;
            _baseCameraFieldOfView = baseCameraFieldOfView;
        }
        
        public Vector2 GetCharacterRotation(float mouseXInput, float mouseYInput, float cameraFieldOfView,
            float internalMouseSensitivity, float verticalRotationRange, float cameraSmoothing)
        {
            if (_targetAngles.y > 180)
            {
                _targetAngles.y -= 360;
                _followAngles.y -= 360;
            }
            else if (_targetAngles.y < -180)
            {
                _targetAngles.y += 360;
                _followAngles.y += 360;
            }

            if (_targetAngles.x > 180)
            {
                _targetAngles.x -= 360;
                _followAngles.x -= 360;
            }
            else if (_targetAngles.x < -180)
            {
                _targetAngles.x += 360;
                _followAngles.x += 360;
            }

            _targetAngles.y += mouseXInput * (internalMouseSensitivity - (_baseCameraFieldOfView - cameraFieldOfView) / 6.0f);
            _targetAngles.x += mouseYInput * (internalMouseSensitivity - (_baseCameraFieldOfView - cameraFieldOfView) / 6.0f);
            _targetAngles.y = Mathf.Clamp(_targetAngles.y, -0.5f * Mathf.Infinity, 0.5f * Mathf.Infinity);
            _targetAngles.x = Mathf.Clamp(_targetAngles.x, -0.5f * verticalRotationRange, 0.5f * verticalRotationRange);

            _followAngles = Vector3.SmoothDamp(_followAngles,
                _targetAngles,
                ref _cameraCurrentVelocity,
                cameraSmoothing / 100);
            
            return new Vector2(-_followAngles.x, _followAngles.y + _originalYRotation);
        }
    }
}