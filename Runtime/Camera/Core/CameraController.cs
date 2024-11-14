using Unity.Cinemachine;

using UnityEngine;

namespace SF
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<CameraController>();

                if(_instance == null)
                    _instance = Camera.main.gameObject.AddComponent<CameraController>();

                return _instance;
            }
            set { _instance = value; }
        }
        private static CameraController _instance;

        public CinemachineCamera PlayerCamera;

        private void Awake()
        {
            if(Instance != null && _instance  != this)
                Destroy(this);

            Instance = this;
        }
        public static void ChangeCameraConfiner(CinemachineCamera cmCamera)
        {
            if(Instance.PlayerCamera != null)
            {
                cmCamera.Prioritize();
                Instance.PlayerCamera = cmCamera;
            }
        }
    }
}
