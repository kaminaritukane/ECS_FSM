using Unity.Entities;

namespace ECS.Utility
{
    public abstract class FsmStateSystemBase : SystemBase
    {
        protected abstract void OnStateEnter(EntityCommandBuffer ecb);
        protected abstract void OnStateUpdate(EntityCommandBuffer ecb);
        protected abstract void ShouldExit(EntityCommandBuffer ecb);
        protected abstract void OnStateExit(EntityCommandBuffer ecb);

        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            _endSimulationECBSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var endSimulationCMB = _endSimulationECBSystem.CreateCommandBuffer();

            OnStateEnter(endSimulationCMB);
            OnStateUpdate(endSimulationCMB);
            ShouldExit(endSimulationCMB);
            OnStateExit(endSimulationCMB);
        }
    }
}