
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

        public static TerrainType Plains1 = new TerrainType("plains1", true, true);
        public static TerrainType Plains2 = new TerrainType("plains2", true, true);
        public static TerrainType Hills1 = new TerrainType("hills1", true, true);
        public static TerrainType Hills2 = new TerrainType("hills2", true, true);
        public static TerrainType Mountains = new TerrainType("mountains", true, true);

    }
}
