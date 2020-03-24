using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.Config
{
    public class DataTableData<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        private List<T> newData;
        public List<T> data
        {
            get
            {
                return newData;
            }
            set
            {
                newData = value;
            }
        }
    }
}
