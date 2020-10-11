using System;
using System.IO;
using System.Windows.Forms;

namespace ArxFileValidator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				txtFile.Text = ofd.FileName;
			}
		}

		private void BtnAnalyze_Click(object sender, EventArgs e)
		{
			var file = new FileInfo(txtFile.Text);
			if (!file.Exists)
			{
				MessageBox.Show("File does not exist");
				return;
			}

			string ext = Path.GetExtension(file.FullName).ToLowerInvariant();
			bool result = false;
			Analyzer analyzer = null;
			switch (ext)
			{
				case ".dlf":
					analyzer = new DLFAnalyzer();
					break;
				case ".llf":
					analyzer = new LLFAnalyzer();
					break;
				case ".fts":
					analyzer = new FTSAnalyzer();
					break;
				case ".ftl":
					analyzer = new FTLAnalyzer();
					break;
			}
			if (analyzer == null)
			{
				return;
			}

			result = analyzer.Analyze(file.FullName);

			if (!result)
			{
				MessageBox.Show("Progress was aborted");
			}
			if (analyzer.ErrCount > 0)
			{
				MessageBox.Show("There have been " + analyzer.ErrCount + " errors");
			}
			else
			{
				MessageBox.Show("No errors were detected in this file");
			}
		}
	}
}
