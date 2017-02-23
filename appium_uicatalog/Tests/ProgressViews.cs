using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public static class ProgressViews
	{
		public static void Run()
		{
			IOSDriver<IOSElement> app = SupportLib.SetupApp();

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementFilterByAttributeType.Name, "Back")));

			//Select 'Progress Views' table cell
			SupportLib.ScrollAndSelectListItem(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.None, string.Empty)),
											   By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Progress Views")));

			Console.WriteLine(SupportLib.WaitForAttributeValue(app, By.XPath(SupportLib.GetXPath(eGUIElementType.ProgressIndicator, eGUIElementFilterByAttributeType.None, string.Empty)),
															   "value", "100%", TimeSpan.FromSeconds(30)));

			app.CloseApp();

		}
	}
}
