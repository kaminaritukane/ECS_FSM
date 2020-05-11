using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class EatStateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref Cat cat,
                in EatState eatState) =>
            {
                cat.hunger = math.clamp(
                    cat.hunger - eatState.hungerRecoverPerSecond * deltaTime, 0f, 100f);
            }).ScheduleParallel();
        }
    }
}
