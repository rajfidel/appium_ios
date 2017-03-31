using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;

namespace Fidlee.Appium
{
	public static class VerifyLib
	{
		public static void VerifyAlertButtons(string[] expButtonsByName, params string[] reqTags)
		{
			ReadOnlyCollection<AppiumWebElement> alertButtons = AppManager.CurrentAppDriver.FindElements(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.None, string.Empty)));

			String[] actButtonsByName = new String[alertButtons.Count];
			for (int i = 0; i < alertButtons.Count; i++)
			{
				actButtonsByName[i] = alertButtons[i].GetAttribute("name");
			}
			VerifyString(String.Join(",", actButtonsByName), String.Join(",", expButtonsByName), reqTags);
		}

		public static void VerifyString(string actString, string expString, params string[] reqTags)
		{
			Console.WriteLine("Actual: " + actString);
			Console.WriteLine("Expected: " + expString);
			if (actString.Equals(expString))
			{
				Console.WriteLine("PASS Requirements: " + string.Join(",", reqTags));
			}
			else
			{
				Console.WriteLine("FAIL Requirements: " + string.Join(",", reqTags));
			}
		}

		public static void VerifyInt(int actInt, int expInt, int tolerance = 0, params string[] reqTags)
		{
			Console.WriteLine("Actual: " + actInt);
			Console.WriteLine("Expected: " + actInt);
			Console.WriteLine("Tolerance: " + tolerance);
			if (Math.Abs(actInt - expInt) <= tolerance)
			{
				Console.WriteLine("PASS Requirements: " + string.Join(",", reqTags));
			}
			else
			{
				Console.WriteLine("FAIL Requirements: " + string.Join(",", reqTags));
			}
		}

		public static void VerifyProperty(AppiumDriver<AppiumWebElement> appDriver, By selector, eGUIElementAttribute attribute, string expAttributeVal, params string[] reqTags)
		{
			AppiumWebElement element = SupportLib.FindElement(selector);
			string actAttributeVal = SupportLib.GetAttributeValue(element, attribute);
			VerifyString(actAttributeVal, expAttributeVal, reqTags);
		}
	}
}
