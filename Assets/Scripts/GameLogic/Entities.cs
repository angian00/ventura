using Ventura.GameLogic.Components;
using Ventura.Util;
using Ventura.Behaviours;

namespace Ventura.GameLogic
{


    public class Entity: GameLogicObject
    {
        protected string _name;
        public string Name { get => _name; }
        protected int _x;
        public int x { get => _x; }
        protected int _y;
        public int y { get => _y; }

        protected bool _isBlocking;
        public bool IsBlocking { get => _isBlocking; }


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

        private Inventory? _inventory;
        public Inventory Inventory { get => _inventory;  }
        //private Equipment? _equipment;


        //stats?: Stats
        //inventory?: Inventory
        //equipment?: Equipment

        public Actor(Orchestrator orch, string name) : base(name, true)
        {
            this._orch = orch;
            if (name == "player") //FIXME: use a more robust way to check if actor is player
                _ai = new PlayerAI(_orch, this);
        }

        public void Act()
        {
            if (_ai != null)
            {
                //        while (true)
                //        {
                //            var a = _ai.ChooseAction();

                //            GameDebugging.Log($"Performing ${a.GetType().Name}");
                //var actionResult = a.Perform();

                //            if (!actionResult.Success)
                //                GameDebugging.Display("WARNING - " + actionResult.Reason);

                //            //only monsters waste a turn on failed actions
                //            if (actionResult.Success || !(_ai is PlayerAI))
                // break;
                //        }

                var a = _ai.ChooseAction();
                if (a == null)
                    return;
                DebugUtils.Log($"Performing ${a.GetType().Name}");

                var actionResult = a.Perform();
                StatusLineManager.DisplayStatus(actionResult.Reason, actionResult.Success ? StatusSeverity.Normal : StatusSeverity.Warning);
            }
        }

        public void Die()
        {
        }

    }



    public class GameItem : Entity
    {
        private Container? _parent;
        public Container Parent { get => _parent; set => _parent = value;  }

        private Consumable? _consumable;
        public Consumable Consumable { get => _consumable; }
        //private Equippable? _equippable;
        //private Combinable? _combinable;

        public GameItem(string name) : base(name, false)
        { }
    }

#nullable enable
    public class Site : Entity
    {
        private GameMap? _parent;
        public GameMap? Parent { get => _parent; }
        private string? _mapName;
        public string? MapName { get => _mapName; }

        public Site(string name, string mapName) : base(name, false)
        {
            this._mapName = mapName;
        }
    }
}
