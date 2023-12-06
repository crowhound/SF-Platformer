using SF.Characters;
using SF.Characters.Controllers;
using SF.StateMachine.AI;

using UnityEngine;

namespace SF.AI
{
    public abstract class AIState : ScriptableObject
	{
        protected AIBrain AIBrain;
        protected Character2D character;
        protected Controller2D controller;
        

        public void Init(AIBrain brain)
        {
		
			AIBrain = brain;
            OnInit();
        }
        protected abstract void OnInit();

        public void UpdateState()
        {

        }
        protected abstract void OnUpdateState(Controller2D controller);
    }
}