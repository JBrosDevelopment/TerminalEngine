#pragma warning disable IDE1006 // Naming Styles
namespace Main
{
    public class Pixel(char c, int index, ConsoleColor color)
    {
        public char c { get; set; } = c;
        public int index { get; internal set; } = index;
        public ConsoleColor color { get; set; } = color;

        public const char DEFAULT_FULL = '\u2588';
        public const char DEFAULT_EMPTY = ' ';

        public int row
        {
            get
            {
                GetRowAndColumn(index, Canvas.Width, out int row, out _);
                return row;
            }
        }
        public int column
        {
            get
            {
                GetRowAndColumn(index, Canvas.Width, out _, out int column);
                return column;
            }
        }
        public Position position
        {
            get
            {
                GetRowAndColumn(index, Canvas.Width, out int row, out int column);
                return new Position(column, row);
            }
        }
        private static void GetRowAndColumn(int index, int width, out int row, out int column)
        {
            row = index / width;
            column = index % width;
        }


        public void Fill()
        {
            Fill(DEFAULT_FULL);
        }
        public void Fill(char c)
        {
            this.c = c;
            Canvas.PixelActions.Enqueue(this);
        }
        public override string ToString()
        {
            return $"Pixel:{{ c:\"{c}\", index:\"{index}\", position:{{ {position} }} }}";
        }
    }
}

#pragma warning restore IDE1006 // Naming Styles