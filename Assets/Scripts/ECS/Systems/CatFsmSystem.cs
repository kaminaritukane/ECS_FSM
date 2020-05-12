using Unity.Entities;

namespace ECS
{
    class CatFsmSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecb;

        private EntityQuery _catWithoutFsmQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            _ecb = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

            _catWithoutFsmQuery = GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { ComponentType.ReadOnly<CatFiniteStateMachine>() },
                All = new ComponentType[] { ComponentType.ReadOnly<Cat>() }
            });
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _ecb.CreateCommandBuffer();

            var count = _catWithoutFsmQuery.CalculateChunkCount();
            if (count > 0)
            {
                Entities
                    .WithStoreEntityQueryInField(ref _catWithoutFsmQuery)
                    .ForEach((Entity entity) => {
                        commandBuffer.AddComponent<CatFiniteStateMachine>(entity);

                        // start from play
                        commandBuffer.AddComponent<FsmStateChanged>(entity);
                        commandBuffer.SetComponent(entity, new FsmStateChanged
                        {
                            from = FsmState.Null,
                            to = FsmState.Play
                        });
                    }).Run();
            }

            var ecbConcurrent = commandBuffer.ToConcurrent();

            Entities.ForEach((Entity entity,
                int entityInQueryIndex,
                ref Cat cat,
                in FsmStateChanged stateChanged) =>
            {
                switch( stateChanged.from )
                {
                    case FsmState.Play:
                        ecbConcurrent.RemoveComponent<PlayState>(entityInQueryIndex, entity);
                        break;
                    case FsmState.Sleep:
                        ecbConcurrent.RemoveComponent<SleepState>(entityInQueryIndex, entity);
                        break;
                    case FsmState.Eat:
                        ecbConcurrent.RemoveComponent<EatState>(entityInQueryIndex, entity);
                        break;
                }

                switch( stateChanged.to )
                {
                    case FsmState.Play:
                        {
                            ecbConcurrent.AddComponent<PlayState>(entityInQueryIndex, entity);
                            ecbConcurrent.SetComponent(entityInQueryIndex, entity, new PlayState
                            {
                                hungerCostPerSecond = 2f,
                                tirednessCostPerSecond = 4f
                            });
                        }
                        break;
                    case FsmState.Eat:
                        {
                            ecbConcurrent.AddComponent<EatState>(entityInQueryIndex, entity);
                            ecbConcurrent.SetComponent(entityInQueryIndex, entity, new EatState
                            {
                                hungerRecoverPerSecond = 5f
                            });
                        }
                        break;
                    case FsmState.Sleep:
                        {
                            ecbConcurrent.AddComponent<SleepState>(entityInQueryIndex, entity);
                            ecbConcurrent.SetComponent(entityInQueryIndex, entity, new SleepState
                            {
                                tirednessRecoverPerSecond = 3f
                            });
                        }
                        break;
                }
                cat.currentState = stateChanged.to;

                ecbConcurrent.RemoveComponent<FsmStateChanged>(entityInQueryIndex, entity);

            }).ScheduleParallel();

            _ecb.AddJobHandleForProducer(Dependency);
        }
    }
}
