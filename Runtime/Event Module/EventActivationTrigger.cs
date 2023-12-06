using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

namespace SF.Events
{
    public enum EventTriggerType
    {
        OnCollisionEnter,
        OnCollisionExit,
        OnCollisionStay,
        OnCollisionEnter2D,
        OnCollisionExit2D,
        OnCollisionStay2D,
    }

    public class EventActivationTrigger : MonoBehaviour
    {
        [Header("Event Activation Options")]
        public EventTriggerType EventTriggerType;
        public LocalPhysicsMode PhysicsMode;
        public GameEventTypes GameEventTypes;
    }
}