using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class SleepStateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Cat cat,
                in SleepState sleepState) =>
            {
                cat.tiredness = math.clamp(
                    cat.tiredness - sleepState.tirednessRecoverPerSecond * deltaTime, 0f, 100f);
            }).ScheduleParallel();
        }
    }
}
