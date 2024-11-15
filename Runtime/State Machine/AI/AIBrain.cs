using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SF.AIModule.StateMachines
{
    public class AIBrain : MonoBehaviour
    {
        // TODO: The below _shouldSearchForAIActions is only needed
        /// <summary>
        /// When Start() is called on this AIBrain should we auto get components.
        /// </summary>
        [SerializeField] private bool _shouldSearchForAIActions = true;
		
		[SerializeField] private AIActionBase _startingAction;
		[SerializeField] private AIActionBase _currentAction;
        [SerializeField] private List<AIActionBase> _aiActions = new();

		public void Awake()
		{
			if(_shouldSearchForAIActions)
			{
				_aiActions.Clear();
				_aiActions = GetComponents<AIActionBase>().ToList();
			}

			Init();
		}

		protected void Start()
		{
			if(_currentAction != null)
				_currentAction.DoAction();
		}

		private void Init()
		{
			for(int x = 0; x < _aiActions.Count; x++)
			{
				_aiActions[x].IsControlledByAIBrain = true;
				_aiActions[x].Init();
			}

			// We only set the current action if a starting action was set.
			// This allows for people to choose a starting action.
			if(_startingAction != null)
				_currentAction = _startingAction;
		}
	}
}
