using System;
using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO
{
    public static class ArxIO
    {
        static ArxIO()
        {
            ArxIONative.Init();
        }

        public static byte[] Unpack(byte[] bytes)
        {
            uint inSize = (uint)bytes.Length;

            var pinnedBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            ArxIONative.UnpackAlloc(pinnedBytes.AddrOfPinnedObject(), inSize, out IntPtr outPtr, out uint outSize);

            pinnedBytes.Free();

            var outBytes = new byte[outSize];
            Marshal.Copy(outPtr, outBytes, 0, (int)outSize);

            ArxIONative.UnpackFree(outPtr);

            return outBytes;
        }

        public static byte[] Pack(byte[] bytes)
        {
            return bytes; //because arxio doesnt support that yet im just gonna save uncompressed
        }
    }

    public static class ArxIONative
    {
        //TODO: hook arxio.dll

        [DllImport("ArxIO.dll", EntryPoint = "ArxIO_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init();
        [DllImport("ArxIO.dll", EntryPoint = "ArxIO_getError", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetError(IntPtr outMessage, int size);
        [DllImport("ArxIO.dll", EntryPoint = "ArxIO_getLogLine", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetLogLine(IntPtr outMessage, int size);
        [DllImport("ArxIO.dll", EntryPoint = "ArxIO_unpack_alloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnpackAlloc(IntPtr in_, uint inSize, out IntPtr out_, out uint outSize);
        [DllImport("ArxIO.dll", EntryPoint = "ArxIO_unpack_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnpackFree(IntPtr buffer);
    }
}
