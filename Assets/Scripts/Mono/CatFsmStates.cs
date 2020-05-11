using System;
using Unity.Mathematics;
using UnityEngine;

namespace Mono
{
    public interface IFsmState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
        Type ShoudExit();
    }

    public class PlayState : IFsmState
    {
        private readonly CatBehavior _catBehavior = null;

        private readonly float _hungerCostPerSecond = 2.0f;
        private readonly float _tirednessCostPerSecond = 4.0f;

        public PlayState(CatBehavior behavior)
        {
            _catBehavior = behavior;
        }

        public void OnEnter()
        {
            Debug.Log("Start to play");
        }

        public void OnUpdate()
        {
            _catBehavior.Hunger = math.clamp(
                _catBehavior.Hunger + _hungerCostPerSecond * Time.deltaTime, 0f, 100f);

            _catBehavior.Tireness = math.clamp(
                _catBehavior.Tireness + _tirednessCostPerSecond * Time.deltaTime, 0f, 100f);
        }
        
        public void OnExit()
        {
            Debug.Log("End playing");
        }

        public Type ShoudExit()
        {
            if (_catBehavior.Tireness > 40f)
            {
                return typeof(SleepState);
            }
            else if (_catBehavior.Hunger > 60f)
            {
                return typeof(EatState);
            }

            return null;
        }
    }

    public class SleepState : IFsmState
    {
        private readonly CatBehavior _catBehavior = null;

        private readonly float _tirednessRecoverPerSecond = 3.0f;

        public SleepState(CatBehavior behavior)
        {
            _catBehavior = behavior;
        }

        public void OnEnter()
        {
            Debug.Log("Start to Sleep");
        }

        public void OnUpdate()
        {
            _catBehavior.Tireness = math.clamp(
                _catBehavior.Tireness - _tirednessRecoverPerSecond * Time.deltaTime, 0f, 100f);
        }

        public void OnExit()
        {
            Debug.Log("End Sleeping");
        }

        public Type ShoudExit()
        {
            if (_catBehavior.Tireness < 10f)
            {
                return typeof(PlayState);
            }
            return null;
        }
    }

    public class EatState : IFsmState
    {
        private readonly CatBehavior _catBehavior = null;

        private readonly float _hungerRecoverPerSecond = 5.0f;

        public EatState(CatBehavior behavior)
        {
            _catBehavior = behavior;
        }
        public void OnEnter()
        {
            Debug.Log("Start to Eat");
        }

        public void OnUpdate()
        {
            _catBehavior.Hunger = math.clamp(
                _catBehavior.Hunger - _hungerRecoverPerSecond * Time.deltaTime, 0f, 100f);
        }

        public void OnExit()
        {
            Debug.Log("End Eating");
        }

        public Type ShoudExit()
        {
            // once it starts to eat, it will not stop until it's full
            if (_catBehavior.Hunger <= 0f)
            {
                return typeof(PlayState);
            }

            return null;
        }
    }
}
