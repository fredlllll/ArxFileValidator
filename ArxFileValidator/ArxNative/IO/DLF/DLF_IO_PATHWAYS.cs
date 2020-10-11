using ArxFileValidator.ArxNative.IO.Shared_IO;
using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DLF_IO_PATHWAYS
    {
        public SavedVec3 rpos;
        public int flag;
        public uint time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] fpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] lpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] cpadd;
    }
}
