using System.Collections.Generic;
using UnityEngine;

using SF.AI;

namespace SF.StateMachine.AI
{
    public class AIBrain : MonoBehaviour
    {
        public List<AIState> States = new();

		public void Awake()
		{
			Init();
		}

		protected void Start()
		{

		}

		private void Init()
		{
			Debug.Log(States.Count);
			for(int x = 0; x < States.Count; x++)
			{
				Debug.Log(States[x]);
				States[x].Init(this);
			}
		}
	}
}
