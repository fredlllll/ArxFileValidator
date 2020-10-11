using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_VERTEX
    {
        public float posY;
        public float posX;
        public float posZ;
        public float texU;
        public float texV;
    }
}
