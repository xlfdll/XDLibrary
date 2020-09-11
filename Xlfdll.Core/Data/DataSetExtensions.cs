using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Xlfdll.Data
{
    public static class DataSetExtensions
    {
        public static String ToCSVString(this DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<String> columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);

            sb.AppendLine(String.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<String> fields = row.ItemArray.Select(field => field.ToString());

                sb.AppendLine(String.Join(",", fields));
            }

            return sb.ToString();
        }
    }
}