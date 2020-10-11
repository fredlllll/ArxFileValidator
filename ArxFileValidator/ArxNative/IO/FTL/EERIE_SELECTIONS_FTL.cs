using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTL
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_SELECTIONS_FTL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] name;
        public int nb_selected;
        public int selected;
    }
}
