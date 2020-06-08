using Unity.Entities;
using UnityEngine;

namespace ECS.Utility.Sample
{
    public class TestStates : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<TestPlayState>(entity);
        }

        public struct TestPlayState : IComponentData
        {
            public float value;
        }

        public struct TestPlayStateSystemState : ISystemStateComponentData
        {

        }

        public struct TestSleepState : IComponentData
        {
            public float value;
        }

        public struct TestSleepStateSystemState : ISystemStateComponentData
        {

        }
    }
}