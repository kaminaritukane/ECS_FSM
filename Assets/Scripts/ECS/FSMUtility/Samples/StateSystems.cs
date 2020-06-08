using Unity.Entities;
using UnityEngine;
using static ECS.Utility.Sample.TestStates;

namespace ECS.Utility.Sample
{
    public class TestPlayStateSystem : FsmStateSystemBase
    {
        protected override void OnStateEnter(EntityCommandBuffer ecb)
        {
            Entities
                .WithNone<TestPlayStateSystemState>()
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestPlayState state) =>
                {
                    Debug.Log($"{entity} OnStateEnter Play");
                    ecb.AddComponent<TestPlayStateSystemState>(entity);
                })
                .WithoutBurst()
                .Run();
        }

        protected override void OnStateUpdate(EntityCommandBuffer ecb)
        {
            Entities
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestPlayState state,
                in TestPlayStateSystemState sysState) =>
                {
                    state.value += 1f;
                    Debug.Log($"{entity} OnStateUpdate Play {state.value}");
                })
                .WithoutBurst()
                .Run();
        }

        protected override void ShouldExit(EntityCommandBuffer ecb)
        {
            Entities
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestPlayState state,
                in TestPlayStateSystemState sysState) =>
                {
                    // Check ShouldExit
                    if (state.value >= 100f)
                    {
                        Debug.Log($"{entity} ShouldExit Play");
                        // to sleep
                        ecb.RemoveComponent<TestPlayState>(entity);
                        ecb.AddComponent<TestSleepState>(entity);
                    }
                })
                .WithoutBurst()
                .Run();
        }

        protected override void OnStateExit(EntityCommandBuffer ecb)
        {
            Entities
                .WithNone<TestPlayState>()
                .ForEach((Entity entity,
                int entityInQueryIndex,
                in TestPlayStateSystemState state) =>
                {
                    Debug.Log($"{entity} OnStateExit Play");
                    ecb.RemoveComponent<TestPlayStateSystemState>(entity);
                })
                .WithoutBurst()
                .Run();
        }
    }

    public class TestSleepStateSystem : FsmStateSystemBase
    {
        protected override void OnStateEnter(EntityCommandBuffer ecb)
        {
            // OnStateEnter
            Entities
                .WithNone<TestSleepStateSystemState>()
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestSleepState state) =>
                {
                    Debug.Log($"{entity} OnStateEnter Sleep");
                    ecb.AddComponent<TestSleepStateSystemState>(entity);
                })
                .WithoutBurst()
                .Run();
        }

        protected override void OnStateUpdate(EntityCommandBuffer ecb)
        {
            Entities
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestSleepState state,
                in TestSleepStateSystemState sysState) =>
                {
                    state.value += 1f;
                    Debug.Log($"{entity} OnStateUpdate Sleep {state.value}");
                })
                .WithoutBurst()
                .Run();
        }

        protected override void ShouldExit(EntityCommandBuffer ecb)
        {
            Entities
                .ForEach((Entity entity,
                int entityInQueryIndex,
                ref TestSleepState state,
                in TestSleepStateSystemState sysState) =>
                {
                    // Check ShouldExit
                    if (state.value >= 100f)
                    {
                        Debug.Log($"{entity} ShouldExit Sleep");
                        // to sleep
                        ecb.RemoveComponent<TestSleepState>(entity);
                        ecb.AddComponent<TestPlayState>(entity);
                    }
                })
                .WithoutBurst()
                .Run();
        }

        protected override void OnStateExit(EntityCommandBuffer ecb)
        {
            Entities
                .WithNone<TestSleepState>()
                .ForEach((Entity entity,
                int entityInQueryIndex,
                in TestSleepStateSystemState state) =>
                {
                    Debug.Log($"{entity} OnStateExit Sleep");
                    ecb.RemoveComponent<TestSleepStateSystemState>(entity);
                })
                .WithoutBurst()
                .Run();
        }
    }
}