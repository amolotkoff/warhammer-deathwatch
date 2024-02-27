using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Warhammer.State
{
    public abstract class WarhammerState : IState, IActivateStateListener, IDeactivateStateListener
    {
        private event Action activateListeners = default;
        private event Action deactivateListeners = default;

        public void Activate()
        {
            activateListeners();
        }

        public void Deactivate()
        {
            deactivateListeners();
        }

        public void AddActivateListener(Action callbackListener)
        {
            activateListeners += callbackListener;
        }

        public void RemoveActivateListener(Action callbackListener)
        {
            activateListeners -= callbackListener;
        }

        public void AddDeactivateListener(Action callbackListener)
        {
            deactivateListeners += callbackListener;
        }

        public void RemoveDeactivateListener(Action callbackListener)
        {
            deactivateListeners -= callbackListener;
        }
    }
}