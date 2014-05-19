﻿using Collecte.DAL;
using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Tools;

namespace Collecte.Logic
{
	public class BundleLogic
	{
		public static DateTime GetYesterday()
		{
			DateTime now = DateTime.Now.AddDays(-1);
			return new DateTime(now.Year, now.Month, now.Day);
		}

		public OperationResult<Bundle> CreateBundle(DateTime date)
		{
			BundleDal dal = new BundleDal();
			OperationResult<Bundle> result = dal.GetBundleByDate(date);
			Bundle b;
			if (result.Result && result.ReturnObject != null)
				 b = result.ReturnObject;
			else
			{
				b = new Bundle();
				b.Date = date;
				b.Status = BundleStatus.NoFileCreated;
			}
			result = dal.Create(b);

			return result;
		}

		public OperationResult<Bundle> SetBundleStatus(DateTime date, BundleStatus status)
		{
			BundleDal dal = new BundleDal();
			OperationResult<Bundle> result = dal.GetBundleByDate(date);
			if (result.ReturnObject == null)
				throw new CollecteException("Bundle introuvable.");

			result.ReturnObject.Status = status;

			result = dal.Update(result.ReturnObject);

			return result;
		}

		public OperationResult<Bundle> GetBundleByDate(DateTime date)
		{
			BundleDal dal = new BundleDal();
			OperationResult<Bundle> result = dal.GetBundleByDate(date);
			return result;
		}

		/// <summary>
		/// Permet de définir pour un bundle combien de participations au jeu il référence au total.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="totalSubs"></param>
		/// <returns></returns>
		public OperationResult<Bundle> SetBundleTotalSubs(DateTime date, int totalSubs)
		{
			BundleDal dal = new BundleDal();
			OperationResult<Bundle> result = dal.GetBundleByDate(date);
			if (result.ReturnObject == null)
				throw new CollecteException("Bundle introuvable.");

			result.ReturnObject.NbInscriptions = totalSubs;

			result = dal.Update(result.ReturnObject);

			return result;
		}

		/// <summary>
		/// Permet de spécifier combien de profils sont retournés par Canal.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="nb"></param>
		/// <returns></returns>
		public OperationResult<Bundle> SetBundleNbReturnsCanal(DateTime date, int nbRetoursCanal, int nbOk, int nbKo)
		{
			BundleDal dal = new BundleDal();
			OperationResult<Bundle> result = dal.GetBundleByDate(date);
			if (result.ReturnObject == null)
				throw new CollecteException("Bundle introuvable.");

			result.ReturnObject.NbRetoursCanal = nbRetoursCanal;
			result.ReturnObject.NbOk = nbOk;
			result.ReturnObject.NbKo = nbKo;

			result = dal.Update(result.ReturnObject);

			return result;
		}

		/// <summary>
		/// Permet d'attacher un fichier à un bundle
		/// </summary>
		/// <param name="date"></param>
		/// <param name="filePath"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public OperationResult<BundleFile> AttachFileToBundle(DateTime date, string filePath, BundleFileType type)
		{
			if(!File.Exists(filePath))
				throw new CollecteException("Le fichier indiqué n'existe pas.");

			switch (type)
			{
				case BundleFileType.CsvIn:
				case BundleFileType.CsvOut:
				if (new FileInfo(filePath).Extension != ".csv")
					throw new CollecteException("Le fichier n'a pas la bonne extension.");
				break;
				case BundleFileType.XmlTrade:
				if (new FileInfo(filePath).Extension != ".xml")
					throw new CollecteException("Le fichier n'a pas la bonne extension.");
				break;
			}

			BundleFile bFile = new BundleFile
			{
				//Bundle = resultBundle.ReturnObject,
				CreationDate = DateTime.Now,
				FileName = (new FileInfo(filePath)).Name,
				Type = type
			};


			BundleFileDal bfDal = new BundleFileDal();
			OperationResult<BundleFile> resultFileBundle = bfDal.Create(bFile, date);
			//dal.Update(resultBundle.ReturnObject);
			return resultFileBundle;
		}

		/// <summary>
		/// Retourne la liste de tous les bundles.
		/// </summary>
		/// <returns></returns>
		public List<Bundle> ListBundles()
		{
			BundleDal dal = new BundleDal();

			OperationResult<List<Bundle>> listResult = dal.ListBundles();

			return listResult.ReturnObject;

		}

		public Dictionary<int, List<Bundle>> GroupBundles()
		{
			List<Bundle> list = ListBundles();

			Dictionary<int, List<Bundle>> dico = new Dictionary<int, List<Bundle>>();

			var groups = list.GroupBy(b => GetIso8601WeekOfYear(b.Date));

			foreach (IGrouping<int, Bundle> item in groups)
			{
				dico.Add(item.Key, item.ToList());
			}
			return dico;
		}

		public OperationResult<Bundle> DeleteBundles()
		{
			BundleDal dal = new BundleDal();
			return dal.DeleteBundles();

		}



		public static int GetIso8601WeekOfYear(DateTime time)
		{
			DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			{
				time = time.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}

	}
}