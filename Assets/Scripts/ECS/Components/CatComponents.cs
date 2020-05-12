using Unity.Entities;

namespace ECS
{
    enum FsmState
    {
        Null,
        Eat,
        Sleep,
        Play,
    }

#pragma warning disable 0649

    struct EatState : IComponentData
    {
        public float hungerRecoverPerSecond;
    }
    struct SleepState : IComponentData
    {
        public float tirednessRecoverPerSecond;
    }
    struct PlayState : IComponentData
    {
        public float hungerCostPerSecond;
        public float tirednessCostPerSecond;
    }
    struct FsmStateChanged : IComponentData
    {
        public FsmState from;
        public FsmState to;
    }
    struct CatFiniteStateMachine : IComponentData
    {
    }

    struct Cat : IComponentData
    {
        public float hunger;// 0: not hungry, 100: hungry to death
        public float tiredness;// 0: not tired, 100: tired to death
        public FsmState currentState;
    }
#pragma warning restore 0649
}