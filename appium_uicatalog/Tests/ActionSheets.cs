using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public static class ActionSheets
	{
		public static void Run()
		{
			IOSDriver<IOSElement> app = SupportLib.SetupApp();

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementFilterByAttributeType.Name, "Back")));

			//Select Action Sheets table cell
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Action Sheets")));

			//Select Okay / Cancel table cell and select each action sheet value
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Okay / Cancel")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, eGUIElementFilterByAttributeType.Name, "OK")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Okay / Cancel")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, eGUIElementFilterByAttributeType.Name, "Cancel")));

			//Select Other table cell and select each action sheet value
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Other")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, eGUIElementFilterByAttributeType.Name, "Destructive Choice")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Other")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, eGUIElementFilterByAttributeType.Name, "Safe Choice")));

			app.CloseApp();
		}
	}
}
