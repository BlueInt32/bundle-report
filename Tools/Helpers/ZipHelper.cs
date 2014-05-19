using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Tools
{
	public class ZipHelper
	{
		/// <summary>
		/// This function creates a zip
		/// </summary>
		/// <param name="filepaths">List of absolute system filepaths</param>
		/// <param name="zipFileName">Absolute desired systeme final zip filepath</param>
		/// <param name="compressionLevel">Compression level from 0 (no comp.) to 9 (best comp.)</param>
		/// <returns></returns>
		public OperationResult<NoType> CreateZip(List<string> filepaths, string zipFileName, int compressionLevel)
		{
			try
			{
				using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
				{
					s.SetLevel(9); // 0 - store only to 9 - means best compression

					byte[] buffer = new byte[4096];

					foreach (string file in filepaths)
					{

						// Using GetFileName makes the result compatible with XP
						// as the resulting path is not absolute.
						ZipEntry entry = new ZipEntry(Path.GetFileName(file));

						// Setup the entry data as required.

						// Crc and size are handled by the library for seakable streams
						// so no need to do them here.

						// Could also use the last write time or similar for the file.
						entry.DateTime = DateTime.Now;
						s.PutNextEntry(entry);

						using (FileStream fs = File.OpenRead(file))
						{

							// Using a fixed size buffer here makes no noticeable difference for output
							// but keeps a lid on memory usage.
							int sourceBytes;
							do
							{
								sourceBytes = fs.Read(buffer, 0, buffer.Length);
								s.Write(buffer, 0, sourceBytes);
							} while (sourceBytes > 0);
						}
					}

					// Finish/Close arent needed strictly as the using statement does this automatically

					// Finish is important to ensure trailing information for a Zip file is appended.  Without this
					// the created file would be invalid.
					s.Finish();

					// Close is important to wrap things up and unlock the file.
					s.Close();
					return OperationResult<NoType>.OkResult;
				}
			}
			catch (Exception ex)
			{
				return OperationResult<NoType>.BadResult(ex.Message);

				// No need to rethrow the exception as for our purposes its handled.
			}
		}
	}
}
