using SF.Characters.Controllers;
using UnityEngine;

namespace SF
{
    public class Stomp : MonoBehaviour
    {
        private Collider2D _collider2d;
        private GroundedController2D _controller2D;
        private void Awake()
        {
            _collider2d = GetComponent<Collider2D>();
            _controller2D = GetComponent<GroundedController2D>();
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent(out IStompable stompable))
            {
                if(_collider2d.Distance(other).normal.y < 0)
                    stompable.Stomp();

                if (_controller2D == null)
                    return;

                _controller2D.SetVerticalVelocity(_controller2D.CurrentPhysics.JumpHeight);
            }
        }
    }
}
