using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;

namespace appium_uicatalog
{
	public static class AlertViews
	{
		public static void Run()
		{
			IOSDriver<IOSElement> app = SupportLib.SetupApp();

			string expTitle = "A Short Title Is Best\nA message should be a short, complete sentence.";

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(app, By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementFilterByAttributeType.Name, "Back")));

			//Select 'Action Sheets' table cell
			SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Alert Views")));

			{
				//Select 'Simple' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Simple")));

				IAlert simpleAlert = app.SwitchTo().Alert();
				VerifyLib.VerifyAlertButtons(app, simpleAlert, new String[] { "OK" }, "Req1");	//Req1 is the imaginary requirement tag that is verified.

				string actTitle = simpleAlert.Text;
				VerifyLib.VerifyString(actTitle, expTitle, "Req1");     

				//Click OK on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "OK")));
			}

			{
				//Select 'Okay / Simple' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label,  "Okay / Cancel")));

				IAlert okayCancelAlert = app.SwitchTo().Alert();
				VerifyLib.VerifyAlertButtons(app, okayCancelAlert, new String[] { "Cancel", "OK" }, "Req2");  

				string actTitle = okayCancelAlert.Text;
				VerifyLib.VerifyString(actTitle, expTitle, "Req2");     

				//Click OK on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "OK")));

				//Select 'Okay / Simple' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Okay / Cancel")));

				//Click Cancel on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Cancel")));
			}

			{
				//Select 'Other' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Other")));

				IAlert otherAlert = app.SwitchTo().Alert();
				VerifyLib.VerifyAlertButtons(app, otherAlert, new String[] { "Choice One", "Choice Two", "Cancel" }, "Req3");

				string actTitle = otherAlert.Text;
				VerifyLib.VerifyString(actTitle, expTitle, "Req3");

				//Click Choice One on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Choice One")));

				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Other")));

				//Click Choice Two on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Choice Two")));

				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Other")));

				//Click Cancel on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Cancel")));
			}

			{
				//Select 'Text Entry' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Text Entry")));

				IAlert otherAlert = app.SwitchTo().Alert();
				VerifyLib.VerifyAlertButtons(app, otherAlert, new String[] { "OK", "Cancel" }, "Req4");

				string actTitle = otherAlert.Text;
				VerifyLib.VerifyString(actTitle, expTitle, "Req4");

				//Send keys to text field
				SupportLib.SendKeys(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertTextField, eGUIElementFilterByAttributeType.None, String.Empty)), "Hi");

				//Click OK on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "OK")));

				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Text Entry")));

				//Click Cancel on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Cancel")));
			}

			{
				//Select 'Secure Text Entry' table cell
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Secure Text Entry")));

				IAlert otherAlert = app.SwitchTo().Alert();
				VerifyLib.VerifyAlertButtons(app, otherAlert, new String[] { "OK", "Cancel" }, "Req5");

				string actTitle = otherAlert.Text;
				VerifyLib.VerifyString(actTitle, expTitle, "Req5");

				//Send keys to text field
				SupportLib.SendKeys(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertSecureTextField, eGUIElementFilterByAttributeType.None, String.Empty)), "password");

				//Click OK on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "OK")));

				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementFilterByAttributeType.Label, "Secure Text Entry")));

				//Click Cancel on alert
				SupportLib.Click(app, By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementFilterByAttributeType.Name, "Cancel")));
			}

			app.CloseApp();
		}
	}
}
