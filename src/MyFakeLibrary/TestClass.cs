using System;

namespace MyFakeLibrary
{
    public class TestClass
    {
        public void TestLibrary(string message)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = fg;
        }
    }
}
