using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;
using mshtml;
using System.Collections;
using System.Drawing;
using SHDocVw;

namespace AutomateIE
{
	public class IEDriver
	{
		// Setup some global variables and properties
		private string _pageName;
		private bool _isDocumentComplete = false;
		Process _Proc;

		private InternetExplorer _IE;
		private InternetExplorer IE
		{
			get { return _IE; }
		}

		private HTMLDocument Document
		{
			get { return (HTMLDocument)_IE.Document; }
		}
		
		// Placeholder to output progress
		private RichTextBox textBox1;

		~IEDriver()
		{

		}

		/// <summary>
		/// Constructor to attach automation object
		/// </summary>
		/// <param name="attachToExisting"></param>
		/// <param name="pageName"></param>
		/// <param name="pageTitle"></param>
		/// <param name="TextBox1"></param>
		public IEDriver(bool attachToExisting, string pageName, string pageTitle, RichTextBox TextBox1)
		{
			// Setup some local variables
			textBox1 = TextBox1;
			int hwnd = -1;
			ShellWindows m_IEFoundBrowsers = new ShellWindowsClass();
			_IE = null;

			// Start a stopwatch for timeout issues
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			while (_IE == null & stopWatch.Elapsed.TotalSeconds < 60)
			{
				// Determine how to attach -- Existing or new
				// When starting a new test, best not to attach
				if (!attachToExisting)
				{
					if (m_IEFoundBrowsers.Count <= 1)
					{
						_Proc = Process.Start("IExplore.exe");

						Thread.Sleep(1000);
						hwnd = (int)_Proc.MainWindowHandle;
					}

					foreach (InternetExplorer Browser in m_IEFoundBrowsers)
					{
						if (_Proc != null)
						{
							if (Browser.HWND == hwnd)
							{
								_IE = Browser;
								break;
							}
						}
						else
						{
							//Try to attach to an existing browser window
							HTMLDocument doc = Browser.Document as HTMLDocument;
							if (doc != null)
							{
								_IE = Browser;
								break;
							}
						}
					}
				}
				else
				{
					foreach (InternetExplorer Browser in m_IEFoundBrowsers)
					{
						if (_Proc != null)
						{
							if (Browser.HWND == hwnd)
							{
								_IE = Browser;
								break;
							}
						}
						else
						{
							//Try to attach to an existing browser window
							HTMLDocument doc = Browser.Document as HTMLDocument;
							if (doc != null)
							{
								string url = doc.URLUnencoded;
								url = url.Substring(url.LastIndexOf("/") + 1);
								if (url.IndexOf("?") >= 0)
									url = url.Substring(0, url.IndexOf("?"));

								if (doc.title.Equals(pageTitle ?? "", StringComparison.InvariantCultureIgnoreCase))
								{
									printTestResult(textBox1, "Attaching to", String.Format("{0}   {1}{2}", Environment.NewLine, pageTitle, Environment.NewLine));
									_IE = Browser;
									break;
								}

								if (url.Equals(pageName ?? "", StringComparison.InvariantCultureIgnoreCase))
								{
									printTestResult(textBox1, "Attaching to", String.Format("{0}   {1}{2}", Environment.NewLine, pageName, Environment.NewLine));
									_IE = Browser;
									break;
								}
							}
						}
					}
				}
			}

			if (_IE == null & stopWatch.Elapsed.TotalSeconds > 60)
				throw new Exception("IE did not load within 60 seconds.");

			if (_IE == null)
				throw new Exception("IE did not load.");

			IE.Visible = true;
			IE.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(IE_DocumentComplete);
		}

		private void IE_DocumentComplete(object pDisp, ref object URL)
		{
			// Check to see if we are on the correct page (if supplied)
			// or that the Document has a body loaded
			// Set a global flag for document complete
			IHTMLElement body = Document.body;
			if (!String.IsNullOrEmpty(_pageName))
			{
				string url = Document.URLUnencoded;
				url = url.Substring(url.LastIndexOf("/") + 1);
				if (url.IndexOf("?") >= 0)
					url = url.Substring(0, url.IndexOf("?"));

				if (url.Equals(_pageName, StringComparison.InvariantCultureIgnoreCase))
				{
					_isDocumentComplete = true;
				}
			}
			else
				_isDocumentComplete = !String.IsNullOrEmpty(body.innerHTML);
		}

