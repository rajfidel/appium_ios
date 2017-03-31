using System;
namespace Fidlee.Appium
{
	public static class Config
	{
		public static string PLATFORM_NAME = "iOS";
		public static string BUNDLE_ID = "";// "com.example.apple-samplecode.UICatalog";
		public static string DEVICE_NAME = "iPhone";
		public static string UDID = "";					//Update the UDID of the Phone
		public static string AUTOMATION_NAME = "XCUITest";
		public static string SERVER_URL = "http://127.0.0.1:4723/wd/hub";	//Update the ip address / port if Appium server is running on a remote server
		public static TimeSpan IMPLICITLY_WAIT_5SEC = TimeSpan.FromSeconds(5);	
	}
}
