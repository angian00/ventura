
namespace Ventura.GameLogic
{
    public record TerrainType: GameLogicObject
    {
        private string _name;
        public string Name { get { return _name; }  }

        private bool _walkable;
        public bool Walkable { get { return _walkable; } }

        private bool _transparent;
        public bool Transparent { get { return _transparent; } }

        //darkTile: TerrainAspect
        //lightTile: TerrainAspect

        public TerrainType(string name, bool walkable, bool transparent)
        {
            this._name = name;
            this._walkable = walkable;
            this._transparent = transparent;
        }

        public static TerrainType Plains = new TerrainType("plains", true, true);
        public static TerrainType Mountain = new TerrainType("mountain", false, false);
    }
}
