using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.PageObjects;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace appium_uicatalog
{
	public static class SupportLib
	{

		private static IOSDriver<IOSElement> app;

		public static IOSDriver<IOSElement> App
		{
			get
			{
				if (app == null)
				{
					SetupApp();
				}
				return app;
			}
		}

		/// <summary>
		/// Sets up the app.
		/// </summary>
		/// <returns>The app.</returns>
		public static void SetupApp()
		{
			DesiredCapabilities capabilities = new DesiredCapabilities();
			capabilities.SetCapability(MobileCapabilityType.PlatformName, Config.PLATFORM_NAME);
			capabilities.SetCapability(MobileCapabilityType.DeviceName, Config.DEVICE_NAME);
			capabilities.SetCapability(IOSMobileCapabilityType.BundleId, Config.BUNDLE_ID);
			capabilities.SetCapability(MobileCapabilityType.Udid, Config.UDID);
			capabilities.SetCapability(MobileCapabilityType.AutomationName, Config.AUTOMATION_NAME);
			app = new IOSDriver<IOSElement>(new Uri(Config.SERVER_URL), capabilities, TimeSpan.FromMinutes(5));
			app.Manage().Timeouts().ImplicitlyWait(Config.IMPLICITLY_WAIT_5SEC);
		}

		/// <summary>
		/// Presses objects of type XCUIElementTypeButton.
		/// </summary>
		/// <param name="buttonName">Button ID / Name.</param>
		public static void PressButton(string buttonName)
		{
			string xPathButton = "//XCUIElementTypeButton[@name='" + buttonName + "']";
			Console.WriteLine("Select Button by XPath: " + xPathButton);
			Click(By.XPath(xPathButton));
		}

		/// <summary>
		/// Selects the list value.
		/// </summary>
		/// <param name="listVal">list value.</param>
		public static void SelectListValue(string listVal)
		{
			String xpathListValue = "//XCUIElementTypeCell//XCUIElementTypeStaticText[@label='" + listVal + "']";
			Console.WriteLine("Select List value by XPath: " + xpathListValue);
			Click(By.XPath(xpathListValue));
		}

		private static String GetXPathsOfIOSElementReadOnlyCollection(ReadOnlyCollection<IOSElement> elements)
		{
			String xpaths = string.Empty;
			foreach (IOSElement element in elements)
			{
				xpaths += element.ToString() + ", ";
			}
			return xpaths;
		}

		/// <summary>
		/// Checks if an element is present.
		/// </summary>
		/// <returns><c>true</c>, if element is present, <c>false</c> otherwise.</returns>
		/// <param name="bySelector">By selector method</param>
		public static bool IsElementPresent(By bySelector)
		{
			bool isPresent = false;
			try
			{
				ReadOnlyCollection<IOSElement> filteredElements = app.FindElements(bySelector);
				if (filteredElements.Count == 0)
				{
					Console.WriteLine("0 elements found");
				}
				else
				{
					if (filteredElements.Count != 1)
					{
						Console.WriteLine("WARNING: Multiple elements filtered: " + GetXPathsOfIOSElementReadOnlyCollection(filteredElements));
					}
					isPresent = true;
				}
			}
			catch (NotFoundException)
			{
				Console.WriteLine("0 elements found");
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: \n StackTrace:\n" + e.StackTrace + "\n Error Message:\n" + e.Message);
			}
			return isPresent;
		}

		/// <summary>
		/// Checks if an element is present, also outs filteredElements (To avoid duplicate calls to method FindElements later)
		/// </summary>
		/// <returns><c>true</c>, if element is present, <c>false</c> otherwise.</returns>
		/// <param name="bySelector">By selector method</param>
		public static bool IsElementPresent(By bySelector, out ReadOnlyCollection<IOSElement> filteredElements)
		{
			filteredElements = null;
			bool isPresent = false;
			try
			{
				filteredElements = app.FindElements(bySelector);
				if (filteredElements.Count == 0)
				{
					Console.WriteLine("0 elements found");
				}
				else
				{
					if (filteredElements.Count != 1)
					{
						Console.WriteLine("WARNING: Multiple elements filtered: " + GetXPathsOfIOSElementReadOnlyCollection(filteredElements));
					}
					isPresent = true;
				}
			}
			catch (NotFoundException)
			{
				Console.WriteLine("0 elements found");
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: \n StackTrace:\n" + e.StackTrace + "\n Error Message:\n" + e.Message);
			}
			return isPresent;
		}

		/// <summary>
		/// Click an element specified by a selector on the app.
		/// Steps performed:
		/// 1. Find all elements by selector.
		/// 2. If 
		/// </summary>
		/// <returns>The click.</returns>
		/// <param name="bySelector">Selector for filtering elements in app.</param>
		public static void Click(By bySelector)
		{
			try
			{
				ReadOnlyCollection<IOSElement> filteredElements;
				if (IsElementPresent(bySelector, out filteredElements))
				{
					filteredElements[0].Click();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: \n StackTrace:\n" + e.StackTrace + "\n Error Message:\n" + e.Message);
			}
		}



		/// <summary>
		/// Selects the date on date picker.
		/// </summary>
		/// <param name="date">Date to be selected.</param>
		public static void SelectDateOnDatePicker(DateTime date)
		{

			string monthXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[1]";
			string dayXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[2]";
			string yearXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[3]";

			if (IsElementPresent(By.XPath(monthXpath)))
			{
				String dayStr = date.Day.ToString();
				String monthStr = date.ToString("MMMM");
				String yearStr = date.Year.ToString();

				Console.WriteLine("Send Keys : '" + dayStr + "' to XPath: '" + dayXpath + "'");
				app.FindElement(By.XPath(dayXpath)).SendKeys(dayStr);

				Console.WriteLine("Send Keys : '" + monthStr + "' to XPath: '" + monthXpath + "'");
				app.FindElement(By.XPath(monthXpath)).SendKeys(monthStr);

				Console.WriteLine("Send Keys : '" + yearStr + "' to XPath: '" + yearXpath + "'");
				app.FindElement(By.XPath(yearXpath)).SendKeys(yearStr);

			}

		}

	}
}
