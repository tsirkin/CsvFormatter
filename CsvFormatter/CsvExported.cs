using System;
using System.Collections.Generic;
using System.Text;

namespace Csv
{
    /// <summary>
    /// Provides an annotation for creating csv files from model classes.
    /// Gives a way to annotate fields that need to be exported specifying 
    /// the order and the title of the field.
    /// <code>
    /// [CsvExported(1, "TheFieldTitle")] 
    /// </code>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field |
                       System.AttributeTargets.Property)  // Multiuse attribute.  
    ]
    public class CsvExported : Attribute
    {
        private int _order;
        private string _title;
        public int Order
        {
            get { return _order; }
        }
        public string Title
        {
            get { return _title; }
        }
        public CsvExported(int Order, string Title = "")
        {
            _order = Order;
            _title = Title;
        }
    }
}
