using System;
using System.Drawing;
using System.Text;

namespace ArxFileValidator.ArxNative.IO
{
	public static class ArxIOHelper
	{
		public static string GetString(byte[] bytes)
		{
			return Encoding.ASCII.GetString(ArrayHelper.TrimEnd<byte>(bytes, 0));
		}

		public static Color FromBGRA(uint bgra)
		{
			byte[] bytes = BitConverter.GetBytes(bgra);
			return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
		}

		public static Color FromRGB(uint rgba)
		{
			byte[] bytes = BitConverter.GetBytes(rgba);
			return Color.FromArgb(bytes[3], bytes[0], bytes[1], bytes[2]);
		}
	}
}