		private void printTestResult(RichTextBox richTextBox, string header, string body)
		{
			// Print out the results to the provided richtext box (colors, formatting, etc...)
			if (richTextBox != null)
				printTestResult(richTextBox, header, Color.Black, body);
		}

		public void printTestResult(RichTextBox richTextBox, string header, Color headerColor, string body)
		{
			// Print out the results to the provided richtext box (colors, formatting, etc...)
			if (richTextBox != null)
			{
				int length = richTextBox.TextLength;
				string msg = header + ":";
				richTextBox.AppendText(msg);
				richTextBox.SelectionStart = length;
				richTextBox.SelectionLength = msg.Length;
				richTextBox.SelectionColor = headerColor;

				length = richTextBox.TextLength;
				msg = body;
				richTextBox.AppendText(msg);
				richTextBox.SelectionStart = length;
				richTextBox.SelectionLength = msg.Length;
				richTextBox.SelectionColor = Color.Black;

				richTextBox.SelectionStart = textBox1.Text.Length;
				richTextBox.ScrollToCaret();
			}
		}

		public void WaitForPageToLoad(int WaitPeriod, string PageName, bool showWaitingMessage)
		{
			// Attempt to wait for a page to load on long loading pages
			// This seems to be a bit flakey and may be due to race-conditions with DocumentComplete?
			// Still working on getting this to be more stable!

			_isDocumentComplete = false;
			_pageName = PageName;
			if (showWaitingMessage)
			{
				if (!String.IsNullOrEmpty(PageName))
				{
					printTestResult(textBox1, "Waiting", String.Format("{0}   {1}: {2}sec{3}", Environment.NewLine, PageName, WaitPeriod/1000, Environment.NewLine));
				}
				else
				{
					printTestResult(textBox1, "Waiting", String.Format("{0}   {1}sec{2}", Environment.NewLine, WaitPeriod/1000, Environment.NewLine));
				}
			}

			// Set a timespan to determine if the document has taken longer than the specified time
			DateTime start;
			TimeSpan ts;
			start = DateTime.Now;

			while (!_isDocumentComplete)
			{
				if (Program.closeClicked == true)
				{
					_isDocumentComplete = true;
					break;
				}

				ts = DateTime.Now.Subtract(start);
				if (ts.TotalMilliseconds > WaitPeriod)
				{
					// Not found, so print out error and stop
					printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Web page took longer than Wait Period to load.{1}", Environment.NewLine, Environment.NewLine));
					_isDocumentComplete = true;

					throw new Exception("Web page took longer than Wait Period to load.");
				}

				System.Threading.Thread.Sleep(250);
				Application.DoEvents();
			}
		}

		public void Close()
		{
			// Close the IE instance
			printTestResult(textBox1, "Closing", String.Format("{0}   {1}", Environment.NewLine, Environment.NewLine));
			object empty = "";
			IE.Quit();
		}

		public void Open(string url)
		{
			// Navigate to a new URL
			printTestResult(textBox1, "Navigating to", String.Format("{0}   {1}{2}", Environment.NewLine, url, Environment.NewLine));
			object empty = "";
			IE.Navigate(url, ref empty, ref empty, ref empty, ref empty);
		}

		/// <summary>
		/// Try to get an element by the ID, index, and/or name
		/// </summary>
		/// <param name="elems"></param>
		/// <param name="index"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private IHTMLElement GetElementById(IHTMLElementCollection elems, int index, string name)
		{
			// If there is no document, then throw exception -- sometimes the driver loses the IE instance
			if (IE.Document == null)
			{
				printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Document is null!{1}", Environment.NewLine, Environment.NewLine));
				throw new Exception("Document is null!");
			}

			IHTMLElement result;
			int i = 0;
			foreach (IHTMLElement htmlElement in elems)
			{
				// if the index == -1, then grab the first one
				// Otherwise, check to see if we've counted enough repeated elements
				if (index < 0 | i == index)
				{
					// Now, get either the id or name, and check if the name (if provided) matches
					string elemID = htmlElement.id ?? (htmlElement.getAttribute("name") == null ? "" : htmlElement.getAttribute("name").ToString());
					if (elemID.EndsWith(name, StringComparison.InvariantCultureIgnoreCase))
					{
						result = htmlElement;
						return result;
					}
				}
				else
					i++;
			}

			// Not found, so print out error and stop
			printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Element not found in current collection.{1}", Environment.NewLine, Environment.NewLine));
			throw new Exception("Element not found in current collection.");
		}

