using UnityEngine;

namespace SF.InputModule
{
    public class InputManager : MonoBehaviour
    {
		private static InputManager _instance;
		public static InputManager Instance
		{
			get
			{
				if(_instance == null)
					_instance = FindAnyObjectByType<InputManager>();

				return _instance;
			}
		}

		private static Controls _controls;
		public static Controls Controls 
		{
			get
			{
				_controls ??= new Controls();

				return _controls;
			}
		}
		private void Awake()
		{
			if(Instance != null && Instance != this)
				Destroy(this);
		}


		public void EnbaleActionMap()
        {

        }

        public void DisableActionMap()
        {

        }
    }
}