using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ArxFileValidator.ArxNative.IO.FTL
{
	public class FTL_IO
    {
        public FTL_IO_PRIMARY_HEADER header;
        public FTL_IO_SECONDARY_HEADER secondaryHeader;

        public bool has3DDataSection = false;
        public FTL_IO_3D_DATA_SECTION _3DDataSection;
        //apparently all the other sections are unused and undocumented, saving other contents of file into arrays, this will break values in secondary header if modified (for now)
        byte[] dataTill3Ddata, dataTillFileEnd;

        public void ReadFrom(Stream s)
        {
            StructReader reader = new StructReader(s, Encoding.ASCII, true);

            header = reader.ReadStruct<FTL_IO_PRIMARY_HEADER>();

            secondaryHeader = reader.ReadStruct<FTL_IO_SECONDARY_HEADER>();

            long till3Ddata = secondaryHeader.offset_3Ddata - s.Position;
            if (till3Ddata > 0)
            {
                dataTill3Ddata = reader.ReadBytes((int)till3Ddata);
            }
            else
            {
                dataTill3Ddata = null;
            }


            if (secondaryHeader.offset_3Ddata >= 0)
            {
                s.Position = secondaryHeader.offset_3Ddata;

                _3DDataSection = new FTL_IO_3D_DATA_SECTION();
                _3DDataSection.ReadFrom(reader);
                has3DDataSection = true;
            }
            else
            {
				MessageBox.Show("invalid 3d offset: " + secondaryHeader.offset_3Ddata);
            }

            long tillFileEnd = s.Length - s.Position;
            if (tillFileEnd > 0)
            {
                dataTillFileEnd = reader.ReadBytes((int)tillFileEnd);
            }
            else
            {
                dataTillFileEnd = null;
            }
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            var start = s.Position;

            byte[] first3 = new byte[3];
            s.Read(first3, 0, first3.Length);
            s.Position = start;
            if (first3[0] == 'F' && first3[1] == 'T' && first3[2] == 'L')
            {
                //uncomressed
                return s;
            }

            byte[] packed = new byte[s.Length];
            s.Read(packed, 0, packed.Length);
            byte[] unpacked = ArxIO.Unpack(packed);

            MemoryStream ms = new MemoryStream();
            ms.Write(unpacked, 0, unpacked.Length);
            ms.Position = 0;
            s.Dispose(); //close old stream
            return ms;
        }

        public static Stream EnsurePacked(Stream s)
        {
            byte[] unpacked = new byte[s.Length];
            s.Read(unpacked, 0, unpacked.Length);
            byte[] packed = ArxIO.Pack(unpacked);

            MemoryStream ms = new MemoryStream();
            ms.Write(packed, 0, packed.Length);
            ms.Position = 0;
            s.Dispose(); //close old stream
            return ms;
        }
    }
}
