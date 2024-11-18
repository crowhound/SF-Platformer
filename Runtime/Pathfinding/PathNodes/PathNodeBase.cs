using System.Collections.Generic;

using UnityEngine;

namespace SF.Pathfinding
{
    [System.Serializable]
    public class PathNodeBase
    {
        public float GCost; // Distance to the starting node.
        public float HCost; // Distance to the ending node.
        public float FCost => GCost + HCost; // G + H. Good way to memorize what this is: F stands for final cost.

        public PathNodeBase ParentNodeOnPath;

        /// <summary>
        /// The node position on the current grid this node is on.
        /// </summary>
        public Vector2 NodePosition;
        public Vector2 WorldPosition;
        private bool _isTraversable;
        public virtual bool IsTraversable
        {
            get { return _isTraversable; }
            set { _isTraversable = value; }
        }

        public PathNodeBase(bool isTraversable, Vector2 worldPosition,Vector2 nodePosition)
        {
            IsTraversable = isTraversable;
            WorldPosition = worldPosition;
            NodePosition = nodePosition;
        }
    }
}
