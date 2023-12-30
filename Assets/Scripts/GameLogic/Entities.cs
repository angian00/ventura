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

        public virtual void MoveTo(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }


    public class Actor : Entity
    {

        protected Orchestrator _orch;
        protected AI? _ai;

        protected Skills? _skills;
        public Skills? Skills { get => _skills; }

        protected Inventory? _inventory;
        public Inventory Inventory { get => _inventory;  }
        //private Equipment? _equipment;


        public Actor(Orchestrator orch, string name) : base(name, true)
        {
            this._orch = orch;
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
                StatusLineManager.Instance.Display(actionResult.Reason, actionResult.Success ? StatusSeverity.Normal : StatusSeverity.Warning);
            }
        }

        //public void Die()
        //{
        //}

    }

    public class Player : Actor
    {
        public Player(Orchestrator orch, string name) : base(orch, name)
        {
            _ai = new PlayerAI(_orch, this);
            _inventory = new Inventory(this, 999);
            _skills = new Skills(this);
        }

        public override void MoveTo(int x, int y)
        {
            base.MoveTo(x, y);
            PendingUpdates.Instance.Add(PendingUpdateId.MapPlayerPos);
        }
    }


    public class GameItem : Entity
    {
        protected Container? _parent;
        public Container Parent { get => _parent; set => _parent = value;  }

        protected Consumable? _consumable;
        public Consumable Consumable { get => _consumable; }
        //protected Equippable? _equippable;
        //protected Combinable? _combinable;

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
