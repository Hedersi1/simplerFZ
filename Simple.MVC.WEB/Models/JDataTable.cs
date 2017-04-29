using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simple.MVC.WEB.Models
{
    public class JDataTable
    {
        private string _searchText;
        public int Draw { get; set; }
        public int Start { get; set; }
        public int PageSize { get; set; }
        public string SearchText
        {
            get
            {
                if (_searchText == null)
                    _searchText = "";

                return _searchText;
            }
            set
            {
                _searchText = value;
            }
        }
        public string Order { get; set; }

        public static JDataTable GetDataTableParams(HttpRequestBase request)
        {
            var dataTable = new JDataTable
            {
                Draw = Convert.ToInt32(request.Params["draw"]),
                Start = Convert.ToInt32(request.Params["start"]),
                PageSize = Convert.ToInt32(request.Params["length"]),
                SearchText = request.Params["searchText"],
            };

            var orderColumn = request.Params["order[0][column]"];
            dataTable.Order = request.Params["columns[" + orderColumn + "][data]"] + " " + request.Params["order[0][dir]"];

            return dataTable;
        }
    }
}