using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FTS_IO_UNIQUE_HEADER2
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] path;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] check;
    }
}
