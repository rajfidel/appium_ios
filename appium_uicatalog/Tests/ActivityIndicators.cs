﻿using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public static class ActivityIndicators
	{
		public static void Run()
		{
			IOSDriver<IOSElement> app = SupportLib.SetupApp();

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementFilterByAttributeType.Name, "Back")));

			//Select 'Activity Indicators' table cell
			SupportLib.ScrollAndSelectListItem(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.None, string.Empty)),
			                                   By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Activity Indicators")));

			app.CloseApp();
		}
	}
}
