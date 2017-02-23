using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;

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
		Slider,
		StatusBarElement,
		ProgressIndicator
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
		/// This method performs the following steps:
		/// 1. Scroll to the top of the current list.
		/// 2. If search item selector is available on the current screen, click the item.
		/// 3. If search item selector is not available on the current screen, scroll to next screen until:
		///     a. The element is found and the item is clicked.
		///     b. Retry exceeds maxTryCount
		/// </summary>
		/// <param name="appDriver">App driver</param>
		/// <param name="genericListItemSelector">Selector for generic list item</param>
		/// <param name="searchItemSelector">Selector for search item</param>
		/// <param name="maxTryCount">Max try count for scrolling between pages</param>
		public static void ScrollAndSelectListItem(IOSDriver<IOSElement> appDriver, By genericListItemSelector, By searchItemSelector, int maxTryCount = 5)
		{
			int firstVisibleElementYAxis = int.MaxValue;
			int lastVisibleElementYAxis = int.MinValue;
			int elementXAxisMidPt = -1;
			int currentTryCount = 0;
			ScrollToTopOfList(appDriver);
			ReadOnlyCollection<IOSElement> displayedListItems;
			ReadOnlyCollection<IOSElement> displayedSearchItems;

			while (!IsElementDisplayed(appDriver, searchItemSelector) && (currentTryCount < maxTryCount))
			{
				if (IsElementDisplayed(appDriver, genericListItemSelector, out displayedListItems))
				{
					foreach (IOSElement element in displayedListItems)
					{
						int elementYLocation = element.Location.Y;
						if (elementYLocation > lastVisibleElementYAxis)
						{
							lastVisibleElementYAxis = elementYLocation;
						}
						if (elementYLocation < firstVisibleElementYAxis)
						{
							firstVisibleElementYAxis = elementYLocation;
						}
						elementXAxisMidPt = element.Size.Width / 2;
					}
					currentTryCount++;
					appDriver.Swipe(elementXAxisMidPt, lastVisibleElementYAxis - 20, elementXAxisMidPt, firstVisibleElementYAxis, 500); //Padding of 20 so Control center is not invoked.
				}
			}

			if (!IsElementDisplayed(appDriver, searchItemSelector, out displayedSearchItems))
			{
				Console.WriteLine("Error: Element not available");
			}
			else
			{
				displayedSearchItems[0].Click();
			}

		}

		/// <summary>
		/// This method Clicks an element
		/// </summary>
		/// <param name="element">element to be clicked</param>
		public static void Click(IOSElement element)
		{
			element.Click();
		}

		/// <summary>
		/// This method Clicks an element specified by a selector.
		/// </summary>
		/// <returns>The click.</returns>
		/// <param name="bySelector">Selector for filtering elements in app.</param>
		public static void Click(IOSDriver<IOSElement> appDriver, By bySelector)
		{
			ReadOnlyCollection<IOSElement> filteredElements;
			int indexOfFirstDisplayedElement = -1;
			if (IsElementDisplayed(appDriver, bySelector, out filteredElements))
			{
				//Clicking the first element with Displayed true
				for (int i = 0; i < filteredElements.Count; i++)
				{
					if (filteredElements[i].Displayed)
					{
						indexOfFirstDisplayedElement = i;
					}
				}
				if (indexOfFirstDisplayedElement != -1)
				{
					filteredElements[indexOfFirstDisplayedElement].Click();
				}
				else
				{
					Console.WriteLine("ERROR: Element found but not displayed");
				}
			}
			else
			{
				Console.WriteLine("ERROR: Element not found");
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
			ReadOnlyCollection<IOSElement> filteredDisplayedElements;
			if (IsElementDisplayed(appDriver, bySelector, out filteredDisplayedElements))
			{
				filteredDisplayedElements[0].SendKeys(keysToSend);
			}
			else
			{
				Console.WriteLine("ERROR: Element not found");
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
			ReadOnlyCollection<IOSElement> filteredDisplayedElements;
			while (IsElementDisplayed(appDriver, bySelector, out filteredDisplayedElements))
			{
				Click(filteredDisplayedElements[0]);
			}
		}

		/// <summary>
		/// This method returns displayed elements filtered by selector criteria
		/// </summary>
		/// <param name="appDriver">Application driver</param>
		/// <param name="selector">selector for filtering</param>
		/// <returns></returns>
		public static ReadOnlyCollection<IOSElement> FindDisplayedElements(IOSDriver<IOSElement> appDriver, By selector)
		{
			ReadOnlyCollection<IOSElement> roDisplayedElements = null;
			ReadOnlyCollection<IOSElement> filteredElementsByFindElements;
			List<IOSElement> displayedElements = new List<IOSElement>();

			try
			{
				//filteredElementsByFindElements contains all elements filtered by selector
				filteredElementsByFindElements = appDriver.FindElements(selector);

				//From filteredElementsByFindElements, only move the Displayed elements to displayedElements
				foreach (IOSElement element in filteredElementsByFindElements)
				{
					if (element.Displayed)
					{
						displayedElements.Add(element);
					}
				}
				//Create a ReadOnlyCollection<T> from displayedElements
				roDisplayedElements = new ReadOnlyCollection<IOSElement>(displayedElements);
			}
			catch (NotFoundException)
			{
				Console.WriteLine("0 elements found");
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: \n StackTrace:\n" + e.StackTrace + "\n Error Message:\n" + e.Message);
			}
			return roDisplayedElements;
		}

		/// <summary>
		/// This method returns true if any of the elements filtered by selector have availability true.
		/// </summary>
		/// <param name="appDriver">Application driver</param>
		/// <param name="selector">selector</param>
		/// <returns></returns>
		public static bool IsElementDisplayed(IOSDriver<IOSElement> appDriver, By selector)
		{
			ReadOnlyCollection<IOSElement> roDisplayedElements = null;
			bool isDisplayed = false;

			roDisplayedElements = FindDisplayedElements(appDriver, selector);
			//If Count of roDisplayedElements != 0, isDisplayed is true
			if (roDisplayedElements.Count == 0)
			{
				Console.WriteLine("0 elements found");
			}
			else
			{
				if (roDisplayedElements.Count != 1)
				{
					Console.WriteLine("Multiple elements filtered by filter criteria: " + roDisplayedElements.Count);
				}
				isDisplayed = true;
			}
			return isDisplayed;
		}

		/// <summary>
		/// This method returns true if any of the elements filtered by selector have availability true.
		/// </summary>
		/// <param name="appDriver">Application driver</param>
		/// <param name="selector">selector</param>
		/// <param name="roDisplayedElements">filtered read only displayed elements</param>
		/// <returns></returns>
		public static bool IsElementDisplayed(IOSDriver<IOSElement> appDriver, By selector, out ReadOnlyCollection<IOSElement> roDisplayedElements)
		{
			roDisplayedElements = null;
			bool isDisplayed = false;

			roDisplayedElements = FindDisplayedElements(appDriver, selector);
			//If Count of roDisplayedElements != 0, isDisplayed is true
			if (roDisplayedElements.Count == 0)
			{
				Console.WriteLine("0 elements found");
			}
			else
			{
				if (roDisplayedElements.Count != 1)
				{
					Console.WriteLine("Multiple elements filtered by filter criteria: " + roDisplayedElements.Count);
				}
				isDisplayed = true;
			}
			return isDisplayed;

		}


		/// <summary>
		/// This method waits for the attribute of selector to attain value within specified timeout
		/// </summary>
		/// <param name="appDriver">Application driver</param>
		/// <param name="selector">selector for element</param>
		/// <param name="attributeName">attribute name</param>
		/// <param name="attributeVal">attribute value to wait for</param>
		/// <param name="timeOut">timeout</param>
		/// <returns>true if element attribute reaches value within timeout, else false</returns>
		public static bool WaitForAttributeValue(IOSDriver<IOSElement> appDriver, By selector, string attributeName, string attributeVal, TimeSpan timeOut)
		{
			DateTime startDateTime = DateTime.Now;
			ReadOnlyCollection<IOSElement> roDisplayedElements;
			bool isConditionMet = false;
			do
			{
				if (IsElementDisplayed(appDriver, selector, out roDisplayedElements))
				{
					if (roDisplayedElements[0].GetAttribute(attributeName).Equals(attributeVal))
					{
						isConditionMet = true;
						break;
					}
				}
			} while (DateTime.Now.Subtract(startDateTime).CompareTo(timeOut) < 0);
			return isConditionMet;
		}

		/// <summary>
		/// This method gets the XPath including the attribute filter.
		/// </summary>
		/// <returns>The XPath.</returns>
		/// <param name="elementType">Element type.</param>
		/// <param name="attributeFilterType">Attribute type.</param>
		/// <param name="attributeFilterVal">Attribute name.</param>
		public static string GetXPath(eGUIElementType elementType, eGUIElementFilterByAttributeType attributeFilterType, string attributeFilterVal)
		{
			string xpath = string.Empty;

			xpath = GetXPathForElement(elementType) + GetXPathAttributeFilterText(attributeFilterType, attributeFilterVal);
			Console.WriteLine("xpath = " + xpath);

			return xpath;
		}

		#region Private Methods

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
				case eGUIElementType.Slider:
					xpathForElement = "//XCUIElementTypeSlider";
					break;
				case eGUIElementType.StatusBarElement:
					xpathForElement = "//XCUIElementTypeStatusBar//XCUIElementTypeOther";
					break;
				case eGUIElementType.ProgressIndicator:
					xpathForElement = "//XCUIElementTypeProgressIndicator";
					break;
				default:
					Console.WriteLine("ERROR: Unknown elementType: " + elementType.ToString());
					break;
			}
			return xpathForElement;
		}

		/// <summary>
		/// This method clicks the status bar. This scrolls the list to the top.
		/// </summary>
		/// <param name="appDriver">App driver</param>
		private static void ScrollToTopOfList(IOSDriver<IOSElement> appDriver)
		{
			ReadOnlyCollection<IOSElement> statusBarElements = FindDisplayedElements(appDriver, By.XPath(GetXPath(eGUIElementType.StatusBarElement, eGUIElementFilterByAttributeType.None, string.Empty)));
			Click(statusBarElements[0]);
		}

		#endregion

	}
}