		/// <summary>
		/// Try to get an element by the text of the element
		/// </summary>
		/// <param name="elems"></param>
		/// <param name="text"></param>
		/// <param name="caseSensitive"></param>
		/// <returns></returns>
		private IHTMLElement GetElementByText(IHTMLElementCollection elems, string text, bool caseSensitive)
		{
			// If there is no document, then throw exception -- sometimes the driver loses the IE instance
			if (IE.Document == null)
			{
				printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Document is null!{1}", Environment.NewLine, Environment.NewLine));
				throw new Exception("Document is null!");
			}

			// Start checking each element for the text and use case sensitivity, if requested
			IHTMLElement result;
			foreach (IHTMLElement htmlElement in elems)
			{
				string elemText = htmlElement.innerText;
				if (caseSensitive)
				{
					if (elemText.Equals(text))
					{
						result = htmlElement;
						return result;
					}
				}
				else
				{
					if (elemText.Equals(text, StringComparison.InvariantCultureIgnoreCase))
					{
						result = htmlElement;
						return result;
					}
				}
			}

			// Not found, so print out error and stop
			printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Element not found in current collection.{1}", Environment.NewLine, Environment.NewLine));
			throw new Exception("Element not found in current collection.");
		}
		
		/// <summary>
		/// Check if an element exists in the page
		/// </summary>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public bool DoesElementExist(string elementId)
		{
			IHTMLElement input = GetElementById(Document.all, -1, elementId);
			return input != null;
		}

		/// <summary>
		/// Get the page title
		/// </summary>
		/// <returns></returns>
		public string GetPageTitle()
		{
			string results = ((HTMLDocument)IE.Document).title;
			printTestResult(textBox1, "Get Page Title", String.Format("{0}   {1}{2}", Environment.NewLine, results, Environment.NewLine));
			return results;
		}

