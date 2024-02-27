using UnityEditor;
using UnityEngine;

namespace Warhammer.Map
{
    public class Map<T> where T : class
    {
        public Node[] Nodes { get; private set; }

        public Map(Node[] nodes)
        {
            this.Nodes = nodes;
        }

        public class Node
        {
            public Vector2 Position { get; set; }
            public T Value { get; set; }
            public Node[] Transition { get; private set; }

            public Node(Vector2 position) : this(position, null) { }
            public Node(Vector2 position, T value) : this(position, new Node[0], value) { }
            public Node(Vector2 position, Node[] transition, T value)
            {
                this.Position = position;
                this.Value = value;
                this.Transition = transition;
            }
        }
    }
}