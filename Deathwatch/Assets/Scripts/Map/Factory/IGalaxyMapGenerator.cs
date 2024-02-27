using UnityEditor;
using UnityEngine;

namespace Warhammer.Map
{
    public interface IGalaxyMapGenerator
    {
        Warhammer.Map.Map<Star> Produce();
    }
}