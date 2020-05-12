using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class PlayStateSystem : SystemBase
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
                in PlayState playState) =>
            {
                cat.hunger = math.clamp(
                    cat.hunger + playState.hungerCostPerSecond * deltaTime, 0f, 100f);
                cat.tiredness = math.clamp(
                    cat.tiredness + playState.tirednessCostPerSecond * deltaTime, 0f, 100f);

                // check to exit current state
                if (cat.tiredness > 40f)
                {
                    // to sleep
                    ecb.AddComponent<FsmStateChanged>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new FsmStateChanged
                    {
                        from = FsmState.Play,
                        to = FsmState.Sleep
                    });
                }
                else if ( cat.hunger > 60f )
                {
                    // to eat
                    ecb.AddComponent<FsmStateChanged>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new FsmStateChanged
                    {
                        from = FsmState.Play,
                        to = FsmState.Eat
                    });
                }
            }).ScheduleParallel();

            _ecb.AddJobHandleForProducer(Dependency);
        }
    }
}
