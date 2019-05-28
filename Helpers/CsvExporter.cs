using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitSense.Extensions.Helpers
{
    public sealed class CsvExporter<T>
    {
        private string headerString;
        private List<string> propNames;
        private IEnumerable<T> source;
        public CsvExporter(IEnumerable<T> objects)
        {
            if (objects == null || !objects.Any())
                throw new ArgumentException("collection is null or empty", nameof(objects));
            var underType = objects.First().GetType();
            var properties = underType.GetProperties();
            this.propNames = properties.Select(x => x.Name).ToList();
            headerString = string.Join(";", propNames);
            headerString += Environment.NewLine;
            source = objects;
        }

        public string GetResults()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(headerString);
            foreach (T item in source)
            {
                foreach (var propName in propNames)
                {
                    var value = item.GetType().GetProperty(propName).GetValue(item);
                    String converted = value?.ToString() ?? "N/A";

                    sb.Append(converted + ";");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}