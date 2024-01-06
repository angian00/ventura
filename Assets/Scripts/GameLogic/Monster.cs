using System;
using UnityEngine;

namespace Ventura.GameLogic
{
    [Serializable]
    public class Monster : Actor, ISerializationCallbackReceiver
    {
        public Monster(string name) : base(name)
        {
            //_ai = new RandomMovementAI(this);
        }
    }
}



