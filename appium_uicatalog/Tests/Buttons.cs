using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public static class Buttons
	{

		public static void Run()
		{

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementAttribute.Name, "Back")));

			//Select 'Buttons' table cell
			SupportLib.ScrollAndSelectListItem(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Buttons")));

		}

	}
}
