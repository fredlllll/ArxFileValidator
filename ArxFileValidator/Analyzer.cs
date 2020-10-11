using System;
using System.Windows.Forms;

namespace ArxFileValidator
{
	public abstract class Analyzer
	{
		protected int errCount = 0;
		public int ErrCount
		{
			get { return errCount; }
		}

		public abstract bool Analyze(string file);

		/// <summary>
		/// shows a messagebox with the error and asks the user if we should continue, but only if abort is false
		/// </summary>
		/// <param name="message"></param>
		/// <param name="abort"></param>
		/// <returns></returns>
		protected bool Err(string message, bool abort = false)
		{
			MessageBoxButtons btns = MessageBoxButtons.OK;
			if (!abort)
			{
				btns = MessageBoxButtons.YesNo;
				message += Environment.NewLine + Environment.NewLine + "Do you want to continue analyzing this file?";
			}
			errCount++;
			return MessageBox.Show(message, "Error", btns) == DialogResult.Yes;
		}
	}
}
