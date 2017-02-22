using System;


namespace appium_uicatalog
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			ActionSheets.Run();
			ActivityIndicators.Run();	//TODO
			AlertViews.Run();	
			Buttons.Run();		//TODO
			DatePicker.Run();
		}
	}
}
