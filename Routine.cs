namespace TerminalEngine
{
    public struct Axis(ConsoleKey[] positive, ConsoleKey[] negative, float step = 1, float defaultValue = 0)
    {
        public ConsoleKey[] Positive = positive;
        public ConsoleKey[] Negative = negative;
        public float defaultValue = defaultValue;
        public float step = step;
        public override readonly string ToString()
        {
            return $"Axis: {{ positive:\"{Positive}\", negative:\"{Negative}\", defaultValue:\"{defaultValue}\", step:\"{step}\" }}";
        }
    }
    public static class Routine
    {
        public delegate void StartFunction();
        public delegate void UpdateFunction();
        public static bool IsRunning { get; private set; } = true;
        public static void Quit()
        {
            IsRunning = false;
        }
        public static void BeginRoutine(StartFunction startFunc, UpdateFunction updateFunc)
        {
            Pixel[] localPixels = Canvas.Pixels;
            IDrawable[] localDrawings = Canvas.Drawable;

            startFunc();

            while (IsRunning)
            {
                Canvas.Draw();

                updateFunc();

                Canvas.Pixels = localPixels;
                Canvas.Drawable = localDrawings;
            }
        }
        public static ConsoleKey? GetKeyDown()
        {
            if (!Console.KeyAvailable) return null;
            return Console.ReadKey().Key;
        }
        public static bool IsKeyDown(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey().Key == key;
        } 
        public static float Get1DFromAxis(Axis axis)
        {
            float dir = axis.defaultValue;
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (axis.Positive.Any(x => x == key))
                {
                    dir += axis.step;
                }
                else if (axis.Negative.Any(x => x == key))
                {
                    dir -= axis.step;
                }
            }
            return dir;
        }
        public static float2 Get2DFromAxis(Axis horizontal, Axis vertical)
        {
            float XDir = horizontal.defaultValue;
            float YDir = vertical.defaultValue;
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (horizontal.Positive.Any(x => x == key))
                {
                    XDir += horizontal.step;
                }
                else if (horizontal.Negative.Any(x => x == key))
                {
                    XDir -= horizontal.step;
                }
                if (vertical.Positive.Any(x => x == key))
                {
                    YDir += vertical.step;
                }
                else if (vertical.Negative.Any(x => x == key))
                {
                    YDir -= vertical.step;
                }
            }
            return new(XDir, YDir);
        }
    }
}