
namespace Ventura.GameLogic
{
    public record TerrainType : GameLogicObject
    {
        private string _name;
        public string Name { get => _name; }

        private string _label;
        public string Label { get => _label; }

        private bool _walkable;
        public bool Walkable { get => _walkable; }

        private bool _transparent;
        public bool Transparent { get => _transparent; }

        //darkTile: TerrainAspect
        //lightTile: TerrainAspect

        public TerrainType(string name, string label, bool walkable, bool transparent)
        {
            this._name = name;
            this._label = label;
            this._walkable = walkable;
            this._transparent = transparent;
        }

        public static TerrainType Plains1 = new TerrainType("plains1", "Plains", true, true);
        public static TerrainType Plains2 = new TerrainType("plains2", "Plains", true, true);
        public static TerrainType Hills1 = new TerrainType("hills1", "Hills", true, true);
        public static TerrainType Hills2 = new TerrainType("hills2", "Hills", true, true);
        public static TerrainType Mountains = new TerrainType("mountains", "Mountains", true, true);

    }
}
