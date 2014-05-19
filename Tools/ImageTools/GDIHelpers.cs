using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Tools;

namespace ImageResizer
{
	public class GDIHelpers
	{

		static public OperationResult<NoType> ResizeImageConstrained(string SourceFilePath, string DestinationFilePath, int width, int size)
		{
			try
			{
				if (File.Exists(SourceFilePath))
				{
					// opening source image, this is the uber-magic non-blocking way !
					FileStream fs = new FileStream(SourceFilePath, FileMode.Open);
					Image SourceImage = Image.FromStream(fs);
					fs.Close();

					// size of the target image
					Size TargetSize = new Size(width, size);


					// we need to adjust the source image to the maximum size of the target size
					if (TargetSize.Width > TargetSize.Height)
					{
						//float x = (float)TargetSize.Width / (float)SourceImage.Width;
						//int z = SourceImage.Height;
						//int d = (int) (x * z);

						Size TempSize = new Size(TargetSize.Width, (int)(((float)TargetSize.Width / (float)SourceImage.Width) * SourceImage.Height));

						if (TargetSize.Height > TempSize.Height)
						{
							TempSize = new Size((int)(((float)TargetSize.Height / (float)SourceImage.Height) * SourceImage.Width), TargetSize.Height);
						}

						SourceImage = ResizeImage(SourceImage, TempSize);
					}
					else
					{
						Size TempSize = new Size((int)(((float)TargetSize.Height / (float)SourceImage.Height) * SourceImage.Width), TargetSize.Height);

						if (TargetSize.Width > TempSize.Width)
						{
							TempSize = new Size(TargetSize.Width, (int)(((float)TargetSize.Width / (float)SourceImage.Width) * SourceImage.Height));
						}

						SourceImage = ResizeImage(SourceImage, TempSize);
					} 

					// now we can crop
					if (SourceImage.Height > TargetSize.Height)
					{
						Rectangle rt = new Rectangle(0, ((SourceImage.Height - TargetSize.Height) / 2), TargetSize.Width, TargetSize.Height);
						SourceImage = CropImage(SourceImage, rt);
					}
					else
					{
						Rectangle rt = new Rectangle(((SourceImage.Width - TargetSize.Width) / 2), 0, TargetSize.Width, TargetSize.Height);
						SourceImage = CropImage(SourceImage, rt);
					}
					ImageFormat format = ImageFormat.Jpeg;
					//EncoderParameters parameters = new EncoderParameters();
					//parameters.Param = new EncoderParameter{ 
					SourceImage.Save(DestinationFilePath, format);
				}
				return OperationResult<NoType>.OkResult;
			}
			catch (Exception e)
			{
				return OperationResult<NoType>.BadResultFormat("Une exception s'est produite :{0}", e.Message);
			}
		}

		public static Image CropImage(Image img, Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(img);
			Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
			return (Image)(bmpCrop);
		}

		public static Image ResizeImage(Image imgToResize, Size size)
		{
			int destWidth = size.Width;
			int destHeight = size.Height;

			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage((Image)b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();

			return (Image)b;
		}

		public static Image ResizeImage(Image imgToResize, float nPercent)
		{
			int sourceWidth = imgToResize.Width;
			int sourceHeight = imgToResize.Height;

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			return ResizeImage(imgToResize, new Size(destWidth, destHeight));
		}
	}
}
