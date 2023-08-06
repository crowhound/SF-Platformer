namespace SF.Physics
{
    [System.Serializable]
    public struct MovementProperties
    {
        [UnityEngine.Header("Gound Physics")]
        public float GroundSpeed;
        public float GroundAcceleration;
        public float GroundMaxSpeed;

        [UnityEngine.Header("Air Physics")]
        public float GravityScale;
        public float GravityAcceleration;
        public float TerminalVelocity;
        public float MaxUpForce;

        [UnityEngine.Header("Jump Physics")]
        public float JumpHeight;

        public MovementProperties(
            float _groundSpeed = 5f,
            float _groundAcceleration = 1f,
            float _groundMaxSpeed = 10f,
            float _gravityScale = 1,
            float _gravityAcceleration = 1f,
            float _terminalVelocity = 20f,
            float _maxUpForce = 20f,
            float _jumpheight = 8f)
        {
            GroundSpeed = _groundSpeed;
            GroundAcceleration = _groundAcceleration;
            GroundMaxSpeed = _groundMaxSpeed;

            GravityScale = _gravityScale;
            GravityAcceleration = _gravityAcceleration;
            TerminalVelocity = _terminalVelocity;
            MaxUpForce = _maxUpForce;

            JumpHeight = _jumpheight;
        }
    }
}