namespace Ventura.Test.WorldGenerating
{
    public enum TerrainType
    {
        Water,
        Desert,
        Grass,
        Forest,
        Tropical,
        Rock,
        Snow,
    }


    public class MapSector
    {
        public int landCount;
    }

    public class GameMap
    {
        public int sectorSize = 10;

        public int width;
        public int height;
        public float[,] altitudes;
        public float[,] temperatures;
        public float[,] moistures;
        public TerrainType[,] terrain;
        public MapSector[,] sectors;

        public int[,] civilizations;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void ComputeSectors()
        {
            var wSectors = width / sectorSize;
            var hSectors = height / sectorSize;

            sectors = new MapSector[wSectors, hSectors];

            for (int xSectors = 0; xSectors < wSectors; xSectors++)
            {
                for (int ySectors = 0; ySectors < hSectors; ySectors++)
                {
                    var sector = new MapSector();
                    int landCount = 0;
                    for (int x = xSectors * sectorSize; x < (xSectors + 1) * sectorSize; x++)
                    {
                        for (int y = ySectors * sectorSize; y < (ySectors + 1) * sectorSize; y++)
                        {
                            if (terrain[x, y] != TerrainType.Water)
                                landCount++;
                        }
                    }

                    sector.landCount = landCount;
                    sectors[xSectors, ySectors] = sector;
                }
            }
        }
    }
}
