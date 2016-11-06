namespace MovieStreaming
{
    using System;

    public static class ColorConsole
    {
        public static void WriteLineWhite(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.White);
        }

        public static void WriteLineMagenta(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Magenta);
        }

        public static void WriteLineGray(string message)
        {
            WriteLineInDifferentColor(message, ConsoleColor.Gray);
        }

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
