using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    public class CubeSpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate = 0.01f;

        [Header("Bloque de cubos")]
        public int gridX = 20;
        public int gridY = 20;
        public int gridZ = 20;
        public float spacing = 1.1f;

        [Header("Movimiento hacia la grilla")]
        public float cubeMoveSpeed = 5f;
    }

    public class CubeSpawnerBaker : Baker<CubeSpawnerAuthoring>
    {
        public override void Bake(CubeSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new CubeSpawnerComponent
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                spawnPos = authoring.transform.position,
                nextSpawnTime = 0.0f,
                spawnRate = authoring.spawnRate,
                gridSize = new int3(authoring.gridX, authoring.gridY, authoring.gridZ),
                spacing = authoring.spacing,
                spawnedCount = 0,
                cubeMoveSpeed = authoring.cubeMoveSpeed
            });
        }
    }
}
