using UnityEngine;

namespace _Project.Scripts
{
    public class ClientCharacterController : MonoBehaviour
    {
        public float CameraYRotation => cameraTransform.eulerAngles.y;

        [SerializeField] private Transform cameraTransform;
    }
}