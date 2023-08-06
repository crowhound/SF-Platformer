using System.Collections.Generic;
using UnityEngine;

using SF.AI;

namespace SF.StateMachine.AI
{
    public class AIBrain : MonoBehaviour
    {
        public List<AIState> States = new();
		private List<AIState> AIStates = new();
		public void Awake()
		{
			
		}

		public void Start()
		{

		}
	}
}
