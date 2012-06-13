using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ICSharpCode.TextEditor.Document;

namespace AutomateIE
{
	public partial class frmAutomateIE : Form
	{
		public IEDriver webBrowser1;
		public string startUrl = "";
		public string webTestFileName = "";

		public frmAutomateIE()
		{
			InitializeComponent();
			Program.closeClicked = false;
			try
			{
				webBrowser1 = new IEDriver(false, null, null, textBox1);
				webBrowser1.Open("about:blank");
				textBox1.AppendText("Starting..." + Environment.NewLine);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw;
			}
		}

		private void frmWebTestAX_FormClosing(object sender, FormClosingEventArgs e)
		{
			Program.closeClicked = true;
		}

		private void frmWebTestAX_Load(object sender, EventArgs e)
		{

		}

		private void btnRunTest_Click(object sender, EventArgs e)
		{
			Program.closeClicked = false;
			try
			{
				tctlMain.SelectedIndex = 1;
				textBox1.Clear();
				textBox1.AppendText("Starting..." + Environment.NewLine);
				RunTest();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Program.closeClicked = true;
			this.Close();
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void OpenFile()
		{
			dlgOpen.Filter = "Xml files (*.xml)|*.xml";
			dlgOpen.Multiselect = false;
			dlgOpen.CheckFileExists = true;
			dlgOpen.CheckPathExists = true;
			dlgOpen.ValidateNames = true;

			try
			{
				if (dlgOpen.ShowDialog() == DialogResult.OK)
				{
					webTestFileName = dlgOpen.FileName;
					tecMain.LoadFile(webTestFileName, true, true);
					tctlMain.SelectedIndex = 0;
				}
			}
			catch { }
		}

		private void mnuSaveAs_Click(object sender, EventArgs e)
		{
			saveAs();
		}

		private void mnuSave_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(webTestFileName))
				tecMain.SaveFile(webTestFileName);
			else
				saveAs();
		}

		private void saveAs()
		{
			if (!String.IsNullOrEmpty(webTestFileName))
			{
				dlgSave.FileName = Path.GetFileName(webTestFileName);
				dlgSave.InitialDirectory = Path.GetFullPath(webTestFileName);
			}

			dlgSave.Filter = "Xml files (*.xml)|*.xml";
			dlgSave.CheckFileExists = true;
			dlgSave.CheckPathExists = true;
			dlgSave.ValidateNames = true;

			if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
			}
		}

		/// <summary>
		/// This is the "meat" of the decision engine.  Take the test script, parse
		/// the XML element and attributes, then execute the proper step.
		/// </summary>
		private void RunTest()
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(tecMain.Text);

