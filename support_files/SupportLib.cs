using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace appium_uicatalog
{
	public enum eSwitchValue
	{
		On,
		Off
	}

	public enum eGUIElementAttribute
	{
		None,
		Name,
		Label,
		Value,
		Displayed,
		Enabled
	}

	public enum eGUIElementType
	{
		Button,
		NavBarButton,
		AlertButton,
		AlertText,
		AlertTextField,
		AlertSecureTextField,
		TableCell,
		ActionSheetButton,
		Slider,
		StatusBarElement,
		Switch,
		ProgressIndicator,
		PickerWheel
	}

	public static class SupportLib
	{

		/// <summary>
		/// Finds the elements by selector.
		/// </summary>
		/// <returns>The elements.</returns>
		/// <param name="bySelector">By selector.</param>
		public static ReadOnlyCollection<AppiumWebElement> FindElements(By bySelector)
		{
			ReadOnlyCollection<AppiumWebElement> elements = AppManager.CurrentAppDriver.FindElements(bySelector);
			return elements;
		}

		/// <summary>
		/// Gets the attribute value for an AppiumWebElement.
		/// </summary>
		/// <returns>The attribute value.</returns>
		/// <param name="element">Element.</param>
		/// <param name="attribute">Attribute.</param>
		public static string GetAttributeValue(AppiumWebElement element, eGUIElementAttribute attribute)
		{
			string attributeVal = string.Empty;
			switch (attribute)
			{
				case eGUIElementAttribute.Name:
					attributeVal = element.GetAttribute("name");
					break;
				case eGUIElementAttribute.Label:
					attributeVal = element.GetAttribute("label");
					break;
				case eGUIElementAttribute.Value:
					attributeVal = element.GetAttribute("value");
					break;
				case eGUIElementAttribute.Displayed:
					attributeVal = element.Displayed.ToString().ToLower();
					break;
				case eGUIElementAttribute.Enabled:
					attributeVal = element.Enabled.ToString().ToLower();
					break;
				default:
					Console.WriteLine("Unknonwn attribute : " + attribute.ToString());
					break;
			}
			return attributeVal;
		}

		/// <summary>
		/// Waits for attribute value to be attained.
		/// </summary>
		/// <returns><c>true</c>, if for attribute value was waited, <c>false</c> otherwise.</returns>
		/// <param name="bySelector">Selector.</param>
		/// <param name="attribute">Attribute.</param>
		/// <param name="value">Value.</param>
		/// <param name="timeoutMS">Timeout ms.</param>
		public static bool WaitForAttributeValue(By bySelector, eGUIElementAttribute attribute, string value, int timeoutMS) 
		{
			DefaultWait<AppiumDriver<AppiumWebElement>> wait = new DefaultWait<AppiumDriver<AppiumWebElement>>(AppManager.CurrentAppDriver);
			wait.PollingInterval = TimeSpan.FromMilliseconds(250);
			wait.Timeout = TimeSpan.FromMilliseconds(timeoutMS);
			wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
			wait.IgnoreExceptionTypes(typeof(TimeoutException));

			bool isSuccessful = false;
			try
			{
				isSuccessful = wait.Until<bool>((d) =>
				{
					AppiumWebElement element = d.FindElement(bySelector);
					if (GetAttributeValue(element, attribute).Equals(value))
					{
						return true;
					}
					else
					{
						throw new TimeoutException();
					}
				});
			}
			catch (Exception e)
			{
				PrintException(e);
			}

			return isSuccessful;
		}

		/// <summary>
		/// This method finds the element. Explicit wait time of 2 second is provided for this method to find the element.
		/// </summary>
		/// <returns>The element.</returns>
		/// <param name="bySelector">By selector.</param>
		public static AppiumWebElement FindElement(By bySelector)
		{
			DefaultWait<AppiumDriver<AppiumWebElement>> wait = new DefaultWait<AppiumDriver<AppiumWebElement>>(AppManager.CurrentAppDriver);
			AppiumWebElement elementFound = null;
			wait.PollingInterval = TimeSpan.FromMilliseconds(250);
			wait.Timeout = TimeSpan.FromSeconds(2);
			wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
			try
			{
				elementFound = wait.Until((d) =>
				{
					return d.FindElement(bySelector);
				});
			}
			catch (Exception e)
			{
				PrintException(e);
			}
			return elementFound;
		}



		/// <summary>
		/// Sets the switch value to On / Off.
		/// </summary>
		/// <param name="bySelector">Selector for switch.</param>
		/// <param name="value">Value to be selected on switch.</param>
		public static void SetSwitchValue(By bySelector, eSwitchValue value)
		{
			AppiumWebElement switchElement = null;

			try
			{
				switchElement = FindElement(bySelector);
				string currentSwitchValue = switchElement.GetAttribute("value");
				if ((value == eSwitchValue.Off && currentSwitchValue.Equals("1")) ||
					(value == eSwitchValue.On && currentSwitchValue.Equals("0")))
				{
					Click(switchElement);
				}
			}
			catch (Exception e)
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// Sets the picker wheel values.
		/// </summary>
		/// <param name="valuesToSelect">Values to select - all values for picker wheel have to be provided</param>
		public static void SetPickerWheelValues(params string[] valuesToSelect)
		{
			try
			{
				WaitForAttributeValue(By.XPath(GetXPath(eGUIElementType.PickerWheel)), eGUIElementAttribute.Displayed, "true", 2000);
				ReadOnlyCollection<AppiumWebElement> pickerWheelElements = FindElements(By.XPath(GetXPath(eGUIElementType.PickerWheel)));
				for (int i = 0; i < pickerWheelElements.Count; i++)
				{
					pickerWheelElements[i].SendKeys(valuesToSelect[i]);
				}
				if (pickerWheelElements.Count != valuesToSelect.Length)
				{
					Console.WriteLine("ERROR: picker wheel count and values to be selected should be equal");
				}
			}
			catch (Exception e) 
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// This method Scrolls to item.
		/// </summary>
		/// <param name="searchItemSelector">Search item selector.</param>
		/// <param name="maxTryCount">Max try count.</param>
		public static void ScrollToItem(By searchItemSelector, int maxTryCount = 5) 
		{ 
			int elementXAxisScrollPt = 10;
			int elementYAxisScrollPtStart = AppManager.CurrentAppDriver.Manage().Window.Size.Height - 20;
			int elementYAxisScrollPtEnd = 60;
			int currentTryCount = 0;
			ScrollToTopOfList();
			if (!(WaitForAttributeValue(searchItemSelector, eGUIElementAttribute.Displayed, "true", 2000)) &&
			   (currentTryCount < maxTryCount))
			{
				AppManager.CurrentAppDriver.Swipe(elementXAxisScrollPt, elementYAxisScrollPtStart, 
				                                  elementXAxisScrollPt, elementYAxisScrollPtEnd, 500);
				currentTryCount++;
			}
		}

		/// <summary>
		/// This method performs the following steps:
		/// 1. Scroll to the top of the current list.
		/// 2. If search item selector is available on the current screen, click the item.
		/// 3. If search item selector is not available on the current screen, scroll to next screen until:
		///     a. The element is found and the item is clicked.
		///     b. Retry exceeds maxTryCount
		/// </summary>
		/// <param name="searchItemSelector">Selector for search item</param>
		/// <param name="maxTryCount">Max try count for scrolling between pages</param>
		public static void ScrollAndSelectListItem(By searchItemSelector, int maxTryCount = 5)
		{
			ScrollToItem(searchItemSelector, maxTryCount);
			Click(searchItemSelector);
		}

		/// <summary>
		/// This method Clicks an element
		/// </summary>
		/// <param name="element">element to be clicked</param>
		public static void Click(AppiumWebElement element)
		{
			try
			{
				element.Click();
			}
			catch (Exception e) 
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// This method Clicks an element specified by a selector.
		/// </summary>
		/// <returns>The click.</returns>
		/// <param name="bySelector">Selector for filtering elements in app.</param>
		public static void Click(By bySelector)
		{
			try
			{
				AppiumWebElement element = FindElement(bySelector);
				Click(element);
			}
			catch (Exception e)
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// Sends the keys to the element defined by AppiumWebElement.
		/// </summary>
		/// <param name="element">Element.</param>
		/// <param name="keysToSend">Keys to send.</param>
		public static void SendKeys(AppiumWebElement element, String keysToSend)
		{
			try
			{
				element.SendKeys(keysToSend);
			}
			catch (Exception e)
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// This method sends key strokes to elements.
		/// </summary>
		/// <param name="bySelector">By selector.</param>
		/// <param name="keysToSend">Keys to send.</param>
		public static void SendKeys(By bySelector, String keysToSend)
		{
			try
			{
				AppiumWebElement element = FindElement(bySelector);
				SendKeys(element, keysToSend);
			}
			catch (Exception e)
			{
				PrintException(e);
			}
		}

		/// <summary>
		/// This method selects Date and Time on DatePicker.
		/// </summary>
		/// <param name="dateTime">Date and time.</param>
		public static void SelectDateTimeOnDatePicker(DateTime dateTime)
		{
			WaitForAttributeValue(By.XPath(GetXPath(eGUIElementType.PickerWheel)), eGUIElementAttribute.Displayed, "true", 2000);
			ReadOnlyCollection<AppiumWebElement> pickerWheelElements = FindElements(By.XPath(GetXPath(eGUIElementType.PickerWheel)));

			string monthDayStr = dateTime.ToString("MMM") + " " + dateTime.Day.ToString();
			string hoursStr = dateTime.ToString("%h");
			string minsStr = dateTime.ToString("mm");
			string ampmStr = dateTime.ToString("tt");

			SendKeys(pickerWheelElements[0], monthDayStr);
			SendKeys(pickerWheelElements[0], hoursStr);
			SendKeys(pickerWheelElements[0], minsStr);
			SendKeys(pickerWheelElements[0], ampmStr);
		}

		/// <summary>
		/// This method clicks the element until element is not available.
		/// </summary>
		/// <param name="bySelector">By selector.</param>
		public static void ClickUntilElementNotAvailable(By bySelector)
		{
			while (!WaitForAttributeValue(bySelector, eGUIElementAttribute.Displayed, "true", 2000))
			{
				AppiumWebElement element = FindElement(bySelector);
				Click(element);
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
		public static string GetXPath(eGUIElementType elementType, eGUIElementAttribute attributeFilterType, string attributeFilterVal)
		{
			string xpath = string.Empty;

			xpath = GetXPath(elementType) + GetXPathAttributeFilterText(attributeFilterType, attributeFilterVal);
			Console.WriteLine("xpath = " + xpath);

			return xpath;
		}

		/// <summary>
		/// This method gets the XPath attribute filter text.
		/// </summary>
		/// <returns>The XPath attribute filter text.</returns>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="attributeVal">Attribute value.</param>
		public static string GetXPathAttributeFilterText(eGUIElementAttribute attributeType, string attributeVal)
		{
			string xpathAttributeFilterText = string.Empty;

			switch (attributeType)
			{
				case eGUIElementAttribute.Label:
					xpathAttributeFilterText = "[@label='" + attributeVal + "']";
					break;
				case eGUIElementAttribute.Name:
					xpathAttributeFilterText = "[@name='" + attributeVal + "']";
					break;
				case eGUIElementAttribute.None:
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
		public static string GetXPath(eGUIElementType elementType)
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
				case eGUIElementType.AlertText:
					xpathForElement = "//XCUIElementTypeAlert//XCUIElementTypeStaticText";
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
				case eGUIElementType.Switch:
					xpathForElement = "//XCUIElementTypeSwitch";
					break;
				case eGUIElementType.ProgressIndicator:
					xpathForElement = "//XCUIElementTypeProgressIndicator";
					break;
				case eGUIElementType.PickerWheel:
					xpathForElement = "//XCUIElementTypePickerWheel";
					break;
				default:
					Console.WriteLine("ERROR: Unknown elementType: " + elementType.ToString());
					break;
			}
			return xpathForElement;
		}

		#region Private Methods

		/// <summary>
		/// Gets the first displayed element.
		/// </summary>
		/// <returns>The first displayed element.</returns>
		/// <param name="bySelector">By selector.</param>
		private static AppiumWebElement GetFirstDisplayedElement(By bySelector) 
		{
			AppiumWebElement firstDisplayedElement = null;

			ReadOnlyCollection<AppiumWebElement> elements = FindElements(bySelector);
			foreach (AppiumWebElement element in elements) 
			{
				if (element.Displayed)
				{
					firstDisplayedElement = element;
					break;
				}
			}
			return firstDisplayedElement;
		}

		/// <summary>
		/// This method clicks the status bar. This scrolls the list to the top.
		/// </summary>
		private static void ScrollToTopOfList()
		{
			AppiumWebElement firstDisplayedStatusBarElement = GetFirstDisplayedElement(By.XPath(GetXPath(eGUIElementType.StatusBarElement)));
			Click(firstDisplayedStatusBarElement);
		}

		/// <summary>
		/// This method logs exception
		/// </summary>
		/// <param name="e">E.</param>
		private static void PrintException(Exception e)
		{
			Console.WriteLine("ERROR: " + e.Message + "\n" + e.StackTrace);
		}

		#endregion

	}
}
