#region Usings

using System.Collections;

#endregion

namespace GeneralTools.Orm
{
	public interface IListFilter
	{
		bool FilterReady { get; set; }
		IEnumerable PerformFilter(IEnumerable inputList);

		// Criteres de filtre de base
		bool OnlyOnline { get; set; }
		// Paging
		bool Paging { get; set; }
		int PageNumber { get; set; }
		int NbItemsPerPage { get; set; }

		string SearchToken { get; set; }
	}
}

/** EXEMPLE D'IMPLÉ DE PERFORMFILTER **/

//public IEnumerable PerformFilter(IEnumerable nonFilteredList)
//        {
//            IEnumerable<VideoEntity> workList = nonFilteredList as IEnumerable<VideoEntity>;
//            // Selection des videos Visibles uniquement
//            workList = from v in workList where v.visible select v;

//            // Selection des videos en fonction de leur etat de moderation : 
//            // -- si AwaitingModeration est true, on ne prend que les videos dont moderation est à 0
//            // -- sinon on ne prend que celles qui sont à 1

//                workList = FiltreModeration
//                               ? (from v in workList where v.moderation != -1 select v)
//                               : (from v in workList where v.moderation == 1 select v);
//            if (UserCriteria != Guid.Empty)
//            {
//                workList = from v in workList where v.id_user == UserCriteria select v;
//            }
//            if (MovieIndexCriteria != 0)
//            {
//                // /!\ Le startIndex pour l'enum des films est 1, ça permet de dire qu'à 0 le critere n'est pas pris en compte.
//                workList = from v in workList where v.casting_video == MovieIndexCriteria select v;
//            }
//            switch (SortType)
//            {
//                case VideoSortType.Newest:
//                    workList = from v in workList
//                                      orderby v.creation_date descending
//                                      select v;
//                    break;
//                case VideoSortType.Views:
//                    workList = from v in workList
//                                      orderby v.views descending
//                                      select v;
//                    break;
//                case VideoSortType.Likes:
//                    workList = from v in workList
//                                      orderby v.likes descending
//                                      select v;
//                    break;
//                default:
//                    workList = from v in workList
//                               orderby v.creation_date descending
//                               select v;
//                    break;
//            }
//            if (OrderbyCastingVideo)
//            {
//                workList = from v in workList
//                           orderby v.casting_video
//                           select v;
//            }

//            if (PageNumber > 0)
//            {
//                StartIndex = (PageNumber - 1) * NbItemsPerPage;
//                workList = workList.Skip(StartIndex).Take(NbItemsPerPage);
//            }


//            return workList;
//        }