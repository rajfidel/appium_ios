using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public enum eGUIElementFilterByAttributeType
	{
		None,
		Name,
		Label
	}

	public enum eGUIElementType
	{
		Button,
		NavBarButton,
		AlertButton,
		AlertTextField,
		AlertSecureTextField,
		TableCell,
		ActionSheetButton,
	}

	public static class SupportLib
	{

		/// <summary>
		/// Sets up the app and returns initialized instance of IOSDriver
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
		/// This method returns the XPaths of IOSElement read only collection in string format.
		/// </summary>
		/// <returns>The XPaths of IOSElement read only collection in string format.</returns>
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
		/// This method Clicks an element specified by a selector.
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
		/// This method sends key strokes to elements.
		/// </summary>
		/// <param name="appDriver">App driver.</param>
		/// <param name="bySelector">By selector.</param>
		/// <param name="keysToSend">Keys to send.</param>
		public static void SendKeys(IOSDriver<IOSElement> appDriver, By bySelector, String keysToSend)
		{
			ReadOnlyCollection<IOSElement> filteredElements;
			if (IsElementPresent(appDriver, bySelector, out filteredElements))
			{
				filteredElements[0].SendKeys(keysToSend);
			}
		}

		/// <summary>
		/// This method selects Date and Time on DatePicker.
		/// </summary>
		/// <param name="appDriver">App driver.</param>
		/// <param name="dateTime">Date and time.</param>
		public static void SelectDateTimeOnDatePicker(IOSDriver<IOSElement> appDriver, DateTime dateTime)
		{
			string monthDayXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[1]";
			string hoursXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[2]";
			string minsXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[3]";
			string ampmXpath = "//XCUIElementTypeDatePicker//XCUIElementTypePickerWheel[4]";

			string monthDayStr = dateTime.ToString("MMM") + " " + dateTime.Day.ToString();
			string hoursStr = dateTime.ToString("%h");
			string minsStr = dateTime.ToString("mm");
			string ampmStr = dateTime.ToString("tt");

			Console.WriteLine("Send Keys : '" + monthDayStr + "' to XPath: '" + monthDayXpath + "'");
			SendKeys(appDriver, By.XPath(monthDayXpath), monthDayStr);

			Console.WriteLine("Send Keys : '" + hoursStr + "' to XPath: '" + hoursXpath + "'");
			SendKeys(appDriver, By.XPath(hoursXpath), hoursStr);

			Console.WriteLine("Send Keys : '" + minsStr + "' to XPath: '" + minsXpath + "'");
			SendKeys(appDriver, By.XPath(minsXpath), minsStr);

			Console.WriteLine("Send Keys : '" + ampmStr + "' to XPath: '" + ampmXpath + "'");
			SendKeys(appDriver, By.XPath(ampmXpath), ampmStr);

		}


		/// <summary>
		/// This method clicks the element until element is not available.
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


		/// <summary>
		/// This method gets the XPath including the attribute filter.
		/// </summary>
		/// <returns>The XPath.</returns>
		/// <param name="elementType">Element type.</param>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="attributeName">Attribute name.</param>
		public static string GetXPath(eGUIElementType elementType, eGUIElementFilterByAttributeType attributeType, string attributeName)
		{
			string xpath = string.Empty;

			xpath = GetXPathForElement(elementType) + GetXPathAttributeFilterText(attributeType, attributeName);
			Console.WriteLine("xpath = " + xpath);

			return xpath;
		}

		/// <summary>
		/// This method Checks if an element is present.
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
						Console.WriteLine("ERROR: Multiple elements filtered: " + GetXPathsOfIOSElementReadOnlyCollection(filteredElements));
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
		/// This method Checks if an element is present on screen, also outs filteredElements if any.
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
						Console.WriteLine("ERROR: Multiple elements filtered: " + GetXPathsOfIOSElementReadOnlyCollection(filteredElements));
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
		/// This method gets the XPath attribute filter text.
		/// </summary>
		/// <returns>The XPath attribute filter text.</returns>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="attributeVal">Attribute value.</param>
		private static string GetXPathAttributeFilterText(eGUIElementFilterByAttributeType attributeType, string attributeVal)
		{
			string xpathAttributeFilterText = string.Empty;

			switch (attributeType)
			{
				case eGUIElementFilterByAttributeType.Label:
					xpathAttributeFilterText = "[@label='" + attributeVal + "']";
					break;
				case eGUIElementFilterByAttributeType.Name:
					xpathAttributeFilterText = "[@name='" + attributeVal + "']";
					break;
				case eGUIElementFilterByAttributeType.None:
					//No filter applied.
					break;
				default:
					Console.WriteLine("ERROR: Unknown attributeType: " + attributeType.ToString());
					break;
			}
			return xpathAttributeFilterText;
		}

		/// <summary>
		/// This method gets the XPath for element.
		/// </summary>
		/// <returns>The XPath for element.</returns>
		/// <param name="elementType">Element type.</param>
		private static string GetXPathForElement(eGUIElementType elementType)
		{
			string xpathForElement = string.Empty;

			switch (elementType)
			{
				case eGUIElementType.Button:
					xpathForElement = "//XCUIElementTypeButton";
					break;
				case eGUIElementType.NavBarButton:
					xpathForElement = "//XCUIElementTypeNavigationBar//XCUIElementTypeButton";
					break;
				case eGUIElementType.AlertButton:
					xpathForElement = "//XCUIElementTypeAlert//XCUIElementTypeButton";
					break;
				case eGUIElementType.AlertTextField:
					xpathForElement = "//XCUIElementTypeAlert//XCUIElementTypeTextField";
					break;
				case eGUIElementType.AlertSecureTextField:
					xpathForElement = "//XCUIElementTypeAlert//XCUIElementTypeSecureTextField";
					break;
				case eGUIElementType.TableCell:
					xpathForElement = "//XCUIElementTypeCell//XCUIElementTypeStaticText";
					break;
				case eGUIElementType.ActionSheetButton:
					xpathForElement = "//XCUIElementTypeSheet//XCUIElementTypeButton";
					break;
				default:
					Console.WriteLine("ERROR: Unknown elementType: " + elementType.ToString());
					break;
			}
			return xpathForElement;
		}

	}
}
