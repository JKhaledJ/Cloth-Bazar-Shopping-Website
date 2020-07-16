using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.web.ViewModels
{
    public class BaseListingViewModel
    {
    }

    public class Pager
    {
        public Pager(int totaItems, int? page, int pageSize = 10)
        {
            if (pageSize == 0) pageSize = 10;

            var totalPages = (int)Math.Ceiling((decimal)totaItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totaItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;


        }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }

}