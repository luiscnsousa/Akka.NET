namespace MovieStreaming
{
    using System;

    public static class ColorConsole
    {
        public static void WriteLineCyan(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Cyan);
        }

        public static void WriteLineRed(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Red);
        }

        public static void WriteLineGreen(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Green);
        }
        
        public static void WriteLineYellow(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Yellow);
        }

        private static void WriteLineInDifferentColor(string message, ConsoleColor color)
        {
            var beforeColor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.WriteLine(message);

            Console.ForegroundColor = beforeColor;
        }
    }
}
