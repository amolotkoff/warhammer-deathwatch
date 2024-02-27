using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Warhammer.Map
{
    /*
     * Digging algorythm, which digs in different directions and places nodes
     */
    public class AntGalaxyMapGenerator : IGalaxyMapGenerator
    {
        private class Part
        {
            public Vector2 Direction { get; set; }
            public Vector2 Position { get; set; }
            public List<Part> Connections { get; private set; }

            public int Index { get; private set; }

            public Part(Vector2 position, int index) : this(position, Vector2.zero, index) { }
            public Part(Vector2 position, Vector2 direction, int index)
            {
                this.Direction = direction;
                this.Position = position;
                this.Connections = new List<Part>();
            }
        }

        private Vector2 center;

        private int minPartsCount;
        private int maxPartsCount;

        private float minDistanceBetweenPartsLength;
        private float maxDistanceBetweenPartsLength;

        private int minPartsConnectionsCount;
        private int maxPartsConnectionsCount;
        private float connectionFullness;

        private Vector2 minDirectionFluctuationPartLength;
        private Vector2 maxDirectionFluctuationPartLength;

        private int magicNumber;

        public AntGalaxyMapGenerator(Vector2 center, 
                                     int minPartsCount, 
                                     int maxPartsCount,
                                     float minDistanceBetweenPartsLength,
                                     float maxDistanceBetweenPartsLength,
                                     int minPartsConnectionsCount,
                                     int maxPartsConnectionsCount,
                                     float connectionFullness,
                                     Vector2 minDirectionFluctuationPartLength,
                                     Vector2 maxDirectionFluctuationPartLength,
                                     int magicNumber)
        {
            this.center = center;
            this.minPartsCount = minPartsCount;
            this.maxPartsCount = maxPartsCount;
            this.minDistanceBetweenPartsLength = minDistanceBetweenPartsLength;
            this.maxDistanceBetweenPartsLength = maxDistanceBetweenPartsLength;
            this.minPartsConnectionsCount = minPartsConnectionsCount;
            this.maxPartsConnectionsCount = maxPartsConnectionsCount;
            this.connectionFullness = connectionFullness;
            this.minDirectionFluctuationPartLength = minDirectionFluctuationPartLength;
            this.maxDirectionFluctuationPartLength = maxDirectionFluctuationPartLength;
            this.magicNumber = magicNumber;
        }

        public Warhammer.Map.Map<Star> Produce()
        {
            var partsCount = WarhammerUtil.RandomInt(minPartsCount, maxPartsCount);
            var parts = new List<Part>() { new(center, 0) };
            
            for (int i = 1; i < partsCount; i++)
            {
                // length of creating chain of parts
                var antDigPartCount = WarhammerUtil.RandomInt(1, partsCount - i);
                // get random part from which we start digging
                var part = WarhammerUtil.RandomItem(parts);

                for(int j = 1; i < antDigPartCount; j++)
                {
                    //check nearest parts' directions for creating opposite direction
                    var nearestParts = parts.FindAll(candidatePart => (candidatePart.Position - part.Position).magnitude < Mathf.Pow(minDistanceBetweenPartsLength, 2));
                    var direction = WarhammerUtil.RandomVector().normalized;

                    foreach(var nearestPart in nearestParts)
                    {
                        direction -= nearestPart.Direction;
                        direction.Normalize();
                    }

                    direction += WarhammerUtil.RandomVectorInBounds(minDirectionFluctuationPartLength, maxDirectionFluctuationPartLength);
                    direction.Normalize();
                    direction *= WarhammerUtil.RandomFloat(minDistanceBetweenPartsLength, maxDistanceBetweenPartsLength);

                    var nextPartPosition = part.Position + direction;
                    var newPart = new Part(nextPartPosition, direction, parts.Count);

                    newPart.Connections.Add(part);
                    part.Connections.Add(newPart);

                    // check nearest parts of new generated part for 
                    var nearestPartsOfNextPart = parts.FindAll(candidatePart => (candidatePart.Position - newPart.Position).magnitude < Mathf.Pow(maxDistanceBetweenPartsLength, 2));

                    foreach (var nearestPartOfNextPart in nearestPartsOfNextPart) 
                    {
                        var isNeededForConnection = nearestPartOfNextPart.Connections.Count < minPartsConnectionsCount |
                                                    (nearestPartOfNextPart.Connections.Count < maxPartsConnectionsCount & WarhammerUtil.IsRandomInVariation(connectionFullness));

                        if (isNeededForConnection)
                        {
                            newPart.Connections.Add(nearestPartOfNextPart);
                            nearestPartOfNextPart.Connections.Add(newPart);
                        }
                    }

                    parts.Add(newPart);

                    i++;
                }
            }

            var mapNodes = new Map.Map<Star>.Node[parts.Count];
            
            //fill map with all nodes, then we can connect it
            for(int i = 0; i < mapNodes.Length; i++)
            {
                var part = parts[i];
                mapNodes[i] = new Map.Map<Star>.Node(part.Position, new Map<Star>.Node[part.Connections.Count], null);
            }

            for(int i = 0; i < mapNodes.Length; i++)
            {
                var mapNode = mapNodes[i];
                var part = parts[i];
                var partConnections = part.Connections;

                for (int j = 0; j < mapNode.Transition.Length; j++)
                {
                    var indexOfTransition = partConnections[j].Index;
                    mapNode.Transition[j] = mapNodes[indexOfTransition];
                }
            }

            return new Map<Star>(mapNodes);
        }
    }
}