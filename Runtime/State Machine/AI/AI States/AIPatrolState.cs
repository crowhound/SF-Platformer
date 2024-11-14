using SF.Characters;
using SF.Characters.Controllers;

using UnityEngine;

namespace SF.AIModule.StateMachines
{
	[CreateAssetMenu(fileName = "AI Patrol", menuName = "SF /Data/States/AI/AI Patrol")]
	public class AIPatrolState : AIState
	{
		public bool DoTurnOnCollision = true;
		protected override void OnInit()
		{
			controller = AIBrain.GetComponent<Controller2D>();
			character = AIBrain.GetComponent<Character2D>();
			
		}

		protected override void OnUpdateState(Controller2D controller)
		{
			
		}
	}
}
