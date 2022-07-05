using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    public class Node
    {
        public readonly string Name;
        public HashSet<NodeConnection> Nodes { get; set; } = new HashSet<NodeConnection>();
        public bool Visited { get; set; }
        public int? DistanceFromStart { get; set; }
        public NodeConnection NearestToStart { get; set; }

        public Node(string name)
        {
            Name = name;
        }

        public bool AddConnection(Node to, decimal rate)
        {
            return Nodes.Add(new NodeConnection(to, rate));
        }
    }

    public class NodeConnection
    {
        public Node Node { get; set; }
        public decimal Rate { get; set; }

        public NodeConnection(Node node, decimal rate)
        {
            Node = node;
            Rate = rate;
        }

    }
}
