using System;
using UnityEngine;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Monster : Actor, ISerializationCallbackReceiver
    {
        public Monster(string name) : base(name)
        {
            _ai = new RandomMovementAI(this);
            //_ai = new StaticAI(this);
        }


        // -------- Custom Serialization -------------------
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            _ai = new RandomMovementAI(this);
        }
    }
}



