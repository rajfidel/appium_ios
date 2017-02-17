using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public enum eGUIElementType
	{
		Button,
		NavBarButton,
		TableCell,
		ActionSheetButton,
	}

	public static class SupportLib
	{

		/// <summary>
		/// Sets up the app.
		/// </summary>
		/// <returns>The app.</returns>
		public static IOSDriver<IOSElement> SetupApp()
		{
			DesiredCapabilities capabilities = new DesiredCapabilities();
			capabilities.SetCapability(MobileCapabilityType.PlatformName, Config.PLATFORM_NAME);
			capabilities.SetCapability(MobileCapabilityType.DeviceName, Config.DEVICE_NAME);
			capabilities.SetCapability(IOSMobileCapabilityType.BundleId, Config.BUNDLE_ID);
			capabilities.SetCapability(MobileCapabilityType.Udid, Config.UDID);
			capabilities.SetCapability(MobileCapabilityType.AutomationName, Config.AUTOMATION_NAME);
			IOSDriver<IOSElement> appDriver = new IOSDriver<IOSElement>(new Uri(Config.SERVER_URL), capabilities, TimeSpan.FromMinutes(5));
			appDriver.Manage().Timeouts().ImplicitlyWait(Config.IMPLICITLY_WAIT_5SEC);
			return appDriver;
		}

		/// <summary>
		/// Presses objects of type XCUIElementTypeButton.
		/// </summary>
		/// <param name="buttonName">Button ID / Name.</param>
		public static void PressButton(IOSDriver<IOSElement> appDriver, string buttonName)
		{
			string xPathButton = "//XCUIElementTypeButton[@name='" + buttonName + "']";
			Console.WriteLine("Select Button by XPath: " + xPathButton);
			Click(appDriver, By.XPath(xPathButton));
		}

		/// <summary>
		/// Selects the list value.
		/// </summary>
		/// <param name="listVal">list value.</param>
		public static void SelectListValue(IOSDriver<IOSElement> appDriver, string listVal)
		{
			string xpathListValue = "//XCUIElementTypeCell//XCUIElementTypeStaticText[@label='" + listVal + "']";
			Console.WriteLine("Select List value by XPath: " + xpathListValue);
			Click(appDriver, By.XPath(xpathListValue));
		}

		/// <summary>
		/// Gets the XPaths of IOSElement read only collection.
		/// </summary>
		/// <returns>The XPaths of IOSElement read only collection.</returns>
		/// <param name="elements">Elements.</param>
		private static string GetXPathsOfIOSElementReadOnlyCollection(ReadOnlyCollection<IOSElement> elements)
		{
			string xpaths = string.Empty;
			foreach (IOSElement element in elements)
			{
				xpaths += element.ToString() + ", ";
			}
			return xpaths;
		}

		/// <summary>
		/// Click an element specified by a selector on the app.
		/// Steps performed:
		/// 1. Find all elements by selector.
		/// 2. If 
		/// </summary>
		/// <returns>The click.</returns>
		/// <param name="bySelector">Selector for filtering elements in app.</param>
		public static void Click(IOSDriver<IOSElement> appDriver, By bySelector)
		{
			ReadOnlyCollection<IOSElement> filteredElements;
			if (IsElementPresent(appDriver, bySelector, out filteredElements))
			{
				filteredElements[0].Click();
			}
		}

		/// <summary>
		/// Selects the date on date picker.
		/// </summary>
		/// <param name="date">Date to be selected.</param>
		public static void SelectDateOnDatePicker(IOSDriver<IOSElement> appDriver, DateTime date)
		{

			string monthXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[1]";
			string dayXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[2]";
			string yearXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[3]";

			if (IsElementPresent(appDriver, By.XPath(monthXpath)))
			{
				string dayStr = date.Day.ToString();
				string monthStr = date.ToString("MMMM");
				string yearStr = date.Year.ToString();

				Console.WriteLine("Send Keys : '" + dayStr + "' to XPath: '" + dayXpath + "'");
				appDriver.FindElement(By.XPath(dayXpath)).SendKeys(dayStr);

				Console.WriteLine("Send Keys : '" + monthStr + "' to XPath: '" + monthXpath + "'");
				appDriver.FindElement(By.XPath(monthXpath)).SendKeys(monthStr);

				Console.WriteLine("Send Keys : '" + yearStr + "' to XPath: '" + yearXpath + "'");
				appDriver.FindElement(By.XPath(yearXpath)).SendKeys(yearStr);

			}

		}

		/// <summary>
		/// Clicks the element until element not available.
		/// </summary>
		/// <param name="bySelector">By selector.</param>
		public static void ClickUntilElementNotAvailable(IOSDriver<IOSElement> appDriver, By bySelector)
		{
			ReadOnlyCollection<IOSElement> filteredElements;
			while (IsElementPresent(appDriver, bySelector, out filteredElements))
			{
				filteredElements[0].Click();
			}
		}

		public static string GetXPath(eGUIElementType elementType, string elementName)
		{
			string xpath = string.Empty;

			switch (elementType)
			{
				case eGUIElementType.Button:
					xpath = "//XCUIElementTypeButton[@name='" + elementName + "']";
					break;
				case eGUIElementType.NavBarButton:
					xpath = "//XCUIElementTypeNavigationBar/XCUIElementTypeButton[@name='" + elementName + "']";
					break;
				case eGUIElementType.TableCell:
					xpath = "//XCUIElementTypeCell//XCUIElementTypeStaticText[@label='" + elementName + "']";
					break;
				case eGUIElementType.ActionSheetButton:
					xpath = "//XCUIElementTypeSheet//XCUIElementTypeButton[@label='" + elementName + "']";
					break;
				default:
					Console.WriteLine("Unknown elementType: " + elementType.ToString());
					break;
			}
			return xpath;
		}

		/// <summary>
		/// Checks if an element is present.
		/// </summary>
		/// <returns><c>true</c>, if element is present, <c>false</c> otherwise.</returns>
		/// <param name="bySelector">By selector method</param>
		public static bool IsElementPresent(IOSDriver<IOSElement> appDriver, By bySelector)
		{
			bool isPresent = false;
			try
			{
				ReadOnlyCollection<IOSElement> filteredElements = appDriver.FindElements(bySelector);
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
		public static bool IsElementPresent(IOSDriver<IOSElement> appDriver, By bySelector, out ReadOnlyCollection<IOSElement> filteredElements)
		{
			filteredElements = null;
			bool isPresent = false;
			try
			{
				filteredElements = appDriver.FindElements(bySelector);
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

	}
}
