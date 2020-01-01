using System;
using System.Collections.Generic;
using Common.Models;

namespace LibraryWebSite.Util
{
    public class KeyValuePairUtil
    {
        public static List<KeyValuePair<string, string>> KeyValuePairFromPagedBase(PagedBase filterParameters)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("searchText", filterParameters.SearchText),
                new KeyValuePair<string, string>("pageNum", filterParameters.PageNum.ToString()),
                new KeyValuePair<string, string>("pageSize", filterParameters.PageSize.ToString()),
                new KeyValuePair<string, string>("orderBy", filterParameters.OrderBy.ToString()),
                new KeyValuePair<string, string>("sortOrder", filterParameters.SortOrder.ToString())
            };

            if (filterParameters.FooterFilters != null)
            {
                foreach (string footerFilter in filterParameters.FooterFilters)
                {
                    kvpList.Add(new KeyValuePair<string, string>("footerFilters", footerFilter));
                }
            }
            return kvpList;
        }
    }
}
