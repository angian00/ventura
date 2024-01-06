using System;
using UnityEngine;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic
{
    [Serializable]
    public class Monster : Actor, ISerializationCallbackReceiver
    {
        public Monster(string name) : base(name)
        {
            _ai = new RandomMovementAI(this);
            //_ai = new StaticAI(this);
        }
    }
}



