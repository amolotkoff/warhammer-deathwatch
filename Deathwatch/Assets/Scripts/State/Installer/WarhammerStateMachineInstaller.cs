using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Warhammer.State
{
    public class WarhammerStateMachineInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<WarhammerStateMachine>(Lifetime.Singleton);            
        }
    }
}