using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace ECS
{
    [BurstCompile]
    public partial struct CubeMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.Time.DeltaTime;

            foreach (var (cube, transform) in
                     SystemAPI.Query<RefRW<CubeComponent>, RefRW<LocalTransform>>())
            {
                float3 toTarget = cube.ValueRO.targetPos - transform.ValueRW.Position;
                float distSq = math.lengthsq(toTarget);

                // Si ya está casi en el target, no lo movemos más
                if (distSq < 0.001f)
                    continue;

                float3 dir = math.normalize(toTarget);

                transform.ValueRW.Position += dir * cube.ValueRO.moveSpeed * dt;
            }
        }
    }
}
