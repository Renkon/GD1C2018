﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Utils
{
    public class DatabaseUtils
    {
        // Convierte una lista a un DataTable
        // Source: https://social.msdn.microsoft.com/Forums/vstudio/en-US/6ffcb247-77fb-40b4-bcba-08ba377ab9db/converting-a-list-to-datatable?forum=csharpgeneral
        public static DataTable ConvertToDataTable<T>(IList<T> data, params string[] columns)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            // Modifico para que filtre las columnas que quiero enviar
            return new DataView(table).ToTable(false, columns);
        }

        // tomado prestado de https://stackoverflow.com/a/17001289/2980812
        public static string SHA256of(string str)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(str))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}
