namespace Model
{
    public class Field
    {
        public int Width {  get; private set; }

        public int Height { get; private set; }

        public Cell[,] Map { get; private set; }

        public Field(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new Cell[width, height];
            for (var  i = 0; i < width; i++) 
                for (var j = 0; j < height; j++)
                    Map[i,j] = new Cell(i, j);
        }
    }
}
