using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.MultiTouch;

namespace appium_uicatalog
{
	public static class Sliders
	{
		public static void Run()
		{

			IOSDriver<IOSElement> app = SupportLib.SetupApp();

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementFilterByAttributeType.Name, "Back")));

			//Select 'Sliders' table cell
			SupportLib.ScrollAndSelectListItem(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.None, string.Empty)),
											   By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Sliders")));

			ReadOnlyCollection<IOSElement> displayedSliders;
			if (SupportLib.IsElementDisplayed(app, By.XPath(SupportLib.GetXPath(eGUIElementType.Slider, eGUIElementFilterByAttributeType.None, string.Empty)), 
			                                  out displayedSliders))
			{
				for (int i = 0; i < displayedSliders.Count; i++)
				{
					//TODO Set Slider values
				}

			}

			app.CloseApp();

		}
	}
}
