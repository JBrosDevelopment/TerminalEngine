using System.Runtime.Versioning;

namespace TerminalEngine
{
    public struct Position(int x, int y)
    {
        public int x = x;
        public int y = y;
        public override readonly string ToString()
        {
            return $"Position: {{ x:\"{x}\", y:\"{y}\" }}";
        }
    }
    public static class Canvas
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Left { get; set; }
        public static int Top { get; set; }
        public static ConsoleColor ForegroundColor { get; set; }
        public static ConsoleColor BackgroundColor { get; set; }

        [SupportedOSPlatform("windows")]
        public static bool CursorVisible { get => Console.CursorVisible; set => CursorVisible = value; }
        public static Pixel[] Pixels { get; set; } = [];
        public static IDrawable[] Drawable { get; set; } = [];
        public static uint PixelCharWidth { get; set; }
        public static void Initialize(
            int width, 
            int height, 
            ConsoleColor foreground = ConsoleColor.White, 
            ConsoleColor background = ConsoleColor.Black,
            int? left = null,
            int? top = null,
            uint pixelCharWidth = 2)
        {
            Console.WindowWidth = width;
            Width = Console.WindowWidth;
            Console.WindowHeight = height;
            Height = Console.WindowHeight;

            ForegroundColor = foreground;
            BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            if (left != null && OperatingSystem.IsWindows())
            {
                Console.WindowLeft = (int)left;
                Left = Console.WindowLeft;

            }
            if (top != null && OperatingSystem.IsWindows())
            {
                Console.WindowTop = (int)top;
                Top = Console.WindowTop;
            }

            Pixels = GeneratePixelArray(Width, Height);
            PixelCharWidth = pixelCharWidth;
        }

        public static Position GetCursorPosition()
        {
            return new Position(Console.CursorLeft, Console.CursorTop);
        }
        public static void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }
        public static void SetCursorPosition(Position position)
        {
            Console.SetCursorPosition(position.x, position.y);
        }
        public static int GetCursorSize()
        {
            return Console.CursorSize;
        }
        public static void SetCursorSize(int s)
        {
            if (s < 1 || s >= 100)
            {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            else if (OperatingSystem.IsWindows())
            {
                Console.CursorSize = s;
            }
            else
            {
                throw new Exception("Error, unsupported platform. 'Console.CursorSize' can only be modified on a Windows system");
            }
        }

        private static Pixel[] GeneratePixelArray(int w, int h)
        {
            return new Pixel[w * h].Select((pixel, index) => pixel = new Pixel(Pixel.DEFAULT_EMPTY, index, ConsoleColor.White)).ToArray();
        }
        public static Pixel GetPixelFromPosition(int x, int y)
        {
            int index = x + y * Width;
            return Pixels[index];
        }
        public static Pixel GetPixelFromPosition(Position pos)
        {
            int index = pos.x + pos.y * Width;
            return Pixels[index];
        }
        public static Pixel GetCenterPixel()
        {
            Position center = GetCenterPosition();
            return GetPixelFromPosition(center);
        }
        public static Position GetCenterPosition()
        {
            return new(Width / 2, Height / 2);
        }
        public static void Clear()
        {
            for (int i = 0; i < Pixels.Length; i++)
            { 
                Pixels[i].c = Pixel.DEFAULT_EMPTY;
            }
            Console.Clear();
        }

        public static Queue<Pixel> PixelActions { get; set; } = [];
        public static Queue<DrawableAction> DrawableActions { get; set; } = [];

        public static void Draw()
        {
            if (Pixels.Length != Width * Height)
            {
                throw new ArgumentException("The number of pixels does not match the canvas size.");
            }

            foreach (Pixel pixel in PixelActions)
            {
                DrawPixel(pixel);
            }
            foreach (DrawableAction drawing in DrawableActions)
            {
                if (drawing.LastDraw is not null)
                {
                    EraseDrawing(drawing.LastDraw);
                }
                DrawDrawing(drawing.NewDraw);
            }

            PixelActions.Clear();
            DrawableActions.Clear();
        }
        public static void DrawPixel(Pixel pixel)
        {
            Console.ForegroundColor = pixel.color;

            Console.SetCursorPosition(pixel.column, pixel.row);
            Console.Write(new string(pixel.c, (int)PixelCharWidth));

            Console.ForegroundColor = ForegroundColor;
        }
        public static void DrawPixel(Position position, char c)
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(new string(c, (int)PixelCharWidth));
        }
        public static void DrawPixel(Position position, char c, ConsoleColor color, uint charWidth)
        {
            Console.ForegroundColor = color;

            Console.SetCursorPosition(position.x, position.y);
            Console.Write(new string(c, (int)charWidth));

            Console.ForegroundColor = ForegroundColor;
        }
        public static void EraseDrawing(IDrawable draw)
        {
            for (int y = 0; y < draw.Height; y++)
            {
                for (int x = 0; x < draw.Width; x++)
                {
                    DrawPixel(new(x + draw.Left, y + draw.Top), Pixel.DEFAULT_FULL, BackgroundColor, PixelCharWidth);
                }
            }
        }
        public static void DrawDrawing(Drawable draw)
        {
            Console.ForegroundColor = draw.Color;
            for (int y = 0; y < draw.Height; y++)
            {
                for (int x = 0; x < draw.Width; x++)
                {
                    char c;
                    if (draw.FillChar != null)
                    {
                        c = (char)draw.FillChar;
                    }
                    else if (draw.SpanChars != null)
                    {
                        int index = x + y * draw.Width;
                        if (index >= draw.SpanChars.Length)
                        {
                            c = Pixel.DEFAULT_EMPTY;
                        }
                        else
                        {
                            c = draw.SpanChars[index];
                        }
                    } 
                    else
                    {
                        throw new Exception("The 'Drawable' object's 'FillChar' and 'SpanChars' are both null. Can't draw pixel char to canvas."); 
                    }

                    DrawPixel(new(x + draw.Left, y + draw.Top), c);
                }
            }
            Console.ForegroundColor = ForegroundColor;
        }
    }
    public struct DrawableAction(IDrawable? lastDraw, Drawable newDraw)
    {
        public IDrawable? LastDraw = lastDraw;
        public Drawable NewDraw = newDraw;
    }
}
