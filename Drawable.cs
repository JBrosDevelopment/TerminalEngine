namespace Main
{
    public interface IDrawable
    {
        int Left { get; set; }
        int Top { get; set; }
        int Right { get; }
        int Bottom { get; }
        int Width { get; set; }
        int Height { get; set; }
        ConsoleColor Color { get; set; }
        char? FillChar { get; set; }
        char[]? SpanChars { get; set; }
        CopyDraw Copy();
    }
    public class CopyDraw : IDrawable
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; }
        public int Bottom { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char? FillChar { get; set; }
        public char[]? SpanChars { get; set; }
        public ConsoleColor Color { get; set; }
        public CopyDraw Copy() => throw new NotImplementedException();
    }
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Drawable : IDrawable, IEquatable<Drawable>
    {
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
        public Drawable() { }
        public Drawable(int left, int top, int width, int height, ConsoleColor color, char fill, char[] charsSpan)
        {
            Left = left;
            Top = top;
            Width = width * 2;
            Height = height;
            Color = color;
            FillChar = fill;
            SpanChars = charsSpan;
            Canvas.DrawableActions.Enqueue(new(null, this));
        }
        private int _left, _top, _width, _height;
        public virtual int Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    CopyDraw old = Copy();
                    _left = value < 0 ? 0 : (value + _width) > Canvas.Width ? (Canvas.Width - _width - 1) : value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        public virtual int Top
        {
            get => _top;
            set
            {
                if (_top != value)
                {
                    CopyDraw old = Copy();
                    _top = value < 0 ? 0 : (value + _height) > Canvas.Height ? (Canvas.Height - _height) : value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        public virtual int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    CopyDraw old = Copy();
                    _width = value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        public virtual int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    CopyDraw old = Copy();
                    _height = value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        public virtual float2 Position
        {
            get => new(_left, _top);
            set
            {
                Left = (int)value.a;
                Top = (int)value.b;
            }
        }
        public virtual int Right => Left + Width;
        public virtual int Bottom => Top + Height;
        private char? _fillChar;
        public virtual char? FillChar
        {
            get => _fillChar;
            set
            {
                if (_fillChar != value)
                {
                    CopyDraw old = Copy();
                    _fillChar = value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        private char[]? _spanChars { get; set; }
        public char[]? SpanChars
        {
            get => _spanChars;
            set
            {
                if (_spanChars != value)
                {
                    CopyDraw old = Copy();
                    _spanChars = value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        private ConsoleColor _color;
        public virtual ConsoleColor Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    CopyDraw old = Copy();
                    _color = value;
                    Canvas.DrawableActions.Enqueue(new(old, this));
                }
            }
        }
        public virtual CopyDraw Copy()
        {
            return new CopyDraw { Left = _left, Top = _top, Width = _width, Height = _height, FillChar = FillChar, SpanChars = SpanChars, Color = _color };
        }
        public virtual void Update()
        {
            Canvas.DrawableActions.Enqueue(new(Copy(), this));
        }
        public static bool operator ==(Drawable l, Drawable r) => l.Equals(r);
        public static bool operator !=(Drawable l, Drawable r) => !l.Equals(r);
        public override bool Equals(object? obj)
        {
            return Equals(obj as Drawable);
        }
        public bool Equals(Drawable? draw)
        {
            if (draw is null)
            {
                return false;
            }
            if (draw.Width != _width) return false;
            if (draw.Height != _height) return false;
            if (draw.Left != _left) return false;
            if (draw.Top != _top) return false;
            if (draw.Color != _color) return false;
            if (FillChar is not null)
            {
                if (draw.FillChar is null) return false;
                if (draw.FillChar != FillChar) return false;
            }
            if (SpanChars is not null)
            {
                if (draw.SpanChars is null) return false;
                if (draw.SpanChars != SpanChars) return false;
            }
            return true;
        }
    }
    public class Rect : Drawable, IDrawable, IEquatable<Drawable>
    {
        public Rect(int left, int top, int width, int height, ConsoleColor color, char fill)
        {
            Left = left;
            Top = top;
            Width = width * 2;
            Height = height;
            Color = color;
            FillChar = fill;
            Canvas.DrawableActions.Enqueue(new(null, this));
        }
        public Rect(int left, int top, int width, int height, ConsoleColor color) : this(left, top, width, height, color, Pixel.DEFAULT_FULL) { }
        public Rect(Position position, int width, int height, ConsoleColor color) : this(position.x, position.y, width, height, color, Pixel.DEFAULT_FULL) { }
        public Rect(int left, int top, int width, int height) : this(left, top, width, height, Canvas.ForegroundColor, Pixel.DEFAULT_EMPTY) { }
        public Rect(int left, int top) : this(left, top, 1, 1) { }
        public Rect(Position position, int width, int height) : this(position.x, position.y, width, height) { }
        public Rect() : this(0, 0, 1, 1) { }
    }
    public class Group : Drawable, IDrawable, IEquatable<Drawable>
    {
        public Group(int left, int top, int width, int height, ConsoleColor color, char[]? chars)
        {
            Left = left;
            Top = top;
            Width = width * 2;
            Height = height;
            Color = color;

            Canvas.DrawableActions.Enqueue(new(null, this));

            if (chars is null)
            {
                SpanChars = new char[Width * Height]
                    .Select(x => x = Pixel.DEFAULT_EMPTY)
                    .ToArray();
            }
            else
            {
                SpanChars = chars;
            }
        }
        public Group(int left, int top, int width, int height, ConsoleColor color) : this(left, top, width, height, color, null) { }
        public Group(Position position, int width, int height, ConsoleColor color) : this(position.x, position.y, width, height, color, null) { }
        public Group(int left, int top, int width, int height) : this(left, top, width, height, Canvas.ForegroundColor, null) { }
        public Group(int left, int top) : this(left, top, 1, 1) { }
        public Group(Position position, int width, int height) : this(position.x, position.y, width, height) { }
        public Group() : this(0, 0, 1, 1) { }
    }
}
