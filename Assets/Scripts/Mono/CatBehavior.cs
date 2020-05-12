using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mono
{
    public class CatBehavior : MonoBehaviour
    {
        public float Hunger = 0f; // 0: not hungry, 100: hungry to death
        public float Tireness = 0f; // 0: not tired, 100: tired to death

        private CatFsm _catFsm = null;

        private void Awake()
        {
            IFsmState[] allStates = new IFsmState[]
            {
                new PlayState(this),
                new SleepState(this),
                new EatState(this)
            };
            // starts from play
            _catFsm = new CatFsm(allStates[0], allStates);
        }

        private void Start()
        {
            _catFsm.Start();
        }

        private void Update()
        {
            _catFsm.UpdateTick();
        }
    }
}