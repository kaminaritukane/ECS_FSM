using Unity.Entities;
using UnityEngine;

namespace ECS
{
    public class CatAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Cat
            {
                hunger = 0,
                tiredness = 0
            });
        }
    }
}