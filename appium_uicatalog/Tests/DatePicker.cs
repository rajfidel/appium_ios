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
	public static class DatePicker
	{
		public static void Run()
		{

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementAttribute.Name, "Back")));

			//Select 'Date Picker' table cell
			SupportLib.ScrollAndSelectListItem(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Date Picker")));

			DateTime currentDate = DateTime.Now;
			DateTime dateTimeToSet = currentDate.AddDays(2).AddHours(2).AddMinutes(2);
			SupportLib.SelectDateTimeOnDatePicker(dateTimeToSet);

			//You can verify that the XCUIElementTypeStaticText below the Date Picker displays the correct text.
			//Not writing code for verification as filtering by XCUIElementTypeStaticText is not a good way to filter static text element on this page. 
			//(There are other static text elements for instance the 'Date Picker' static text on the navigation bar.)
			//It is therefore recommended that the developer adds accessibility identifier to this XCUIElementTypeStaticText.

		}
	}
}
