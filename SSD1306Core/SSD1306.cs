using System;
using WPICore;
using WPISPI;
using System.Drawing;
namespace SSD1306Core
{
    public class SSD1306
    {
	    private static SPI spi;
	    //Init sequence array
	    private static byte[][] initSeq = new byte[][] {
		    new byte[] {0xAE},
			new byte[] {0xD5, 0x80},
			new byte[] {0xA8, 0x3F},
			new byte[] {0xD3, 0x00},
			new byte[] {0x40},
			new byte[] {0x8D, 0x14},
			new byte[] {0xA1},
			new byte[] {0xC8},
			new byte[] {0xDA, 0x12},
			new byte[] {0x81, 0xCF},
			new byte[] {0xD9, 0xF1},
			new byte[] {0xDB, 0x40},
			new byte[] {0xA4},
			new byte[] {0xA6},
			new byte[] {0xAF},
			new byte[] {0x20, 0x00}
	    };


	    private SSD1306(){
	    }

	    static SSD1306() {
		    Core.WiringPiSetup();
		    spi = new (0, 2000000);
		    Core.PinMode(8, PinMode.OUTPUT);
		    Core.PinMode(9, PinMode.OUTPUT);
		    Core.DigitalWrite(9, PinState.HIGH);
		    ResetDisplay();
		    //after following init sequence, noise may appear
		    foreach(var initData in initSeq){
			    SendCommand(initData);
		    }

	    }

	    public static void Dummy(){
	    }

	    public static void ClearDisplay(bool clearToDarkScreen = true){
		   byte clearingByte = clearToDarkScreen ? (byte)0x00 :(byte)0xFF;
		   for(int i = 0; i < 1024; i++){
			   SendData(new byte[]{clearingByte});
		   }
	    }

	    public static void DrawBitmapFrame(Bitmap bmp){
		    byte pixel = 0x00;
		    for(int i = 0; i < bmp.Height; i+=8){
			   for(int j = 0; j < bmp.Width; j++){
				   var c0 = bmp.GetPixel(j, i+0);
				   var c1 = bmp.GetPixel(j, i+1);
				   var c2 = bmp.GetPixel(j, i+2);
				   var c3 = bmp.GetPixel(j, i+3);
				   var c4 = bmp.GetPixel(j, i+4);
				   var c5 = bmp.GetPixel(j, i+5);
				   var c6 = bmp.GetPixel(j, i+6);
				   var c7 = bmp.GetPixel(j, i+7);
				   
				   var d0 = (c0.R + c0.G + c0.B) / 3;
				   var d1 = (c1.R + c1.G + c1.B) / 3;
				   var d2 = (c2.R + c2.G + c2.B) / 3;
				   var d3 = (c3.R + c3.G + c3.B) / 3;
				   var d4 = (c4.R + c4.G + c4.B) / 3;
				   var d5 = (c5.R + c5.G + c5.B) / 3;
				   var d6 = (c6.R + c6.G + c6.B) / 3;
				   var d7 = (c7.R + c7.G + c7.B) / 3;

				   pixel = d0 > 0?(byte) 1 : (byte)0;
				   pixel += d1 > 0? (byte)2 : (byte)0;
				   pixel += d2 > 0? (byte)4 : (byte)0;
				   pixel += d3 > 0? (byte)8 : (byte)0;
				   pixel += d4 > 0? (byte)16 : (byte)0;
				   pixel += d5 > 0? (byte)32 : (byte)0;
				   pixel += d6 > 0? (byte)64 : (byte)0;
				   pixel += d7 > 0? (byte)128 : (byte)0;
				  
				   SendData(new byte[]{pixel});
				   //Console.WriteLine(pixel.ToString("X2"));
				   pixel = 0x00;
			   }
		   }

	    }

	    private static void SendCommand(byte[] command){
		    Core.DigitalWrite(9, PinState.LOW); //low for command mode
		    SendData(command);
		    Core.DigitalWrite(9, PinState.HIGH);//high back for data mode
	    }

	    private static void SendData(byte[] data){ //TX only. No RX.
		    spi.SPIRxTx(data);
	    }

	    private static void ResetDisplay(){ // 8 is reset and 9 is command/data selector
		    Core.DigitalWrite(8, PinState.LOW);
		    Core.DigitalWrite(8, PinState.HIGH);
	    }

    }
}
