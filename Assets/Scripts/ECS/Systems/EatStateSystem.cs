using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    class EatStateSystem : SystemBase
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
                in EatState eatState) =>
            {
                cat.hunger = math.clamp(
                    cat.hunger - eatState.hungerRecoverPerSecond * deltaTime, 0f, 100f);

                // check to exit current state
                if ( cat.hunger <= 0f ) // once it starts to eat, it will not stop until it's full
                {
                    // to play
                    ecb.AddComponent<FsmStateChanged>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity, new FsmStateChanged
                    {
                        from = FsmState.Eat,
                        to = FsmState.Play
                    });
                }

            }).ScheduleParallel();

            _ecb.AddJobHandleForProducer(Dependency);
        }
    }
}
