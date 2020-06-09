using Unity.Entities;

namespace ECS.Utility
{
    public abstract class FsmStateSystemBase : SystemBase
    {
        protected abstract void OnStateEnter(EntityCommandBuffer.Concurrent ecbConcurrent);
        protected abstract void OnStateUpdate(EntityCommandBuffer.Concurrent ecbConcurrent);
        protected abstract void ShouldExit(EntityCommandBuffer.Concurrent ecbConcurrent);
        protected abstract void OnStateExit(EntityCommandBuffer.Concurrent ecbConcurrent);

        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            _endSimulationECBSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var endSimulationCMB = _endSimulationECBSystem.CreateCommandBuffer();
            var ecbConcurrent = endSimulationCMB.ToConcurrent();

            OnStateEnter(ecbConcurrent);
            OnStateUpdate(ecbConcurrent);
            ShouldExit(ecbConcurrent);
            OnStateExit(ecbConcurrent);

            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
}