using Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvFormatterTest
{
    class Model
    {
        [CsvExported(0, "Field1")]
        public int field1 = 1;
        [CsvExported(1, "Property1")]
        public string field2 { get ; set; }
    }
}
