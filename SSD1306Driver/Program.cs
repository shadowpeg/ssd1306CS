using System;
using SSD1306Core;
namespace SSD1306Driver
{
    class Program
    {
        static void Main(string[] args)
        {
		SSD1306.Dummy();
		SSD1306.ClearDisplay();
        }
    }
}
