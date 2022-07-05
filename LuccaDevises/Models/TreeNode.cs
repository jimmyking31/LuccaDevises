using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    public class TreeNode
    {
        public List<Node> Nodes = new List<Node>();


        /// <summary>
        /// Add a node to the tree.
        /// </summary>
        /// <param name="exchangeRate"></param>
        public void Add(ExchangeRate exchangeRate)
        {
            var rightNode = Nodes.Find(node => node.Name == exchangeRate.Target);
            var leftNode = Nodes.Find(node => node.Name == exchangeRate.Source);

            if (rightNode == null)
            {
                rightNode = new Node(exchangeRate.Target);
                Nodes.Add(rightNode);
            }

            if (leftNode == null)
            {
                leftNode = new Node(exchangeRate.Source);
                Nodes.Add(leftNode);
            }

            rightNode.AddConnection(leftNode, Math.Round(1 / exchangeRate.Rate, 4, MidpointRounding.AwayFromZero));
            leftNode.AddConnection(rightNode, exchangeRate.Rate);
        }

        /// <summary>
        /// Find node by name
        /// </summary>
        /// <param name="nodeName">Node name</param>
        /// <returns>Return the node</returns>
        /// <exception cref="ArgumentException"></exception>
        private Node Find(string nodeName)
        {
            Node nodeFound = Nodes.Find(n => n.Name == nodeName);
            return nodeFound ?? throw new ArgumentException(string.Format("input {0} is not present in the available rates.", nodeName));
        }

        public List<NodeConnection> ShortestPath(string source, string target)
        {
            var startNode = Find(source);
            var endNode = Find(target);

            var end = GetPathForEndNode(startNode, endNode);
            List<NodeConnection> path = new List<NodeConnection>();

            while (end != startNode)
            {
                path.Add(end.NearestToStart);
                end = end.NearestToStart.Node;
            }

            return path;
        }

        /// <summary>
        /// Find the path to the target node
        /// </summary>
        /// <param name="source">Source node</param>
        /// <param name="target">Target node</param>
        /// <returns>Target node from the source node</returns>
        /// <exception cref="ArgumentException"></exception>
        private Node GetPathForEndNode(Node source, Node target)
        {
            var queue = new List<Node>();

            Node current = source;
            current.DistanceFromStart = 0;
            queue.Add(current);

            while (queue.Count > 0)
            {
                queue = queue.OrderBy(x => x.DistanceFromStart).ToList();
                var node = queue.First();
                queue.Remove(node);

                if (node == target)
                {
                    node.Visited = true;
                    return node;
                }

                foreach (var connection in node.Nodes.Where(node => node.Node.Visited == false))
                {
                    if (connection.Node.DistanceFromStart.HasValue == false || connection.Node.DistanceFromStart + 1 < connection.Node.DistanceFromStart)
                    {
                        connection.Node.DistanceFromStart = node.DistanceFromStart + 1;
                        connection.Node.NearestToStart = new NodeConnection(node, connection.Rate);
                        if (!queue.Contains(connection.Node))
                            queue.Add(connection.Node);
                    }
                }

                node.Visited = true;
            }

            throw new ArgumentException(string.Format("No end node {0} found from starting node {1}", target, source));
        }
    }
}
