using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.Shared_IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DANAE_IO_LIGHTINGHEADER
    {
        public int numLights;
        public int viewMode; // unused
        public int modeLight; // unused
        public int ipad;
    }
}
