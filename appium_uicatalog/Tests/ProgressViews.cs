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

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementAttribute.Name, "Back")));

			//Select 'Progress Views' table cell
			SupportLib.ScrollAndSelectListItem(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Progress Views")));

			Console.WriteLine(SupportLib.WaitForAttributeValue(By.XPath(SupportLib.GetXPath(eGUIElementType.ProgressIndicator)), 
			                                                   eGUIElementAttribute.Value, "100%", 30000));


		}
	}
}
