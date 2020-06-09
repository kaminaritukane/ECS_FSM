using Unity.Entities;
using UnityEngine;

namespace ECS.Utility.Sample
{
    public partial class TestStates
    {
        #region PlayState
        private class TestPlayStateSystem : FsmStateSystemBase
        {
            protected override void OnStateEnter(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .WithNone<TestPlayStateSystemState>()
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestPlayState state) =>
                    {
                        ecbConcurrent.AddComponent<TestPlayStateSystemState>(entityInQueryIndex, entity);
                    })
                    .Schedule();
            }

            protected override void OnStateUpdate(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .ForEach((ref TestPlayState state,
                    in TestPlayStateSystemState sysState) =>
                    {
                        state.value += 1f;
                    })
                    .Schedule();
            }

            protected override void ShouldExit(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestPlayState state,
                    in TestPlayStateSystemState sysState) =>
                    {
                        // Check ShouldExit
                        if (state.value >= 100f)
                        {
                            ecbConcurrent.RemoveComponent<TestPlayState>(entityInQueryIndex, entity);
                            ecbConcurrent.AddComponent<TestSleepState>(entityInQueryIndex, entity);
                        }
                    })
                    .Schedule();
            }

            protected override void OnStateExit(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .WithNone<TestPlayState>()
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestPlayStateSystemState state) =>
                    {
                        ecbConcurrent.RemoveComponent<TestPlayStateSystemState>(entityInQueryIndex, entity);
                    })
                    .Schedule();
            }
        }
        #endregion

        #region SleepState
        private class TestSleepStateSystem : FsmStateSystemBase
        {
            protected override void OnStateEnter(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                // OnStateEnter
                Entities
                    .WithNone<TestSleepStateSystemState>()
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestSleepState state) =>
                    {
                        ecbConcurrent.AddComponent<TestSleepStateSystemState>(entityInQueryIndex, entity);
                    })
                    .Schedule();
            }

            protected override void OnStateUpdate(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    ref TestSleepState state,
                    in TestSleepStateSystemState sysState) =>
                    {
                        state.value += 1f;
                    })
                    .Schedule();
            }

            protected override void ShouldExit(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestSleepState state,
                    in TestSleepStateSystemState sysState) =>
                    {
                        // Check ShouldExit
                        if (state.value >= 100f)
                        {
                            // to sleep
                            ecbConcurrent.RemoveComponent<TestSleepState>(entityInQueryIndex, entity);
                            ecbConcurrent.AddComponent<TestPlayState>(entityInQueryIndex, entity);
                        }
                    })
                    .Schedule();
            }

            protected override void OnStateExit(EntityCommandBuffer.Concurrent ecbConcurrent)
            {
                Entities
                    .WithNone<TestSleepState>()
                    .ForEach((Entity entity,
                    int entityInQueryIndex,
                    in TestSleepStateSystemState state) =>
                    {
                        ecbConcurrent.RemoveComponent<TestSleepStateSystemState>(entityInQueryIndex, entity);
                    })
                    .Schedule();
            }
        }
        #endregion
    }
}