using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
using Fidlee.Appium;

namespace appium_uicatalog
{
	public static class AlertViews
	{
		public static void Run()
		{

			string expTitle = "A Short Title Is Best\nA message should be a short, complete sentence.";

			//Navigate back to Main menu
			SupportLib.ClickUntilElementNotAvailable(By.XPath(SupportLib.GetXPath(eGUIElementType.NavBarButton, eGUIElementAttribute.Name, "Back")));

			//Select 'Alert Views' table cell
			SupportLib.ScrollAndSelectListItem(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Alerts Views")));


			#region 'Simple' Alert 

			{
				//Select 'Simple' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Simple")));

				VerifyLib.VerifyAlertButtons(new String[] { "OK" }, "Req1");    //Req1 is the imaginary requirement tag that is verified.

				AppiumWebElement alertText = SupportLib.FindElement(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertText)));
				string actTitle = SupportLib.GetAttributeValue(alertText, eGUIElementAttribute.Label);
				VerifyLib.VerifyString(actTitle, expTitle, "Req1");     

				//Click OK on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "OK")));
			}

			#endregion

			#region 'Okay / Cancel' Alert

			{
				//Select 'Okay / Cancel' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label,  "Okay / Cancel")));

				VerifyLib.VerifyAlertButtons(new String[] { "Cancel", "OK" }, "Req2");  
				AppiumWebElement alertText = SupportLib.FindElement(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertText)));
				string actTitle = SupportLib.GetAttributeValue(alertText, eGUIElementAttribute.Label);
				VerifyLib.VerifyString(actTitle, expTitle, "Req2");     

				//Click OK on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "OK")));

				//Select 'Okay / Simple' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Okay / Cancel")));

				//Click Cancel on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Cancel")));
			}

			#endregion

			#region 'Other' Alert

			{
				//Select 'Other' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Other")));

				VerifyLib.VerifyAlertButtons(new String[] { "Choice One", "Choice Two", "Cancel" }, "Req3");

				AppiumWebElement alertText = SupportLib.FindElement(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertText)));
				string actTitle = SupportLib.GetAttributeValue(alertText, eGUIElementAttribute.Label);
				VerifyLib.VerifyString(actTitle, expTitle, "Req3");

				//Click Choice One on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Choice One")));

				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Other")));

				//Click Choice Two on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Choice Two")));

				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Other")));

				//Click Cancel on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Cancel")));
			}

			#endregion

			#region 'Text Entry' Alert

			{
				//Select 'Text Entry' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Text Entry")));

				VerifyLib.VerifyAlertButtons(new String[] { "OK", "Cancel" }, "Req4");

				AppiumWebElement alertText = SupportLib.FindElement(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertText)));
				string actTitle = SupportLib.GetAttributeValue(alertText, eGUIElementAttribute.Label);
				VerifyLib.VerifyString(actTitle, expTitle, "Req4");

				//Send keys to text field
				SupportLib.SendKeys(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertTextField, eGUIElementAttribute.None, String.Empty)), "Hi");

				//Click OK on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "OK")));

				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Text Entry")));

				//Click Cancel on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Cancel")));
			}

			#endregion

			#region 'Secure Text Entry' Alert

			{
				//Select 'Secure Text Entry' table cell
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Secure Text Entry")));

				VerifyLib.VerifyAlertButtons(new String[] { "OK", "Cancel" }, "Req5");

				AppiumWebElement alertText = SupportLib.FindElement(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertText)));
				string actTitle = SupportLib.GetAttributeValue(alertText, eGUIElementAttribute.Label);
				VerifyLib.VerifyString(actTitle, expTitle, "Req5");

				//Send keys to text field
				SupportLib.SendKeys(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertSecureTextField, eGUIElementAttribute.None, String.Empty)), "password");

				//Click OK on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "OK")));

				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.TableCell, eGUIElementAttribute.Label, "Secure Text Entry")));

				//Click Cancel on alert
				SupportLib.Click(By.XPath(SupportLib.GetXPath(eGUIElementType.AlertButton, eGUIElementAttribute.Name, "Cancel")));
			}

			#endregion
		}
	}
}
