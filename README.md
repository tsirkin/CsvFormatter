# CsvFormatter
Format your model as a csv.

Annotate your model class with CsvExported and call FormatTitle & FormatItem to produce csv output.
Example (.NET Core 2)

Annotate your model class:
```c#
    class Model
    {
        [CsvExported(0, "Field1")]
        public int field1 = 1;
        [CsvExported(1, "Property1")]
        public string field2 { get ; set; }
    }
```

And format it as csv:

```c#
    var formatter = new CsvFormatter();
    var model = new Model();
    var buffer = new StringBuilder();
    buffer.Append(formatter.FormatTitle(Model));
    buffer.Append("\n");
    buffer.Append(formatter.FormatItem(model));
```