			if (doc.SelectNodes("//testplan").Count > 0)
			{
				foreach (XmlNode step in doc.SelectSingleNode("//testplan").ChildNodes)
				{
					if (step.NodeType != XmlNodeType.Element)
						continue;

					string id = step.Attributes["id"] == null ? null : step.Attributes["id"].InnerText;
					string url = step.Attributes["url"] == null ? null : step.Attributes["url"].InnerText;
					string cellContents = step.Attributes["contents"] == null ? null : step.Attributes["contents"].InnerText;
					string value = step.Attributes["value"] == null ? null : step.Attributes["value"].InnerText;
					string tableId = step.Attributes["tableId"] == null ? null : step.Attributes["tableId"].InnerText;
					string pageName = step.Attributes["pageName"] == null ? null : step.Attributes["pageName"].InnerText;
					string pageTitle = step.Attributes["pageTitle"] == null ? null : step.Attributes["pageTitle"].InnerText;

					bool startsWith = bool.Parse(step.Attributes["startsWith"] == null ? "false" : step.Attributes["startsWith"].InnerText);
					bool caseSensitive = bool.Parse(step.Attributes["caseSensitive"] == null ? "false" : step.Attributes["caseSensitive"].InnerText);
					bool append = bool.Parse(step.Attributes["append"] == null ? "false" : step.Attributes["append"].InnerText);

					int index = int.Parse(step.Attributes["index"] == null ? "0" : step.Attributes["index"].InnerText);
					int cellOffset = int.Parse(step.Attributes["offset"] == null ? "0" : step.Attributes["offset"].InnerText);
					int cellPosition = int.Parse(step.Attributes["position"] == null ? "0" : step.Attributes["position"].InnerText);
					int inFrameNumber = int.Parse(step.Attributes["inFrameNumber"] == null ? "-1" : (step.Attributes["inFrameNumber"].InnerText == "" ? "-1" : step.Attributes["inFrameNumber"].InnerText));
					int timeOut = int.Parse(step.Attributes["timeOut"] == null ? "30000" : (step.Attributes["timeOut"].InnerText == "" ? "-1" : step.Attributes["timeOut"].InnerText));

					switch (step.Name.ToLowerInvariant())
					{
						case "sleep":
							System.Threading.Thread.Sleep(timeOut);
							break;
						case "close":
							webBrowser1.Close();
							break;

						case "attach":
							webBrowser1 = new IEDriver(true, pageName, pageTitle, textBox1);
							break;
						case "wait":
							webBrowser1.WaitForPageToLoad(timeOut, null, false);
							break;
						case "waitfor":
							webBrowser1.WaitForPageToLoad(timeOut, pageName, true);
							break;
						case "open":
							webBrowser1.Open(url);
							break;

						case "getpagetitle":
							webBrowser1.GetPageTitle();
							break;
						case "getelementinnertext":
							webBrowser1.GetElementInnerText(inFrameNumber, id);
							break;
						case "getelementvalue":
							webBrowser1.GetElementValue(inFrameNumber, id);
							break;

						case "clickbutton":
							webBrowser1.ClickButton(inFrameNumber, id);
							break;
						case "clicknamedlink":
							webBrowser1.ClickNamedLink(inFrameNumber, id);
							break;
						case "clicktextlink":
							webBrowser1.ClickTextLink(inFrameNumber, value, caseSensitive);
							break;
						case "settextboxtext":
							webBrowser1.SetTextBoxText(inFrameNumber, "INPUT", id, value, append);
							break;
						case "settextareatext":
							webBrowser1.SetTextBoxText(inFrameNumber, "TEXTAREA", id, value, append);
							break;
						case "setselectvalue":
							webBrowser1.SetSelectValue(inFrameNumber, id, value);
							break;
						case "setcheckbox":
							webBrowser1.SetCheckableControl("CHECKBOX", inFrameNumber, index, id, value);
							break;
						case "setradiobutton":
							webBrowser1.SetCheckableControl("RADIOBUTTON", inFrameNumber, index, id, value);
							break;
						case "verifytext":
							webBrowser1.VerifyTextOnPage(inFrameNumber, value, caseSensitive);
							break;
						case "clicklinkintd":
							webBrowser1.ActionInTD(index, inFrameNumber, "LINK", tableId, cellContents, value, startsWith, cellOffset, cellPosition);
							break;
						case "setcheckboxintd":
							webBrowser1.ActionInTD(index, inFrameNumber, "CHECKBOX", tableId, cellContents, value, startsWith, cellOffset, cellPosition);
							break;
						case "setradiobuttonintd":
							webBrowser1.ActionInTD(index, inFrameNumber, "RADIOBUTTON", tableId, cellContents, value, startsWith, cellOffset, cellPosition);
							break;
					}
				}
			}
		}

#region Web Browser Events
		private void Navigate(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				if (!address.Equals("about:blank"))
				{
					if (!address.StartsWith("http://") && !address.StartsWith("https://"))
					{
						address = "http://" + address;
					}
					try
					{
						webBrowser1.Open(address);
					}
					catch (UriFormatException)
					{
					}
				}
			}
		}
#endregion
	}
}
