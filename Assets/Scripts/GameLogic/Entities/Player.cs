﻿using System;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Player : Actor
    {
        public Player(string name) : base(name)
        {
            _inventory = new Inventory(this, 999);
            _skills = new Skills(this);
        }
    }

}