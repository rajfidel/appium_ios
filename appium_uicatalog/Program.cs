using System;


namespace appium_uicatalog
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			ActionSheets.Run();
			ActivityIndicators.Run();	
			AlertViews.Run();	
			Buttons.Run();		
			DatePicker.Run();
			Sliders.Run();
		}
	}
}
