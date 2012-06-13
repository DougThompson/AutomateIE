namespace AutomateIE
{
	partial class frmAutomateIE
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.textBox1 = new System.Windows.Forms.RichTextBox();
			this.tctlMain = new System.Windows.Forms.TabControl();
			this.tabScript = new System.Windows.Forms.TabPage();
			this.tecMain = new ICSharpCode.TextEditor.TextEditorControl();
			this.tabOutput = new System.Windows.Forms.TabPage();
			this.btnRunTest = new System.Windows.Forms.Button();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.tctlMain.SuspendLayout();
			this.tabScript.SuspendLayout();
			this.tabOutput.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 655);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(861, 22);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(179, 22);
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuOpen
			// 
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpen.Size = new System.Drawing.Size(179, 22);
			this.mnuOpen.Text = "&Open Script";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSave
			// 
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(179, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.Size = new System.Drawing.Size(179, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(861, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(829, 603);
			this.textBox1.TabIndex = 11;
			this.textBox1.Text = "";
			this.textBox1.WordWrap = false;
			// 
			// tctlMain
			// 
			this.tctlMain.Controls.Add(this.tabScript);
			this.tctlMain.Controls.Add(this.tabOutput);
			this.tctlMain.Location = new System.Drawing.Point(12, 27);
			this.tctlMain.Name = "tctlMain";
			this.tctlMain.SelectedIndex = 0;
			this.tctlMain.Size = new System.Drawing.Size(837, 625);
			this.tctlMain.TabIndex = 12;
			// 
			// tabScript
			// 
			this.tabScript.Controls.Add(this.tecMain);
			this.tabScript.Location = new System.Drawing.Point(4, 22);
			this.tabScript.Name = "tabScript";
			this.tabScript.Padding = new System.Windows.Forms.Padding(3);
			this.tabScript.Size = new System.Drawing.Size(829, 599);
			this.tabScript.TabIndex = 0;
			this.tabScript.Text = "Script";
			this.tabScript.UseVisualStyleBackColor = true;
			// 
			// tecMain
			// 
			this.tecMain.Location = new System.Drawing.Point(6, 6);
			this.tecMain.Name = "tecMain";
			this.tecMain.ShowEOLMarkers = true;
			this.tecMain.ShowSpaces = true;
			this.tecMain.ShowTabs = true;
			this.tecMain.ShowVRuler = true;
			this.tecMain.Size = new System.Drawing.Size(817, 587);
			this.tecMain.TabIndex = 0;
			// 
			// tabOutput
			// 
			this.tabOutput.Controls.Add(this.textBox1);
			this.tabOutput.Location = new System.Drawing.Point(4, 22);
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
			this.tabOutput.Size = new System.Drawing.Size(829, 599);
			this.tabOutput.TabIndex = 1;
			this.tabOutput.Text = "Output";
			this.tabOutput.UseVisualStyleBackColor = true;
			// 
			// btnRunTest
			// 
			this.btnRunTest.Location = new System.Drawing.Point(155, 13);
			this.btnRunTest.Name = "btnRunTest";
			this.btnRunTest.Size = new System.Drawing.Size(75, 23);
			this.btnRunTest.TabIndex = 13;
			this.btnRunTest.Text = "RunTest";
			this.btnRunTest.UseVisualStyleBackColor = true;
			this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
			// 
			// frmWebTestAX
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(861, 677);
			this.Controls.Add(this.btnRunTest);
			this.Controls.Add(this.tctlMain);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "frmWebTestAX";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmWebTestAX";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWebTestAX_FormClosing);
			this.Load += new System.EventHandler(this.frmWebTestAX_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tctlMain.ResumeLayout(false);
			this.tabScript.ResumeLayout(false);
			this.tabOutput.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.RichTextBox textBox1;
		private System.Windows.Forms.TabControl tctlMain;
		private System.Windows.Forms.TabPage tabScript;
		private System.Windows.Forms.TabPage tabOutput;
		private System.Windows.Forms.Button btnRunTest;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private ICSharpCode.TextEditor.TextEditorControl tecMain;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.SaveFileDialog dlgSave;
	}
}