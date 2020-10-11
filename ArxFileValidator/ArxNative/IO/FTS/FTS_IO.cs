using System.IO;
using System.Windows.Forms;

namespace ArxFileValidator.ArxNative.IO.FTS
{
    public class FTS_IO
    {
        public FTS_IO_UNIQUE_HEADER header;
        public FTS_IO_UNIQUE_HEADER2[] uniqueHeaders;

        public FTS_IO_SCENE_HEADER sceneHeader;

        public FTS_IO_TEXTURE_CONTAINER[] textureContainers;
        public FTS_IO_CELL[] cells;
        public FTS_IO_ANCHOR[] anchors;
        public EERIE_IO_PORTALS[] portals;
        public FTS_IO_ROOM[] rooms;
        public FTS_IO_ROOM_DIST_DATA[] roomDistances;

        public void LoadFrom(Stream s)
        {
            StructReader reader = new StructReader(s);

            header = reader.ReadStruct<FTS_IO_UNIQUE_HEADER>();

            uniqueHeaders = new FTS_IO_UNIQUE_HEADER2[header.count];
            for (int i = 0; i < header.count; i++)
            {
                uniqueHeaders[i] = reader.ReadStruct<FTS_IO_UNIQUE_HEADER2>();
            }

            sceneHeader = reader.ReadStruct<FTS_IO_SCENE_HEADER>();

            textureContainers = new FTS_IO_TEXTURE_CONTAINER[sceneHeader.nb_textures];
            for (int i = 0; i < sceneHeader.nb_textures; i++)
            {
                textureContainers[i] = reader.ReadStruct<FTS_IO_TEXTURE_CONTAINER>();
            }

            cells = new FTS_IO_CELL[sceneHeader.sizez * sceneHeader.sizex];
            for (int z = 0, index = 0; z < sceneHeader.sizez; z++)
            {
                for (int x = 0; x < sceneHeader.sizex; x++, index++)
                {
                    var cell = new FTS_IO_CELL();
                    cell.ReadFrom(reader);
                    cells[index] = cell;
                }
            }

            anchors = new FTS_IO_ANCHOR[sceneHeader.nb_anchors];
            for (int i = 0; i < sceneHeader.nb_anchors; i++)
            {
                var anchor = new FTS_IO_ANCHOR();
                anchor.ReadFrom(reader);
                anchors[i] = anchor;
            }

            portals = new EERIE_IO_PORTALS[sceneHeader.nb_portals];
            for (int i = 0; i < sceneHeader.nb_portals; i++)
            {
                portals[i] = reader.ReadStruct<EERIE_IO_PORTALS>();
            }

            rooms = new FTS_IO_ROOM[sceneHeader.nb_rooms + 1];
            for (int i = 0; i < sceneHeader.nb_rooms + 1; i++) //no idea why +1, but its in the code
            {
                var room = new FTS_IO_ROOM();
                room.ReadFrom(reader);
                rooms[i] = room;
            }

            roomDistances = new FTS_IO_ROOM_DIST_DATA[rooms.Length * rooms.Length];
            for (int i = 0, index = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < rooms.Length; j++, index++)
                {
                    roomDistances[index] = reader.ReadStruct<FTS_IO_ROOM_DIST_DATA>();
                }
            }

            long remaining = reader.BaseStream.Length - reader.BaseStream.Position;
            if (remaining > 0)
            {
				MessageBox.Show("There are remaining bytes in the fts :" + remaining);
            }
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            var reader = new StructReader(s, System.Text.Encoding.ASCII, true);

            MemoryStream ms = new MemoryStream();
            StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);

            var header = reader.ReadStruct<FTS_IO_UNIQUE_HEADER>();
            writer.WriteStruct(header);

            for (int i = 0; i < header.count; i++)
            {
                writer.WriteStruct(reader.ReadStruct<FTS_IO_UNIQUE_HEADER2>());
            }

            byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            byte[] unpacked = ArxIO.Unpack(restOfFile);

            writer.Write(unpacked); //write unpacked rest
            s.Dispose(); //close old stream
            ms.Position = 0;
            return ms;
        }

     
    }
}
