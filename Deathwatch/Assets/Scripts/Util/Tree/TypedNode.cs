using System;
using System.Collections.Generic;

namespace Warhammer.Util
{
    public class TypedNode<V>
    {
        public V Value { get; set; }

        private Dictionary<Type, TypedNode<V>> transitions;

        public TypedNode(V value)
        {
            this.Value = value;
            this.transitions = new();
        }

        public TypedNode<V> Add(V value, bool bilateral)
        {
            var nextNode = new TypedNode<V>(value);
            transitions.Add(value.GetType(), nextNode);

            if (bilateral)
                nextNode.transitions.Add(GetType(), this);

            return nextNode;
        }

        public TypedNode<V> Add(TypedNode<V> value, bool bilateral)
        {
            transitions.Add(value.Value.GetType(), value);

            if (bilateral)
                value.transitions.Add(GetType(), this);

            return value;
        }

        public bool ContainsNode(Type type) 
        {
            return transitions.ContainsKey(type);
        }

        public bool ContainsNode<T>()
        {
            return ContainsNode(typeof(T));
        }

        public TypedNode<V> Node<T>()
        {
            if (!transitions.TryGetValue(typeof(T), out var node))
                throw new KeyNotFoundException();

            return node;
        }
    }
}