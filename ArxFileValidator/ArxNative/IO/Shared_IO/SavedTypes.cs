using System.Drawing;
using System.Runtime.InteropServices;

namespace ArxFileValidator.ArxNative.IO.Shared_IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SavedColor
    {
        public float r;
        public float g;
        public float b;

        public SavedColor(Color col)
        {
            r = col.R;
            g = col.G;
            b = col.B;
        }

        public Color ToColor()
        {
            return Color.FromArgb((int)(r*255), (int)(g*255), (int)(b*255));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedVec3
    {
        public float x;
        public float y;
        public float z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedAnglef
    {
        public float a;
        public float b;
        public float g;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedTextureVertex
    {
        public SavedVec3 pos;
        public float rhw;
        public uint color;
        public uint specular;
        public float tu;
        public float tv;
    }
}
