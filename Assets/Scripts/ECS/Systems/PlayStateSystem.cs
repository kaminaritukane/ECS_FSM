using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class PlayStateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Cat cat,
                in PlayState playState) =>
            {
                cat.hunger = math.clamp(
                    cat.hunger + playState.hungerCostPerSecond * deltaTime, 0f, 100f);
                cat.tiredness = math.clamp(
                    cat.tiredness + playState.tirednessCostPerSecond * deltaTime, 0f, 100f);
            }).ScheduleParallel();
        }
    }
}
