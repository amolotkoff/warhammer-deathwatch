using System.Collections.Generic;
using UnityEngine;

namespace Warhammer
{
    public class WarhammerUtil
    {
        /**
         * variation is between 0 and 1
         * **/
        public static bool IsRandomInVariation(float variation)
        {
            return Random.Range(0, 1) <= variation;
        }

        public static T RandomItem<T>(List<T> collection)
        {
            if (collection.Count == 0)
                throw new System.Exception("Collection is empty: must at least have 1 item");

            var index = RandomInt(0, collection.Count);
            return collection[index];
        }

        public static Vector2 RandomVector()
        {
            return new Vector2(Random.Range(float.MinValue, float.MaxValue), Random.Range(float.MinValue, float.MaxValue));
        }

        public static Vector2 RandomVectorInBounds(Vector2 min, Vector2 max)
        {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public static float RandomFloat(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int RandomInt(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
}