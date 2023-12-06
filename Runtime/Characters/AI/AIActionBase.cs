using UnityEngine;

namespace SF.Characters.AI
{
    /// <summary>
    /// This is the base class for all AI Actions
    /// </summary>
    public class AIActionBase : MonoBehaviour
    {
        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {

        }
    }
}
