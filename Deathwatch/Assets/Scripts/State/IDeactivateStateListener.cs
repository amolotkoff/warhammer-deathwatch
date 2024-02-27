using UnityEditor;
using UnityEngine;
using System;

namespace Warhammer.State
{
    public interface IDeactivateStateListener
    {
        public void AddDeactivateListener(Action callbackListener);
        public void RemoveDeactivateListener(Action callbackListener);
    }
}