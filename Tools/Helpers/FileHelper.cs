#region Usings

using System;
using System.IO;
using System.Text;

#endregion

namespace Tools
{
	public class FileHelper
	{
		static Encoding Utf8Encoding { get { return Encoding.UTF8; } }
		public static void SaveFileToDisk(string completePath, string content)
		{
				using (StreamWriter sr = new StreamWriter(completePath))
				{
					sr.Write(content);
					sr.Flush();
				}
		}
		public static void TextAppendToFile(string operationDescription, string pathAndName, string content)
		{
			using (StreamWriter sr = new StreamWriter(pathAndName, true, Encoding.UTF8))
			{
				try
				{
					sr.WriteLine(content);
					sr.Flush();
				}
				catch (Exception e)
				{
					throw new Exception(
						string.Format("Erreur d'ecriture du fichier '{0}' : {1}", pathAndName, e.Message));
				}
				finally
				{
					sr.Close();
				}
			}
		}

		public static string ReadFile(string path)
		{
			using (StreamReader streamReader = new StreamReader(path))
			{
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				return text;
			}

		}

		public string ReadFileFromDisk(string pathAndName)
		{
			using (StreamReader sr = new StreamReader(pathAndName, Encoding.UTF8))
			{
				StringBuilder sb = new StringBuilder(string.Empty);
				while (sr.Peek() >= 0)
				{
					sb.AppendLine(sr.ReadLine());
				}
				return sb.ToString();
			}
		}

		public static StdResult<NoType> CopyFile(string from, string to, bool deleteSource = false)
		{
			try
			{
				File.Copy(from, to, true);
				if (deleteSource)
					File.Delete(from);
			}
			catch (Exception e)
			{
				return StdResult<NoType>.BadResult(e.Message);
			}
			return StdResult<NoType>.OkResult;

		}
		public static StdResult<NoType> MoveFile(string from, string to, bool overwriteIfExists = false)
		{
			try
			{
				if (overwriteIfExists && File.Exists(to))
					File.Delete(to);
				File.Move(from, to);
			}
			catch (Exception e)
			{
				return StdResult<NoType>.BadResult(e.Message);
			}
			return StdResult<NoType>.OkResult;
		}




		public static StdResult<NoType> DeleteFile(string filePath)
		{
			if (!File.Exists(filePath))
				return StdResult<NoType>.BadResult("Fichiez inexistant : " + filePath);
			File.Delete(filePath);
			

			return StdResult<NoType>.OkResult;

		}
		public static StdResult<NoType> Exists(string filePath)
		{
			return File.Exists(filePath) ? StdResult<NoType>.OkResult : StdResult<NoType>.BadResult("fichier inexistant");
		}

		public static StdResult<NoType> SaveFile(string filePath, string content)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(filePath))
				{
					sw.Write(content);
				}
				return StdResult<NoType>.OkResult;
			}
			catch (Exception ex)
			{
				return StdResult<NoType>.BadResult(ex.Message);
			}
		}


		/// <summary>
		/// Récupère par exemple "file.txt" à partir d'un chemin serveur D:/Projets/Foo/bar/file.txt
		/// </summary>
		/// <param name="serverPath"></param>
		/// <param name="suffixe">ajoute eventuellement une chaine de caractère avant le point de l'extension</param>
		/// <returns></returns>
		public static string GetFileNameWithExtension(string serverPath, string prefixe)
		{
			FileInfo fi = new FileInfo(serverPath);
			return string.Format("{0}{1}", prefixe, fi.Name);

		}


		public void CopyDirectory(string sourceDir, string destDir)
		{
			string[] Files;

			if (destDir[destDir.Length - 1] != Path.DirectorySeparatorChar)
				destDir += Path.DirectorySeparatorChar;
			if (!Directory.Exists(destDir))
				Directory.CreateDirectory(destDir);
			Files = Directory.GetFileSystemEntries(sourceDir);
			foreach (string Element in Files)
			{
				// Sub directories

				if (Directory.Exists(Element))
					CopyDirectory(Element, destDir + Path.GetFileName(Element));
				// Files in directory

				else
					File.Copy(Element, destDir + Path.GetFileName(Element), true);
			}
		}
	}
}