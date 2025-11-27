using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    public struct CubeSpawnerComponent : IComponentData
    {
        public Entity prefab;
        public float3 spawnPos;

        public float nextSpawnTime;
        public float spawnRate;

        public int3 gridSize;      
        public float spacing;      
        public int spawnedCount;   
        public float cubeMoveSpeed;
    }
}
