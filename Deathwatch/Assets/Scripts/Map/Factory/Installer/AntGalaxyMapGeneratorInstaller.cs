using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Warhammer.Map
{
    public class AntGalaxyMapGeneratorInstaller : LifetimeScope
    {
        [SerializeField, Range(1, 100000)] private int minPlanetsCount;
        [SerializeField, Range(1, 100000)] private int maxPlanetsCount;

        [Space]
        [SerializeField] private float minDistanceBetweenPlanets;
        [SerializeField] private float maxDistanceBetweenPlanets;

        [Space]
        [SerializeField] private int minConnectionBetweenPlanets;
        [SerializeField] private int maxConnectionsBetweenPlanets;
        [SerializeField, Range(0f, 1f)] private float planetsConnectionFullness;

        [Space, Header("Needed for random direction of next planet")]
        [SerializeField] private Vector2 minDirectionFluctuation;
        [SerializeField] private Vector2 maxDirectionFluctuation;

        [SerializeField] private int magicNumber;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(new AntGalaxyMapGenerator(Vector2.zero,
                                                              minPlanetsCount,
                                                              maxPlanetsCount,
                                                              minDistanceBetweenPlanets,
                                                              maxDistanceBetweenPlanets,
                                                              minConnectionBetweenPlanets,
                                                              maxConnectionsBetweenPlanets,
                                                              planetsConnectionFullness,
                                                              minDirectionFluctuation,
                                                              maxDirectionFluctuation,
                                                              magicNumber)).AsImplementedInterfaces();
        }
    }
}