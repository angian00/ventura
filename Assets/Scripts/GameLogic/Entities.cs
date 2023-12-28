
using System;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic
{


    public class Entity: GameLogicObject
    {
        protected string _name;
        public string Name { get { return _name; } }
        protected int _x;
        public int X { get { return _x; } }
        protected int _y;
        public int Y { get { return _y; } }

        protected bool _isBlocking;
        public bool IsBlocking { get { return _isBlocking;  } }


        protected Entity(string name, bool isBlocking = false)
        {
            this._name = name;
            //this.char = char
            //this.color = color
            this._isBlocking = isBlocking;

            this._x = 0;
            this._y = 0;
        }

        public void MoveTo(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }


    public class Actor : Entity
    {

        private Orchestrator _orch;
        private AI? _ai;

        public Orchestrator Orchestrator
        {
            get => _orch;
            set
            {
                _orch = value;
                //    if (stats)
                //    stats.Orchestrator = value

                //   if (inventory)
                //    inventory.Orchestrator = value

                //    if (equipment)
                //    equipment.Orchestrator = value

                //    if (ai)
                //    ai.Orchestrator= value
            }
        }

        //stats?: Stats
        //inventory?: Inventory
        //equipment?: Equipment

        public Actor(Orchestrator orch, string name) : base(name, true)
        {
            this._orch = orch;
            if (name == "player")
                _ai = new PlayerAI(_orch, this);
        }

        public void Act()
        {
            if (_ai != null)
            {
                //        while (true)
                //        {
                //            var a = _ai.ChooseAction();

                //            Messages.Log($"Performing ${a.GetType().Name}");
                //var actionResult = a.Perform();

                //            if (!actionResult.Success)
                //                Messages.Display("WARNING - " + actionResult.Reason);

                //            //only monsters waste a turn on failed actions
                //            if (actionResult.Success || !(_ai is PlayerAI))
                // break;
                //        }

                var a = _ai.ChooseAction();
                if (a == null)
                    return;
                Messages.Log($"Performing ${a.GetType().Name}");

                var actionResult = a.Perform();
                if (!actionResult.Success)
                    Messages.Display("WARNING - " + actionResult.Reason);

            }
        }

        public void Die()
        {
        }

    }



    public class Item : Entity
    {
        public Item(string name) : base(name, false)
        {

        }
    }

#nullable enable
    public class Site : Entity
    {
        private GameMap? _parent;
        public GameMap? Parent { get { return _parent; } }
        private string? _mapName;
        public string? MapName { get { return _mapName; } }

        public Site(string name, string mapName) : base(name, false)
        {
            this._mapName = mapName;
        }
    }
}
