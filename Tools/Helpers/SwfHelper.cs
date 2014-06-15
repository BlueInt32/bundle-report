using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;

namespace Tools
{
	public class SwfHelper
	{
		public StdResult<NoType> SaveSwfFromPostedFile(HttpPostedFileBase file, string fileFullDesiredPath)
		{
			if (file == null)
				return StdResult<NoType>.BadResult("Image obligatoire.");
			string ext = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower().Substring(1);
			if (ext.ToLower() != "swf")
			{
				return StdResult<NoType>.BadResult("Extension non reconnue (utiliser des fichiers swf)");
			}

			string savePath = string.Empty;
			try
			{
				file.SaveAs(fileFullDesiredPath);
			}
			catch
			{
				return StdResult<NoType>.BadResultFormat("Erreur à l'écriture du fichier {0}", savePath);
			}
			StdResult<NoType> res = StdResult<NoType>.OkResult;
			return res;
		}


		/// <summary>
		/// Summary description for FlashHeaderReader.
		/// </summary>
		public class FlashHeaderReader
		{
			internal int FrameRate
			{
				get { return mFrameRate; }
			}
			internal int FrameCount
			{
				get { return mFrameCount; }
			}

			internal int Width
			{
				get { return mWidth; }
			}
			public Size SwfSize { get { return new Size(Width, Height); } }

				internal int Height
			{
				get { return mHeight; }
			}

			internal bool Error
			{
				get { return mbErrorSignature; }
			}

			private int mFrameRate = 0;
			private int mFrameCount = 0;
			private int mVersion = 0;
			//private bool mIsCompressed = false;
			private int mWidth = 0;
			private int mHeight = 0;
			private bool mbErrorSignature;

			// To Parse Header
			private byte[] data;
			private int currentIndex;

			public FlashHeaderReader(string filename)
			{
				currentIndex = 0;
				mbErrorSignature = false;
				FileStream fs = File.OpenRead(filename);
				data = new byte[fs.Length];
				fs.Read(data, 0, data.Length);
				fs.Close();
				ParseHeader();
			}

			/*
			 *  Parse the header of the swf format
			 *  Format http://www.half-serious.com/swf/format/index.html#header
			 *  convert from php : http://www.zend.com/codex.php?id=1382&single=1   
			 */
			private void ParseHeader()
			{
				ReadSignature();
				mVersion = GetNextByte();
				ReadFileLength();
				ReadFrameSize();
				ReadFrameRate();
				ReadFrameCount();
			}

			/*
			 * Read first caractere FWS or CWS
			 * File compressed are not supported
			 */
			private void ReadSignature()
			{
				Byte readingByte = GetNextByte();
				if (readingByte == 'C')
				{
					//mIsCompressed = true;
					// Not supported
				}
				if (readingByte != 'F' && readingByte != 'C')
				{
					mbErrorSignature = true;
				}

				if (GetNextByte() != 'W') mbErrorSignature = true;
				if (GetNextByte() != 'S') mbErrorSignature = true;
			}
			/*
			 *  Read the RECT Structure : Frame size in twips   
			 *  This retrieve from byte the size.
			 *  the source php is difficult to translate I have made
			 *  something working...
			 */
			private void ReadFrameSize()
			{
				int cByte = GetNextByte();
				int NbBits = cByte >> 3;

				cByte &= 7;
				cByte <<= 5;

				int currentBit = 2;

				int currentValue;

				// Must get all 4 values in the RECT
				for (int numField = 0; numField < 4; numField++)
				{
					currentValue = 0;
					int bitcount = 0;
					while (bitcount < NbBits)
					{
						if ((cByte & 128) == 128)
						{
							currentValue = currentValue + (1 << (NbBits - bitcount - 1));
						}
						cByte <<= 1;
						cByte &= 255;
						currentBit--;
						bitcount++;
						// We will be needing a new byte if we run out of bits
						if (currentBit < 0)
						{
							cByte = GetNextByte();
							currentBit = 7;
						}
					}

					// TWIPS to PIXELS
					currentValue /= 20;
					switch (numField)
					{
						case 0:
							mWidth = currentValue;
							break;
						case 1:
							mWidth = currentValue - mWidth;
							break;
						case 2:
							mHeight = currentValue;
							break;
						case 3:
							mHeight = currentValue - mHeight;
							break;
					}
				}
			}

			/*
			 * Read Frame delay in 8.8 fixed number of frames per second  
			 */
			private void ReadFrameRate()
			{
				// Frame rate
				byte fps_decimal, fps_int;
				fps_decimal = GetNextByte();
				fps_int = GetNextByte();
				mFrameRate = fps_int + (fps_decimal) / 100;
			}

			private void ReadFrameCount()
			{
				for (int i = 0; i < 2; i++)
				{
					mFrameCount += (GetNextByte() << (8 * i));
				}
			}
			/*
			 *  Read  FileLength : Length of entire file in bytes   
			 *  Not implemented
			 */
			private void ReadFileLength()
			{
				//Read Size
				GetNextByte();
				GetNextByte();
				GetNextByte();
				GetNextByte();
			}

			/*
			 *  Retrieve one caractere of the buffer.
			 */
			private byte GetNextByte()
			{
				Byte result;
				result = data[currentIndex];
				currentIndex++;
				return (result);
			}
		}

	}
}
