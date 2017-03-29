using System;
namespace appium_uicatalog
{
	public static class Config
	{
		public static string PLATFORM_NAME = "iOS";
		public static string BUNDLE_ID = "com.sjm.crmd.patientappios";// "com.example.apple-samplecode.UICatalog";
		public static string DEVICE_NAME = "iPhone";
		public static string UDID = "7d1ce4873fbae7a4012d1419ed91f70b208a17de";					//Update the UDID of the Phone
		public static string AUTOMATION_NAME = "XCUITest";
		public static string SERVER_URL = "http://127.0.0.1:4723/wd/hub";	//Update the ip address / port if Appium server is running on a remote server
		public static TimeSpan IMPLICITLY_WAIT_5SEC = TimeSpan.FromSeconds(5);	
	}
}
