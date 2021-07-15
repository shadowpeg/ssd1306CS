using System;
using WPICore;
using WPISPI;
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
		    spi = new (0, 500000);
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
