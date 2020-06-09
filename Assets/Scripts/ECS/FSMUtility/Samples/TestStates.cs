using Unity.Entities;
using UnityEngine;

namespace ECS.Utility.Sample
{
    public partial class TestStates : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<TestPlayState>(entity);
        }

        private struct TestPlayState : IComponentData
        {
            public float value;
        }

        private struct TestPlayStateSystemState : ISystemStateComponentData
        {

        }

        private struct TestSleepState : IComponentData
        {
            public float value;
        }

        private struct TestSleepStateSystemState : ISystemStateComponentData
        {

        }
    }
}