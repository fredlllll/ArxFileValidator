using ArxFileValidator.ArxNative.IO.LLF;
using ArxFileValidator.ArxNative.IO.Shared_IO;
using System;
using System.Globalization;
using System.IO;

namespace ArxFileValidator
{
	public class LLFAnalyzer : Analyzer
	{
		const float version = 1.44f;
		const string identifier = "DANAE_LLH_FILE";

		bool AnalyzeHeader(LLF_IO_HEADER header)
		{
			if (header.version != version)
			{
				if (!Err("Version is wrong, expected " + version.ToString(CultureInfo.InvariantCulture) + " but got " + header.version.ToString(CultureInfo.InvariantCulture)))
				{
					return false;
				}
			}

			string id = new string(header.identifier).Trim('\0'); //TODO: fix 0 byte problem, we have to check no trailing characters were read
			if (!id.Equals(identifier))
			{
				if (!Err("Identifier is wrong, expected '" + identifier + "' but got '" + id + "'"))
				{
					return false;
				}
			}

			return true;
		}

		bool AnalyzeFromStream(Stream s)
		{
			var reader = new StructReader(s);

			//header
			LLF_IO_HEADER header;
			try
			{
				header = reader.ReadStruct<LLF_IO_HEADER>();
			}
			catch (EndOfStreamException)
			{
				Err(nameof(EndOfStreamException) + " when reading LLF_IO_HEADER, file is too short", true);
				return false;
			}

			if (!AnalyzeHeader(header))
			{
				return false;
			}

			//lights
			DANAE_IO_LIGHT[] lights;
			try
			{
				lights = new DANAE_IO_LIGHT[header.numLights];
			}
			catch (OutOfMemoryException)
			{
				Err(nameof(OutOfMemoryException) + " when creating DANAE_IO_LIGHT[] with size " + header.numLights + ". This can be caused by trying to load an excessive amount of lights", true);
				return false;
			}
			catch (OverflowException)
			{
				Err(nameof(OverflowException) + " when creating DANAE_IO_LIGHT[] with size " + header.numLights + ". This happens when header.numLights is negative", true);
				return false;
			}
			for (int i = 0; i < header.numLights; i++)
			{
				try
				{
					lights[i] = reader.ReadStruct<DANAE_IO_LIGHT>();
				}
				catch (EndOfStreamException)
				{
					Err(nameof(EndOfStreamException) + " when reading light" + (i + 1) + " of " + header.numLights + ", file is too short", true);
					return false;
				}
			}

			//vertex lights
			DANAE_IO_LIGHTINGHEADER lightingHeader;
			try
			{
				lightingHeader = reader.ReadStruct<DANAE_IO_LIGHTINGHEADER>();
			}
			catch (EndOfStreamException)
			{
				Err(nameof(EndOfStreamException) + " when reading DANAE_IO_LIGHTINGHEADER, file is too short", true);
				return false;
			}
			uint[] lightColors;
			try
			{
				lightColors = new uint[lightingHeader.numLights];
			}
			catch (OutOfMemoryException)
			{
				Err(nameof(OutOfMemoryException) + " when creating uint[] lightColors with size " + lightingHeader.numLights + ". This can be caused by trying to load an excessive amount of colors", true);
				return false;
			}
			catch (OverflowException)
			{
				Err(nameof(OverflowException) + " when creating uint[] lightColors with size " + lightingHeader.numLights + ". This happens when lightingHeader.numLights is negative", true);
				return false;
			}
			for (int i = 0; i < lightingHeader.numLights; i++)
			{
				try
				{
					lightColors[i] = reader.ReadUInt32(); //TODO is apparently BGRA if its in compact mode.
				}
				catch (EndOfStreamException)
				{
					Err(nameof(EndOfStreamException) + " when reading light color" + (i + 1) + " of " + lightingHeader.numLights + ", file is too short", true);
					return false;
				}
			}
			return true;
		}

		public override bool Analyze(string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				return AnalyzeFromStream(LLF_IO.EnsureUnpacked(fs));
			}
		}
	}
}
