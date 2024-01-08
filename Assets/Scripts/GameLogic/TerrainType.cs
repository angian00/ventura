namespace Ventura.GameLogic
{
    public record TerrainDef : GameLogicObject
    {
        public enum TerrainType
        {
            Water,
            Plains1,
            Plains2,
            Hills1,
            Hills2,
            Mountains,
        }


        private TerrainType _type;
        public TerrainType Type { get => _type; }

        private string _name;
        public string Name { get => _name; }

        private string _label;
        public string Label { get => _label; }

        private bool _walkable;
        public bool Walkable { get => _walkable; }

        private bool _transparent;
        public bool Transparent { get => _transparent; }

        private TerrainDef(TerrainType type, string name, string label, bool walkable, bool transparent)
        {
            this._type = type;
            this._name = name;
            this._label = label;
            this._walkable = walkable;
            this._transparent = transparent;
        }

        public static TerrainDef Water = new TerrainDef(TerrainType.Water, "water", "Water", false, true);
        public static TerrainDef Plains1 = new TerrainDef(TerrainType.Plains1, "plains1", "Plains", true, true);
        public static TerrainDef Plains2 = new TerrainDef(TerrainType.Plains2, "plains2", "Plains", true, true);
        public static TerrainDef Hills1 = new TerrainDef(TerrainType.Hills1, "hills1", "Hills", true, true);
        public static TerrainDef Hills2 = new TerrainDef(TerrainType.Hills2, "hills2", "Hills", true, true);
        public static TerrainDef Mountains = new TerrainDef(TerrainType.Mountains, "mountains", "Mountains", true, true);


        public static TerrainDef FromType(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Water:
                    return Water;
                case TerrainType.Plains1:
                    return Plains1;
                case TerrainType.Plains2:
                    return Plains2;
                case TerrainType.Hills1:
                    return Hills1;
                case TerrainType.Hills2:
                    return Hills2;
                case TerrainType.Mountains:
                    return Mountains;

                default:
                    throw new GameException($"Invalid TerrainType: {terrainType}");
            }
        }
    }
}
