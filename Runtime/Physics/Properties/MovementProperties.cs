namespace SF.Physics
{
    [System.Serializable]
    public struct MovementProperties
    {
        public float GroundSpeed;
        public float GroundAcceleration;
        public float GroundMaxSpeed;

        public float GravityScale;
        public float GravityAcceleration;
        public float TerminalVelocity;
        public float MaxUpForce;

        public float JumpHeight;

        public MovementProperties(
            float _groundSpeed = 5f,
            float _groundAcceleration = 1f,
            float _groundMaxSpeed = 10f,
            float _gravityScale = 1,
            float _gravityAcceleration = 9.8f,
            float _terminalVelocity = 20f,
            float _maxUpForce = 15f,
            float _jumpheight = 7f)
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