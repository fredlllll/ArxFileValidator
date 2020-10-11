﻿using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
	public struct EERIE_IO_PORTALS
	{
		public EERIE_IO_EERIEPOLY poly;
		public int room_1; // facing normal
		public int room_2;
		public short useportal;
		public short paddy;
	}
}
