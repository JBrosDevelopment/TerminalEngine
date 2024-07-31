using TerminalEngine;

namespace Pong
{
    public class Program
    {
        public static void Main()
        {
            Canvas.Initialize(
                width: 100,
                height: 24
                );

            Routine.BeginRoutine(Start, Update);
        }
        private static Rect? redPlayer;
        private static Rect? bluePlayer;
        private static Rect? ball;
        private static Axis Vertical;
        private static readonly float speed = 2f;
        private static readonly int delayTickMax = 50;
        private static int delayTick = 0;
        private static int RedScore = 0;
        private static int BlueScore = 0;
        private static Group? redScoreLabel;
        private static Group? blueScoreLabel;
        private static float2 ballVelocity;
        private static float2 ballPosition;
        public static void Start()
        {
            Canvas.Clear();

            redPlayer = new(
                left: 2,
                top: Canvas.Height / 2 - (5 / 2),
                width: 1,
                height: 5,
                color: ConsoleColor.Red
                );
            bluePlayer = new(
                left: Canvas.Width - 3,
                top: Canvas.Height / 2 - (5 / 2),
                width: 1,
                height: 5,
                color: ConsoleColor.Blue
                );
            ball = new(
                left: Canvas.Width / 2,
                top: Canvas.Height / 2,
                width: 1,
                height: 1,
                color: ConsoleColor.Yellow
                );

            redScoreLabel = new(
                left: (Canvas.Width / 4) - 2,
                top: 0,
                width: 6,
                height: 1,
                color: ConsoleColor.Red,
                chars: ['R', 'E', 'D', ' ', '0']
                );
            blueScoreLabel = new(
                left: (Canvas.Width / 4) + (Canvas.Width / 2) - 2,
                top: 0,
                width: 6,
                height: 1,
                color: ConsoleColor.Blue,
                chars: ['B', 'L', 'U', 'E', ' ', '0']
                );

            Vertical = new Axis([ConsoleKey.S, ConsoleKey.DownArrow], [ConsoleKey.W, ConsoleKey.UpArrow]);
            ballVelocity = new(-0.08f, 0.05f * (new Random().Next(-10, 10) > 0 ? 1 : -1));
            ballPosition = ball.Position;
        }
        public static void Update()
        {
            // move shape
            float velocity = Routine.Get1DFromAxis(Vertical) * speed;
            redPlayer!.Top += (int)velocity;

            // draw board
            DrawDashes();

            // move ball
            ballPosition += ballVelocity;
            if (ballPosition.b + ball!.Height >= Canvas.Height || ballPosition.b <= 2)
            {
                ballVelocity.b = -ballVelocity.b;
            }
            ball!.Position = ballPosition;

            // check collision
            if (ballPosition.a - 1 <= redPlayer!.Right && redPlayer!.Top <= ballPosition.b && redPlayer!.Bottom >= ballPosition.b)
            {
                ballVelocity.a = -ballVelocity.a;
            }
            if (ballPosition.a + 1 >= bluePlayer!.Left && bluePlayer!.Top <= ballPosition.b && bluePlayer!.Bottom >= ballPosition.b)
            {
                ballVelocity.a = -ballVelocity.a;
            }

            // check point score
            if (ballPosition.a <= 0)
            {
                // blue scored
                BlueScore++;
                blueScoreLabel!.SpanChars = ("BLUE " + BlueScore).ToCharArray();
                // reset ball 
                ballPosition = Canvas.GetCenterPixel().position;
                ballVelocity = -ballVelocity;
            }
            if (ballPosition.a >= Canvas.Width - 1)
            {
                // blue scored
                RedScore++;
                redScoreLabel!.SpanChars = ("RED " + RedScore).ToCharArray();
                // reset ball 
                ballPosition = Canvas.GetCenterPixel().position;
                ballVelocity = -ballVelocity;
            }

            // move blue player
            float middle = bluePlayer.Bottom - (bluePlayer.Height / 2);
            float blueVelocity = 0;
            if (ballPosition.b > middle)
            {
                blueVelocity = speed;
            }
            if (ballPosition.b < middle)
            {
                blueVelocity = -speed;
            }
            if (delayTick > delayTickMax)
            {
                bluePlayer.Top += (int)blueVelocity;
                delayTick = 0;
            }
            delayTick++;
        }
        private static void DrawDashes()
        {
            Pixel[] dashes = [
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 2),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 3),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 6),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 7),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 10),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 11),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 14),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 15),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 18),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 19),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 22),
                Canvas.GetPixelFromPosition(Canvas.Width / 2, 23),
                ];
            foreach (Pixel dash in dashes)
            {
                dash.Fill();
            }
        }
    }
}