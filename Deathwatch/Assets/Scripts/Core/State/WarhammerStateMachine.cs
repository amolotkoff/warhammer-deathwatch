using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Warhammer.Util;

namespace Warhammer.State
{
    public class WarhammerStateMachine
    {
        private LinkedList<TypedNode<WarhammerState>> stateHistory;
        private Dictionary<Type, TypedNode<WarhammerState>> states;

        public WarhammerStateMachine()
        {
            this.stateHistory = new();
            this.states = new();
        }

        public WarhammerStateMachine Add(WarhammerState state)
        {
            states.Add(state.GetType(),new TypedNode<WarhammerState>(state));
            return this;
        }

        /*
         * S0 -> S1
         */
        public WarhammerStateMachine AddTransition<S0, S1>(bool bidirectional = false) where S0 : WarhammerState where S1 : WarhammerState
        {
            if (!states.TryGetValue(typeof(S0), out var s0))
                throw new KeyNotFoundException($"State:{typeof(S0)} does not exist in state machine");

            if (!states.TryGetValue(typeof(S1), out var s1))
                throw new KeyNotFoundException($"State:{typeof(S1)} does not exist in state machine");

            s0.Add(s1, bidirectional);

            return this;
        }

        //TO-DO: bug fixes
        public NextState Of<NextState>(bool deactivateCurrentState = true) where NextState : WarhammerState
        {
            var currentState = stateHistory.Last;

            if (!states.TryGetValue(typeof(NextState), out var nextState))
                throw new KeyNotFoundException($"State:{typeof(NextState)} does not exist in state machine");

            if (stateHistory.Count == 0)
            {
                stateHistory.AddFirst(nextState);
                return (NextState)nextState.Value;
            }

            if (deactivateCurrentState)
                currentState.Value.Value.Deactivate();

            var previousState = currentState.Previous;

            if (IsSameStates(previousState.Value, nextState))
            {
                stateHistory.RemoveLast();
                return (NextState)previousState.Value.Value;
            }

            stateHistory.AddLast(nextState);

            return (NextState)nextState.Value;
        }

        private bool IsSameStates(TypedNode<WarhammerState> s0, TypedNode<WarhammerState> s1)
        {
            if (s0 == null || s1 == null)
                return false;

            return s0.Value.GetType() == s1.Value.GetType();
        }
    }
}