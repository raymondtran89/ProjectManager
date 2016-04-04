using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.DataAccessLayer.Repository.Helper
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(int pageIndex, int pageSize, int totalCount, IQueryable<T> source)
        {
            AddRange(source);
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int) Math.Ceiling(totalCount/(double) pageSize);
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageCount { get; set; }

        public bool HasPreviosPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPageCount; }
        }
    }
}