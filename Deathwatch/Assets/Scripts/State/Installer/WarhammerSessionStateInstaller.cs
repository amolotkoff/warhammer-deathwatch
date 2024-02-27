using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Warhammer.State
{
    public class WarhammerSessionStateInstaller : LifetimeScope
    {
        enum State
        {
            Loading,
            Game,
            Pause
        }

        [SerializeField] private State startState;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register(container =>
            {
                var stateMachine = container.Resolve<WarhammerStateMachine>();
                var loading = new WarhammerLoadingState();

                stateMachine.Add(loading);

                return loading;
            }, Lifetime.Singleton).AsSelf();

            builder.Register(container =>
            {
                var stateMachine = container.Resolve<WarhammerStateMachine>();
                var session = new WarhammerSessionState();

                stateMachine.Add(session);
                stateMachine.AddTransition<WarhammerLoadingState, WarhammerSessionState>();

                return session;
            }, Lifetime.Singleton).AsSelf();

            builder.Register(container =>
            {
                var stateMachine = container.Resolve<WarhammerStateMachine>();
                var pause = new WarhammerPauseState();

                stateMachine.Add(pause);
                stateMachine.AddTransition<WarhammerSessionState, WarhammerPauseState>(true);

                return pause;
            }, Lifetime.Singleton).AsSelf();


        }
    }
}