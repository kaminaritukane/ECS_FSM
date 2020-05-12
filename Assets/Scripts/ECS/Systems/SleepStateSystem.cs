using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class SleepStateSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecb;

        protected override void OnCreate()
        {
            base.OnCreate();

            _ecb = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            var ecb = _ecb.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((Entity entity,
                int entityInQueryIndex,
                ref Cat cat,
                in SleepState sleepState) =>
            {
                cat.tiredness = math.clamp(
                    cat.tiredness - sleepState.tirednessRecoverPerSecond * deltaTime, 0f, 100f);

                // check to exit current state
                if ( cat.tiredness < 10f )
                {
                    // to play
                    ecb.AddComponent<FsmStateChanged>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new FsmStateChanged
                    {
                        from = FsmState.Sleep,
                        to = FsmState.Play
                    });
                }

            }).ScheduleParallel();

            _ecb.AddJobHandleForProducer(Dependency);
        }
    }
}
