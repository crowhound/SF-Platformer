using SF.Characters.Controllers;

using UnityEngine;

namespace SF.AI
{
    public abstract class AIState : ScriptableObject
	{
        private void Awake()
        {
            Init();
		}
        
        private void Init()
        {
            OnInit();
        }
        protected abstract void OnInit();

        public void UpdateState(Controller2D controller)
        {

        }
        protected abstract void OnUpdateState(Controller2D controller);
    }
}