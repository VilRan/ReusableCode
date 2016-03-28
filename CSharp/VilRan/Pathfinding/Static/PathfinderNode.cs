using System;
using System.Collections.Generic;

namespace VilRan.Pathfinding.Static
{
    public abstract class PathfinderNode : IComparable<PathfinderNode>
    {
        /// <summary>
        /// The number of the last run the Pathfinder visited this node.
        /// </summary>
        internal ulong LastVisit = 0;
        /// <summary>
        /// The previous node in the path to this node.
        /// </summary>
        internal PathfinderNode Previous;
        /// <summary>
        /// The total cost of the path to the node. Assigned by the pathfinder.
        /// </summary>
        internal double PathCost;
        /// <summary>
        /// An estimate of how far away the node is from the destination node.
        /// </summary>
        internal double Heuristic;
        /// <summary>
        /// Used for quickly checking if the node belongs to the open or closed list.
        /// </summary>
        internal PathfinderNodeStatus Status = PathfinderNodeStatus.Unvisited;

        /// <summary>
        /// Used for quickly checking if the node is on the open list.
        /// </summary>
        internal bool IsOpen { get { return Status == PathfinderNodeStatus.Open; } }
        /// <summary>
        /// Used for quickly checking if the node is on the closed list.
        /// </summary>
        internal bool IsClosed { get { return Status == PathfinderNodeStatus.Closed; } }

        /// <summary>
        /// A list of all other nodes the node is directly connected to, usually the adjacent ones.
        /// </summary>
        public abstract IEnumerable<PathfinderLink> GetNeighbors(object agent);
        /// <summary>
        /// Used by the pathfinder when necessary. You don't need to call it yourself.
        /// </summary>
        /// <param name="end"></param>
        public abstract double CalculateHeuristic(PathfinderNode end);

        public int CompareTo(PathfinderNode other)
        {
            double valueSelf = PathCost + Heuristic;
            double valueOther = other.PathCost + other.Heuristic;

            if (valueSelf > valueOther)
                return 1;
            if (valueSelf <= valueOther)
                return -1;

            return 0;
        }
    }

    public enum PathfinderNodeStatus : byte
    {
        Unvisited,
        Open,
        Closed
    }
}
