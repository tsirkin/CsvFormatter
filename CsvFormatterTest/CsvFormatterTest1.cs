using Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace CsvFormatterTest
{
    [TestClass]
    public class CsvFormatterTest1
    {
        [TestMethod]
        public void TestFormatTitle()
        {
            var formatter = new CsvFormatter();
            var buffer = new StringBuilder();
            buffer.Append(formatter.FormatTitle(typeof(Model)));
            Assert.AreEqual("Field1,Property1", buffer.ToString());
        }
        [TestMethod]
        public void TestFormatValues()
        {
            var model = new Model()
            {
                field1 = 1,
                field2 = "field2 value"
            };
            var formatter = new CsvFormatter();
            var buffer = new StringBuilder();
            buffer.Append(formatter.FormatItem(model));
            Assert.AreEqual("1,field2 value", buffer.ToString());
            // With a comma 
            buffer = new StringBuilder();
            model = new Model()
            {
                field1 = 1,
                field2 = "field2,value"
            };
            buffer.Append(formatter.FormatItem(model));
            Assert.AreEqual("1,\"field2,value\"", buffer.ToString());
        }
    }
}
