using System;
using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace Fidlee.Appium
{
	public static class AppManager
	{
		public static AppiumDriver<AppiumWebElement> CurrentAppDriver = null;
		private static Dictionary<string, AppiumDriver<AppiumWebElement>> appDrivers = new Dictionary<string, AppiumDriver<AppiumWebElement>>();

		public static void LaunchApp(string bundleID)
		{
			if (appDrivers.ContainsKey(bundleID))
			{
				appDrivers[bundleID].LaunchApp();
			}
			else
			{
				DesiredCapabilities capabilities = new DesiredCapabilities();
				capabilities.SetCapability(MobileCapabilityType.PlatformName, Config.PLATFORM_NAME);
				capabilities.SetCapability(MobileCapabilityType.DeviceName, Config.DEVICE_NAME);
				capabilities.SetCapability(IOSMobileCapabilityType.BundleId, bundleID);
				capabilities.SetCapability(MobileCapabilityType.Udid, Config.UDID);
				capabilities.SetCapability(MobileCapabilityType.AutomationName, Config.AUTOMATION_NAME);
				AppiumDriver<AppiumWebElement> appDriver = new IOSDriver<AppiumWebElement>(new Uri(Config.SERVER_URL), 
				                                                                           capabilities, TimeSpan.FromMinutes(5));
				appDrivers.Add(bundleID, appDriver);
				//CurrentAppDriver = appDriver;
			}
		}

		public static void CloseApp(string bundleID)
		{
			if (appDrivers.ContainsKey(bundleID))
			{
				appDrivers[bundleID].CloseApp();
				CurrentAppDriver = null;
			}
			else
			{
				Console.WriteLine("ERROR: " + bundleID + " should be launched using LaunchApp() first");
			}
		}
	}
}
