using Main;

namespace PongInTerminal
{
    public class Program
    {
        public static void Main()
        {
            // Initialize canvas and begin routine loop
            Canvas.Initialize(
                width: 60, 
                height: 20
                );
            Routine.BeginRoutine(Start, Update);
        }
        public static void Start()
        {
            // called before the update loop...

            // create a drawable object
            Rect shape = new(left: 5, top: 5, width: 10, height: 10, color: ConsoleColor.Green);

            // modify a pixel
            Canvas.GetCenterPixel().Fill();
        }
        public static void Update()
        {
            // called every frame...
        }
    }
}