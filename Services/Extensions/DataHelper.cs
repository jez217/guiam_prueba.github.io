using Microsoft.Data.SqlClient;
using System.Data;

namespace Pautas.Services.Extensions
{
    public static class DataHelper
    {
        public static string ToStringDateTime(DateTime? date)
        {
            if (date != null)
            {
                DateTime date2 = date ?? new DateTime();

                return date2.ToString("yyyy/MM/dd hh:mm tt");
            }

            return "";
        }

        public static int? GetInt32Null(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetInt32(i);
            }
        }

        public static decimal? GetDecimalNull(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetDecimal(i);
            }
        }

        public static string GetStringNull(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                var datos = data.GetValue(i);
                return data.GetValue(i).ToString();
            }
        }

        public static DateTime? GetDateTimeNull(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetDateTime(i);
            }
        }

        public static TimeSpan? GetTimeSpanNull(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetTimeSpan(i);
            }
        }

        public static bool? GetBooleanNull(this SqlDataReader data, int i)
        {

            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetBoolean(i);
            }
        }

        public static object GetValueNull(this SqlDataReader data, int i)
        {
            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetValue(i);
            }
        }

        public static double? GetDoubleNull(this SqlDataReader data, int i)
        {
            if (data.IsDBNull(i))
            {
                return null;
            }
            else
            {
                return data.GetDouble(i);
            }
        }

        //**************************************************/

        public static int? GetInt32Null(this DataRow data, string ColumnName)
        {

            if (DBNull.Value == data[ColumnName])
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(data[ColumnName]);
            }
        }

        public static decimal? GetDecimalNull(this DataRow data, string ColumnName)
        {

            if (DBNull.Value == data[ColumnName])
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(data[ColumnName]);
            }
        }

        public static string GetStringNull(this DataRow data, string ColumnName)
        {

            if (DBNull.Value == data[ColumnName])
            {
                return null;
            }
            else
            {
                return Convert.ToString(data[ColumnName]);
            }
        }

        public static DateTime? GetDateTimeNull(this DataRow data, string ColumnName)
        {

            if (DBNull.Value == data[ColumnName])
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(data[ColumnName]);
            }
        }

        public static bool? GetBooleanNull(this DataRow data, string ColumnName)
        {

            if (DBNull.Value == data[ColumnName])
            {
                return null;
            }
            else
            {
                return Convert.ToBoolean(data[ColumnName]);
            }
        }

        //*************************

        public static DateTime? ToDateTimeNull(object obj)
        {

            if (obj == null)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }


    }

    public class DictionaryAdvanced<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, value);
        }

        public new void Remove(TKey key)
        {
            if (ContainsKey(key))
                base.Remove(key);
        }
    }
}
