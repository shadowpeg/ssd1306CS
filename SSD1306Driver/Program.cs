using System;
using SSD1306Core;
using System.Drawing;
using System.Threading;
namespace SSD1306Driver
{
    class Program
    {
        static void Main(string[] args)
        {
		var bmp = new Bitmap("");
		SSD1306.ClearDisplay();
		SSD1306.DrawBitmapFrame(bmp);

        }
    }
}
