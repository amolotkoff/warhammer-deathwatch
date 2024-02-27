using UnityEditor;
using UnityEngine;

namespace Warhammer.State
{
    public interface IState
    {
        void Activate();
        void Deactivate();
    }
}