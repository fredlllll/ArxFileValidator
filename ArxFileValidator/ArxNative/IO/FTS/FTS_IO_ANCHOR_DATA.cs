﻿using ArxFileValidator.ArxNative.IO.Shared_IO;
using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_ANCHOR_DATA
    {
        public SavedVec3 pos;
        public float radius;
        public float height;
        public short nb_linked;
        public short flags;
    }
}
