namespace ClassLibrary1
{
    public class Field
    {
        public Cell[,] Map { get; private set; }

        public Field(int width, int height)
        {
            Map = new Cell[width, height];
            for (var  i = 0; i < width; i++) 
                for (var j = 0; j < height; j++)
                    Map[i,j] = new Cell(i, j);
        }
    }
}
