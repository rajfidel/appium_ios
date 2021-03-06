using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using Fidlee.Appium;

namespace appium_uicatalog
{
	public static class PickerView
	{
		public static void Run()
		{
			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementAttribute.Name, "Back")));

			//Select 'Picker View' table cell
			SupportLib.ScrollAndSelectListItem(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Picker View")));

			SupportLib.SetPickerWheelValues("25", "25", "25");

		}
	}
}
