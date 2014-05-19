#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

#endregion

namespace Tools
{
	public class ImageHelper
	{
		public OperationResult<NoType> CreateImageFileFromB64(string imageName, string b64Image)
		{
			try
			{
				byte[] byteData = Convert.FromBase64String(b64Image);
				return CreateImageFile(imageName, byteData);
			}
			catch (FormatException)
			{
				return OperationResult<NoType>.BadResult("Erreur : données B64 mal formées.");
			}
		}

		private OperationResult<NoType> CreateImageFile(string imageName, byte[] byteArrayImage)
		{
			ImageConverter ic = new ImageConverter();
			Image img;
			try
			{
				img = (Image)ic.ConvertFrom(byteArrayImage);
			}
			catch
			{
				return OperationResult<NoType>.BadResult("Erreur : Conversion B64 -> Image a echoué.");
			}

			string imageServerDirectory = HttpContext.Current.Server.MapPath("~/Content/VideoThumbs");
			string filesavePath = Path.Combine(imageServerDirectory, string.Format("{0}.jpg", imageName));
			try
			{
				using (Bitmap bitmap1 = new Bitmap(img))
				{
					bitmap1.Save(filesavePath);
				}
			}
			catch
			{
				return OperationResult<NoType>.BadResult("Erreur : Sauvegarde du fichier image a échoué.");
			}
			return OperationResult<NoType>.OkResult;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">Fichier posté</param>
		/// <param name="serverFileDirectory">Chemin serveur jusqu'à l'image, finit par "/"</param>
		/// <param name="imageName">Nom de l'image, sans extension</param>
		/// <param name="sizeRequired">Taille requise de l'image</param>
		/// <returns></returns>
		public OperationResult<NoType> SaveImageFromPostedFile(HttpPostedFileBase file, string fileFullDesiredPath, Size? sizeRequired, List<string> extensionsOk)
		{
			if (file == null)
				return OperationResult<NoType>.BadResult("Image obligatoire.");
			OperationResult<NoType> ScanExtensionResult = ScanImageExtension(file, extensionsOk);
			if (!ScanExtensionResult.Result)
			{
				return ScanExtensionResult;
			}
			Bitmap imgFile;
			try
			{
				imgFile = new Bitmap(file.InputStream);
			}
			catch
			{
				return OperationResult<NoType>.BadResult("Format d'image inconnu");
			}
			if (sizeRequired != null && !(imgFile.Size == sizeRequired))
				return OperationResult<NoType>.BadResult(string.Format("La taille de l'image n'est pas correcte ({0}x{1})", sizeRequired.Value.Width, sizeRequired.Value.Height));

			string savePath = string.Empty;
			try
			{
				imgFile.Save(fileFullDesiredPath);
			}
			catch
			{
				return OperationResult<NoType>.BadResult(string.Format("Erreur à l'écriture du fichier {0}", savePath));
			}
			finally
			{
				imgFile.Dispose();
			}
			OperationResult<NoType> res = OperationResult<NoType>.OkResult;
			return res;
		}

		/// <summary>
		/// Validation d'une image envoyée en post
		/// </summary>
		/// <param name="extensionsOk">Extensions acceptées sans le "." devant (sinon ça marche pas)</param>
		/// <returns></returns>
		public OperationResult<NoType> ScanImageExtension(HttpPostedFileBase file, List<string> extensionsOk)
		{
			string ext = file.FileName.Substring(file.FileName.LastIndexOf(".")).Substring(1);
			List<string> okExtLower = extensionsOk.Select(okExt => (okExt.StartsWith(".") ? okExt.Substring(1) : okExt).ToLower()).ToList();
			return okExtLower.Contains(ext)
					? OperationResult<NoType>.OkResult
					: OperationResult<NoType>.BadResultFormat("Extension d'image non reconnue (acceptée(s) : {0}).", string.Join("|", extensionsOk.ToArray()));
		}

		public static Size GetImageDimension(string imageUrl)
		{
			WebClient client = new WebClient();
			using (Stream data = client.OpenRead(imageUrl))
			{
				Bitmap img = new Bitmap(data);
				return img.Size;
			}
		}
		public static OperationResult<NoType> Resize(string source, string destination, int width, int height)
		{
			OperationResult<NoType> res = ImageResizer.GDIHelpers.ResizeImageConstrained(source, destination, width, height);
			return res;
		}

	}
	public class ImageWebInfo
	{
		public string ImageExtension { get; set; }
		public string ImageAlt { get; set; }
	}
}