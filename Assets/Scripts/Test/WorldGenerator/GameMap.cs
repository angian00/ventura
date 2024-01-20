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


    public class GameMap
    {
        public int width;
        public int height;
        public float[,] altitudes;
        public float[,] temperatures;
        public float[,] moistures;
        public TerrainType[,] terrain;

        public int[,] civilizations;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
