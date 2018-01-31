using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csv
{
    /// <summary>
    /// Format your model as a csv.
    /// Annotate your model class with CsvExported and call FormatTitle & FormatItem
    /// to produce csv output.
    /// Example (.NET Core 2)
    /// In your model class:
    /// <code>
    /// </code>
    /// <code>
    ///    var formatter = new CsvFormatter();
    ///    var model = new Model();
    ///    var buffer = new StringBuilder();
    ///    buffer.Append(formatter.FormatTitle(Model));
    ///    buffer.Append("\n");
///        buffer.Append(formatter.FormatItem(model));
    /// </code>
    /// </summary>
    public class CsvFormatter
    {
        public CsvFormatter()
        {
        }

        static private char[] _specialChars = new char[] { ',', '\n', '\r', '"' };
        private class OrderedAttr
        {
            public MemberInfo Info;
            public int Order;
            public string Title;
        }
        private Dictionary<Type, List<OrderedAttr>> ExportedAttrByType = new Dictionary<Type, List<OrderedAttr>>();

        private List<MemberInfo> GetExportedFields(object o)
        {
            return GetExportedFields(o.GetType());
        }
        private List<MemberInfo> GetExportedFields(Type type)
        {
            List<OrderedAttr> exportedFields;
            if (ExportedAttrByType.ContainsKey(type))
            {
                exportedFields = ExportedAttrByType[type];
            }
            else
            {
                ExportedAttrByType[type] = exportedFields = _GetExportAttributes(type);
            }
            exportedFields.Sort((a, b) => a.Order.CompareTo(b.Order));
            return exportedFields.Select(x => x.Info).ToList();
        }
        private List<string> GetExportedTitles(Type type)
        {
            List<OrderedAttr> exportedFields;
            if (ExportedAttrByType.ContainsKey(type))
            {
                exportedFields = ExportedAttrByType[type];
            }
            else
            {
                ExportedAttrByType[type] = exportedFields = _GetExportAttributes(type);
            }
            exportedFields.Sort((a, b) => a.Order.CompareTo(b.Order));
            return exportedFields.Select(x => x.Title).ToList();
        }

        private static List<OrderedAttr> _GetExportAttributes(Type type)
        {
            List<OrderedAttr> exportedFields = new List<OrderedAttr>();

            Action<MemberInfo> fAddFields = (info) =>
            {
                Attribute[] attrs = Attribute.GetCustomAttributes(info);
                foreach (Attribute attr in attrs)
                {
                    if (attr is CsvExported)
                    {
                        string title = ((CsvExported)attr).Title;
                        if (title.Equals(""))
                        {
                            title = info.Name;
                        }
                        exportedFields.Add(new OrderedAttr()
                        {
                            Info = info,
                            Order = ((CsvExported)attr).Order,
                            Title = title
                        });
                    }
                }
            };
            foreach (PropertyInfo info in type.GetProperties())
            {
                fAddFields(info);
            }
            foreach (FieldInfo info in type.GetFields())
            {
                fAddFields(info);
            }

            return exportedFields;
        }

        /// <summary>
        /// Format the titles
        /// </summary>
        /// <param name="type">Type definition with CSVExported annotations</param>
        /// <returns></returns>
        public string FormatTitle(Type type)
        {
            List<string> titles = GetExportedTitles(type);
            return string.Join(",", titles.Select(x => Escape(x)));
        }

        /// <summary>
        /// Format an object to CSV format
        /// </summary>
        /// <param name="o">The object to format</param>
        /// <returns></returns>
        public string FormatItem(object o)
        {
            List<MemberInfo> fields = GetExportedFields(o);
            List<string> values = new List<string>();
            foreach (MemberInfo field in fields)
            {
                if (field is FieldInfo)
                {
                    if (((FieldInfo)field).GetValue(o) == null)
                    {
                        values.Add("");
                        continue;
                    }
                    values.Add(((FieldInfo)field).GetValue(o).ToString());
                }
                if (field is PropertyInfo)
                {
                    if (((PropertyInfo)field).GetValue(o) == null)
                    {
                        values.Add("");
                        continue;
                    }
                    values.Add(((PropertyInfo)field).GetValue(o).ToString());
                }
            }
            return string.Join(",", values.Select(x => Escape(x)));
        }

        private string Escape(object o)
        {
            if (o == null)
            {
                return "";
            }
            string field = o.ToString();
            if (field.IndexOfAny(_specialChars) != -1)
            {
                // Delimit the entire field with quotes and replace embedded quotes with "".
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else return field;
        }
    }
}
