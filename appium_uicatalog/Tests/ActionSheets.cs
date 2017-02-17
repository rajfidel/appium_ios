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
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, "Back")));

			//Select Action Sheets table cell
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, "Action Sheets")));

			//Select Okay / Cancel table cell and select each action sheet value
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, "Okay / Cancel")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, "OK")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, "Okay / Cancel")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, "Cancel")));

			//Select Other table cell and select each action sheet value
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, "Other")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, "Destructive Choice")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, "Other")));
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ActionSheetButton, "Safe Choice")));

			app.CloseApp();
		}
	}
}
