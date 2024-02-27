using System;
using UnityEditor;
using UnityEngine;

namespace Warhammer.State
{
    public interface IActivateStateListener
    {
        public void AddActivateListener(Action callbackListener);
        public void RemoveActivateListener(Action callbackListener);
    }
}