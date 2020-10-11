using ArxFileValidator.ArxNative.IO.Shared_IO;
using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_ROOM_DIST_DATA
    {
        public float distance; // -1 means use truedist
        public SavedVec3 startpos;
        public SavedVec3 endpos;
    }
}