		/// <summary>
		/// Get an element's inner text
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public string GetElementInnerText(int frameNumber, string elementId)
		{
			printTestResult(textBox1, "Get Element Text" + FrameMessage(frameNumber), String.Format("{0}   {1}{2}", Environment.NewLine, elementId, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.all, -1, elementId);

			string results = elementByIdEx == null ? "" : elementByIdEx.innerText;
			printTestResult(textBox1, "   Text Results" + FrameMessage(frameNumber), results + Environment.NewLine);

			return results;
		}

		/// <summary>
		/// Get an element's value
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public string GetElementValue(int frameNumber, string elementId)
		{
			printTestResult(textBox1, "Get Element Text" + FrameMessage(frameNumber), String.Format("{0}   {1}{2}", Environment.NewLine, elementId, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.all, -1, elementId);

			string results = elementByIdEx == null ? "" : elementByIdEx.getAttribute("value").ToString();
			printTestResult(textBox1, "   Text Results" + FrameMessage(frameNumber), results + Environment.NewLine);

			return results;
		}

		/// <summary>
		/// Get a page's Frame contents -- used for iFrames, etc...
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <returns></returns>
		private HTMLDocument GetFrameContents(int frameNumber)
		{
			if (frameNumber < 0)
			{
				return Document;
			}
			else
			{
				IHTMLWindow2 windowFrame = (IHTMLWindow2)Document.frames;
				IHTMLFramesCollection2 framescol = windowFrame.frames;

				object index = (object)frameNumber;
				object objFrame = framescol.item(ref index);
				IHTMLWindow2 frame = (IHTMLWindow2)objFrame;
				HTMLDocument frameDoc = (HTMLDocument)frame.document;
				return frameDoc;
			}
		}

		/// <summary>
		/// Update the output with the frame number found
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <returns></returns>
		private static string FrameMessage(int frameNumber)
		{
			return (frameNumber >= 0 ? " in frame #" + frameNumber.ToString() : "");
		}

		/// <summary>
		/// Click an HTML button
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="buttonName"></param>
		public void ClickButton(int frameNumber, string buttonName)
		{
			printTestResult(textBox1, "Click button" + FrameMessage(frameNumber), String.Format("{0}   {1}{2}", Environment.NewLine, buttonName, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.getElementsByTagName("INPUT"), -1, buttonName);
			elementByIdEx.click();
		}

		/// <summary>
		/// Set text in an HTML Textbox
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="type"></param>
		/// <param name="textBoxName"></param>
		/// <param name="value"></param>
		/// <param name="append"></param>
		public void SetTextBoxText(int frameNumber, string type, string textBoxName, string value, bool append)
		{
			printTestResult(textBox1, "Set Text in " + (type == "INPUT" ? "TextBox" : "TextArea") + FrameMessage(frameNumber), String.Format("{0}   {1}: {2}{3}", Environment.NewLine, textBoxName, value, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			HTMLInputElement elementByIdEx = (HTMLInputElement)GetElementById(frameDoc.getElementsByTagName(type), -1, textBoxName);
			if (!append)
				elementByIdEx.value = value;
			else
				elementByIdEx.value += value;
		}

		/// <summary>
		/// Click an HTML a href that has an ID or name
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="linkName"></param>
		public void ClickNamedLink(int frameNumber, string linkName)
		{
			printTestResult(textBox1, "Click Named Link" + FrameMessage(frameNumber), String.Format("{0}   {1}{2}", Environment.NewLine, linkName, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.getElementsByTagName("A"), -1, linkName);
			elementByIdEx.click();
		}

		/// <summary>
		/// Click an HTML a href by checking the link text
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="linkText"></param>
		/// <param name="caseSensitive"></param>
		public void ClickTextLink(int frameNumber, string linkText, bool caseSensitive)
		{
			printTestResult(textBox1, "Click Text Link" + FrameMessage(frameNumber), String.Format("{0}   {1}, {2}{3}", Environment.NewLine, linkText, caseSensitive, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementByText(frameDoc.getElementsByTagName("A"), linkText, caseSensitive);
			elementByIdEx.click();
		}

		/// <summary>
		/// Set a value on an HTML select
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="selectName"></param>
		/// <param name="value"></param>
		public void SetSelectValue(int frameNumber, string selectName, string value)
		{
			printTestResult(textBox1, "Set Dropdown" + FrameMessage(frameNumber), String.Format("{0}   {1}: {2}{3}", Environment.NewLine, selectName, value, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.getElementsByTagName("SELECT"), -1, selectName);
			elementByIdEx.setAttribute("value", value);
		}

		/// <summary>
		/// Set an HTML checkbox or radiobutton
		/// </summary>
		/// <param name="type"></param>
		/// <param name="frameNumber"></param>
		/// <param name="index"></param>
		/// <param name="checkBoxName"></param>
		/// <param name="value"></param>
		public void SetCheckableControl(string type, int frameNumber, int index, string checkBoxName, string value)
		{
			printTestResult(textBox1, "Set " + (type == "CHECKBOX" ? "Checkbox" : "Radiobutton") + FrameMessage(frameNumber), String.Format("{0}   {1}: {2}{3}", Environment.NewLine, checkBoxName, value, Environment.NewLine));
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement elementByIdEx = GetElementById(frameDoc.getElementsByTagName("INPUT"), index, checkBoxName);
			elementByIdEx.setAttribute("checked", value);
		}

		/// <summary>
		/// Verify that certain text is found on the page
		/// </summary>
		/// <param name="frameNumber"></param>
		/// <param name="textToFind"></param>
		/// <param name="caseSensitive"></param>
		public void VerifyTextOnPage(int frameNumber, string textToFind, bool caseSensitive)
		{
			printTestResult(textBox1, "Locate Text on Page" + FrameMessage(frameNumber), String.Format("{0}   {1}, {2}{3}", Environment.NewLine, textToFind, caseSensitive, Environment.NewLine));

			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElement body = frameDoc.body;
			if (body.innerText != null)
			{
				if (!caseSensitive)
				{
					if (!body.innerText.ToLowerInvariant().Contains(textToFind.ToLowerInvariant()))
					{
						// Not found, so print out error and stop
						printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Text not found!{1}", Environment.NewLine, Environment.NewLine));
						throw new Exception("Text not found.");
					}
				}
				else
				{
					if (!body.innerText.Contains(textToFind))
					{
						// Not found, so print out error and stop
						printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Text not found!{1}", Environment.NewLine, Environment.NewLine));
						throw new Exception("Text not found.");
					}
				}
			}
			else
			{
				// Not found, so print out error and stop
				printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   Document is null!{1}", Environment.NewLine, Environment.NewLine));
				throw new Exception("Document is null!");
			}

			printTestResult(textBox1, "   Locate Results" + FrameMessage(frameNumber), " Found!" + Environment.NewLine);
		}

		/// <summary>
		/// In a table, locate a TD by its cell contents, then using an offset, perform an action
		/// </summary>
		/// <param name="index"></param>
		/// <param name="frameNumber"></param>
		/// <param name="type"></param>
		/// <param name="tableId"></param>
		/// <param name="cellContents"></param>
		/// <param name="value"></param>
		/// <param name="startsWith"></param>
		/// <param name="cellOffset"></param>
		/// <param name="cellPosition"></param>
		public void ActionInTD(int index, int frameNumber, string type, string tableId, string cellContents, string value, bool startsWith, int cellOffset, int cellPosition)
		{
			string action = "";
			string tag = "";
			switch (type)
			{
				case "LINK":
					tag = "A";
					action = "Click Link in TD";
					printTestResult(textBox1, action, String.Format("{0}   {1}, {2}, {3}, {4}{5}", Environment.NewLine, cellContents, startsWith, cellOffset, cellPosition, Environment.NewLine));
					break;
				case "CHECKBOX":
					tag = "INPUT";
					action = "Set Checkbox in TD";
					printTestResult(textBox1, action, String.Format("{0}   {1}, {2}, {3}, {4}, {5}{6}", Environment.NewLine, cellContents, value, startsWith, cellOffset, cellPosition, Environment.NewLine));
					break;
				case "RADIOBUTTON":
					tag = "INPUT";
					action = "Set Radiobutton in TD";
					printTestResult(textBox1, action, String.Format("{0}   {1}, {2}, {3}, {4}, {5}{6}", Environment.NewLine, cellContents, value, startsWith, cellOffset, cellPosition, Environment.NewLine));
					break;
			}

			bool flag = false, found = false;
			HTMLDocument frameDoc = GetFrameContents(frameNumber);
			IHTMLElementCollection elementsByTagName = null;
			IEnumerator enumerator = null;

			// If the table ID is provided, then locate the table and grab its contents
			if (!String.IsNullOrEmpty(tableId))
			{
				elementsByTagName = frameDoc.getElementsByTagName("TABLE");
				enumerator = elementsByTagName.GetEnumerator();
				while (enumerator.MoveNext())
				{
					IHTMLElement elem = (IHTMLElement)enumerator.Current;
					string elemID = elem.id ?? (elem.getAttribute("name") == null ? "" : elem.getAttribute("name").ToString());
					if ((elemID ?? "").ToLowerInvariant().EndsWith(tableId.ToLowerInvariant()))
					{
						elementsByTagName = ((IHTMLElement2)elem).getElementsByTagName("TD");
						found = true;
						break;
					}
				}
				if (!found)
				{
					elementsByTagName = null;
					enumerator.Reset();
				}
			}

			//No table ID has been provided, so grab all TDs
			if (elementsByTagName == null)
			{
				elementsByTagName = frameDoc.getElementsByTagName("TD");
			}

			// Loop through all TDs found and check their cell contents
			enumerator = elementsByTagName.GetEnumerator();
			int i = 0;
			int elemCount = 0;
			while (enumerator.MoveNext())
			{
				// If there is a match, then set a flag
				IHTMLElement elem = (IHTMLElement)enumerator.Current;
				if (startsWith)
				{
					flag = (elem.innerText ?? "").StartsWith(cellContents);
				}
				else
				{
					flag = ((elem.innerText ?? "") == cellContents);
				}

				// If this is the proper index (you may have several repeated TDs)
				// then perform the action and break out of the loop
				if (flag)
				{
					if (elemCount == index)
					{
						switch (type)
						{
							case "LINK":
								IHTMLElement link = (IHTMLElement)((IHTMLElement2)elementsByTagName.item(i + cellOffset, i + cellOffset)).getElementsByTagName(tag).item(cellPosition, cellPosition);
								link.click();
								break;
							case "CHECKBOX":
							case "RADIOBUTTON":
								IHTMLElement check = (IHTMLElement)((IHTMLElement2)elementsByTagName.item(i + cellOffset, i + cellOffset)).getElementsByTagName(tag).item(cellPosition, cellPosition);
								check.setAttribute("checked", value);
								break;
						}
						break;
					}
					elemCount++;
				}
				i++;
			}

			if (!found & !flag)
			{
				// Not found, so print out error and stop
				printTestResult(textBox1, "Error", Color.Red, String.Format("{0}   No '{2}' tag with cell contents ({3}) found!{1}", Environment.NewLine, Environment.NewLine, tag, cellContents));
			}
		}
	}
}
