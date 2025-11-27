using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS
{
    [BurstCompile]
    public partial struct CubeSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingletonEntity<CubeSpawnerComponent>(out Entity spawnerEntity))
                return;

            RefRW<CubeSpawnerComponent> spawner =
                SystemAPI.GetComponentRW<CubeSpawnerComponent>(spawnerEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            double elapsed = SystemAPI.Time.ElapsedTime;

            if (spawner.ValueRO.nextSpawnTime < elapsed)
            {
                
                int maxCount = spawner.ValueRO.gridSize.x *
                               spawner.ValueRO.gridSize.y *
                               spawner.ValueRO.gridSize.z;
                if (spawner.ValueRO.spawnedCount >= maxCount)
                {
                    ecb.Playback(state.EntityManager);
                    ecb.Dispose();
                    return;
                }

                Entity newEntity = ecb.Instantiate(spawner.ValueRO.prefab);

               
                ecb.SetComponent(newEntity,
                    LocalTransform.FromPosition(spawner.ValueRO.spawnPos));

                
                int idx = spawner.ValueRO.spawnedCount;
                int gx = spawner.ValueRO.gridSize.x;
                int gy = spawner.ValueRO.gridSize.y;
                int gz = spawner.ValueRO.gridSize.z;

                int x = idx % gx;
                int y = (idx / gx) % gy;
                int z = idx / (gx * gy);

                float spacing = spawner.ValueRO.spacing;

                
                float3 half = new float3(gx - 1, gy - 1, gz - 1) * 0.5f * spacing;
                float3 gridPos = new float3(x, y, z) * spacing;
                float3 targetPos = spawner.ValueRO.spawnPos + gridPos - half;

                ecb.AddComponent(newEntity, new CubeComponent
                {
                    targetPos = targetPos,
                    moveSpeed = spawner.ValueRO.cubeMoveSpeed
                });

                spawner.ValueRW.spawnedCount++;
                spawner.ValueRW.nextSpawnTime =
                    (float)elapsed + spawner.ValueRO.spawnRate;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
