using System;
using UnityEngine;

namespace SF.DetectionModule
{
    public enum SightShapeType
    {
        Box, Line, Arc
    }
    public class SightDetection : MonoBehaviour
    {
        [NonSerialized] public HandleData HandleData;
        public SightShapeType SightShapeType;
        public float DetectionSize = 4;
        public float StartingAngle = 0;
        public float FieldOfView = 180;
        public Color ToolHandleColor;


        private void OnValidate()
        {
            HandleData = new(DetectionSize, Vector3.forward, Vector3.right, FieldOfView, ToolHandleColor);
        }
    }
}
