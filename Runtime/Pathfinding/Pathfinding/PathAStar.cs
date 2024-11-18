using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SF.Pathfinding
{

    /* A* Pathfinding pseudo code
     * 
     * public List<PathNodeBase> OpenNodes; // The nodes to be evaluated
     * public List<PathNodeBase> ClosedNodes; // The nodes already been evaluated
     * 
     * public PathNodeBase CurrentNode; // The current node being evaluated.
     * public PathNodeBase GoalNode;
     * 
     * PathFinding Code
     * 
     * add the starting node to the OpenNodes list.
     * 
     *      loop
     *      {
     *          CurrentNode = node in open node with the lowest F_cost.
     *          remove the CurrentNode from the OpenNode list
     *          Add CurrentNode to the ClosedNode list.
     *          
     *          if(CurrentNode == GoalNode)
     *              return;
     *              
     *          foreach NeighborNode of the CurrentNode
     *          {
     *              if(NeighborNode is not traversable or NeighborNode is in the ClosedNode list)
     *              skip to the next NeighborNode for checks.
     *              
     *              if new path to NeighboreNode is shorter OR is not in OpenNodes list
     *                  set FCost of NeighborNode
     *                  set parent of NeighberNode to CurrentNode
     
     *                  if(NeigberNode is not in OpenNodes)
     *                      Add NeighberNode to OpenNodes
     *          }
     *          
     *       }
     */

    public class PathAStar : MonoBehaviour
    {
        [SerializeField] private GridBase _grid;

        public Transform StartTransform;
        public Transform GoalTransform;

        private PathNodeBase _startNode;
        private PathNodeBase _goalNode;

        private List<PathNodeBase> _openNodes = new();
        private HashSet<PathNodeBase> _closedNodes = new();

        private void Awake()
        {
            if(_grid == null)
                _grid = GetComponent<GridBase>();
        }

        private void Start()
        {
            if(StartTransform != null && GoalTransform != null)
                FindPath(StartTransform.position, GoalTransform.position);
        }

        private void FindPath(Vector2 startPotion, Vector2 goalPosition)
        {
            if(_grid == null)
                return;

            _startNode = _grid.NodeFromWorldPoint(startPotion);
            _goalNode = _grid.NodeFromWorldPoint(goalPosition);

            _openNodes.Clear();
            _closedNodes.Clear();

            _openNodes.Add(_startNode);

            while(_openNodes.Count > 0)
            {
                PathNodeBase currentNode = _openNodes[0];

                // Set it to one because the current node was set to the 0 index already.
                for(int i = 1; i < _openNodes.Count; i++)
                {
                    
                    if(_openNodes[i].FCost < currentNode.FCost || 
                        (_openNodes[i].FCost == currentNode.FCost && _openNodes[i].HCost < currentNode.HCost))
                            currentNode = _openNodes[i];
                }

                _openNodes.Remove(currentNode);
                _closedNodes.Add(currentNode);

                // We found our goal node.
                if(currentNode == _goalNode)
                {
                    RetracePath(_startNode, _goalNode);
                    return;
                }


                foreach(PathNodeBase neighbourNode in _grid.GetNeighbours(currentNode))
                {
                    if(!neighbourNode.IsTraversable || _closedNodes.Contains(neighbourNode))
                        continue;

                    float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbourNode);

                    if(newMovementCostToNeighbour < neighbourNode.GCost || !_openNodes.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = newMovementCostToNeighbour;
                        neighbourNode.HCost = GetDistance(neighbourNode, _goalNode);
                        neighbourNode.ParentNodeOnPath = currentNode;

                        if(!_openNodes.Contains(neighbourNode))
                            _openNodes.Add(neighbourNode);
                    }
                }
            }
        }
        private void RetracePath(PathNodeBase startNode, PathNodeBase goalNode)
        {
            List<PathNodeBase> pathNodes = new();

            PathNodeBase currentNode = goalNode;

            while (currentNode != startNode)
            {
                pathNodes.Add(currentNode);
                currentNode = currentNode.ParentNodeOnPath;
            }

            pathNodes.Reverse();
            _grid.Path = pathNodes;
        }

        private float GetDistance(PathNodeBase nodeA, PathNodeBase nodeB)
        {
            float distanceX = Mathf.Abs(nodeA.NodePosition.x - nodeB.NodePosition.x);
            float distanceY = Mathf.Abs(nodeA.NodePosition.y - nodeB.NodePosition.y);

            // Handling Diagonals for longer horizontal distances
            if(distanceX > distanceY)
                return 1.4f * distanceY + (distanceX - distanceY);
            else // Handling Diagonals for longer vertical distances
                return 1.4f *distanceX + (distanceY - distanceX);
        }
    }
}
