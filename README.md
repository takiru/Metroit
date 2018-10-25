[Japanese](README.ja.md "Japanese")

|Module                 |NuGet                                                                                                                       |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------|
|Metroit.2              |[![NuGet](https://img.shields.io/badge/nuget-v1.0.4-blue.svg)](https://www.nuget.org/packages/Metroit.2/)                   |
|Metroit.Data.2         |[![NuGet](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/Metroit.Data.2/)              |
|Metroit.Windows.Forms2 |[![NuGet](https://img.shields.io/badge/nuget-v1.0.11-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.2/)     |
|Metroit.45             |[![NuGet](https://img.shields.io/badge/nuget-v1.0.4-blue.svg)](https://www.nuget.org/packages/Metroit.45/)                  |
|Metroit.Data.45        |[![NuGet](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/Metroit.Data.45/)             |
|Metroit.Windows.Forms45|[![NuGet](https://img.shields.io/badge/nuget-v1.0.11-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.45/)    |

# Metroit #
Several classes to support logic, and WinForms extension control. Target framework is .NET 2.0, 4.5.  
We will do some help in an environment where development using WinForms is done.  
It eliminates troublesome implementation by pulling out what is likely to be good.  
It may be a little cleared of the problem if there are problems that you are having troubles, or if you have restrictions on using WPF or EntityFramework.

## Metroit ##
It is a library that contains basic classes.

#### Dictionary with upper limit ####
We will use a Dictionary that intentionally needs an upper limit.
```C#
var dic = new Metroit.Collections.Generic.LimitedDictionary<string, string>(3);
dic.Add("key1", "value1");
dic.Add("key2", "value2");
dic.Add("key3", "value3");
if (!dic.CanAdd()) {
    dic.Add("key4", "value4");  // ArgumentException
}
```
#### Extension of string class ####
```C#
using Metroit.Extensions;

// Insert delimiters on judgment of capital letters.
var value = "TestTestTest";
Console.WriteLine(value.InsertSeparator("_", SeparateJudgeType.UpperChar));  // Test_Test_Test
```
#### Rounding calculation ####
```C#
// The decimal second place leaves it off in 4 or more.
var value = 1.24;
value = Metroit.MetMath.Round(1, 4, MidpointRounding.AwayFromZero); // 1.3

// I leave off the decimal third place
var value = 1.123;
value = Metroit.MetMath.Ceiling(1, 2); // 1.13

// I cut off the decimal third place
var value = 1.123;
value = Metroit.MetMath.Floor(1, 2); // 1.12

// I cut off the decimal third place
var value = 1.123;
value = Metroit.MetMath.Truncate(1, 2); // 1.12
```
#### Some kind of conversion ####
With ConverterBase class, I can make a class converting something.
```C#
using Metroit.IO;

class TestConverter : ConverterBase
{
    public TestConverter() :base()
    {
        Prepare += TestConverter_Prepare;
        ConvertCompleted += TestConverter_ConvertCompleted;
    }
    protected void DoConvert(IConverterParameter parameter)
    {
        // I convert something
    }
    private void TestConverter_Prepare(IConvertParameter parameter, CancelEventArgs e)
    {
        // Preparations processing
        // e.Cancel = true is cancel conversion
    }
    private void TestConverter_ConvertCompleted(IConvertParameter parameter, ConvertCompleteEventArgs e)
    {
        // Completed processing
        switch (e.Result) {
            case ConvertResultType.Succeed:
                break;
            case ConvertResultType.Failed:
                Console.WriteLine(e.Error.ToString());
                break;
            case ConvertResultType.Cancelled:
                break;
        }
    }
}

class Test
{
    var converter = new TestConverter();
    var result = converter.Convert();
    if (result == ConvertResultType.Cancelled)
    {
        return;
    }
    // var t = converter.ConvertAsync(); // 4.5 only
}
```
#### Conversion of the file ####
With FileConverterBase class, I can make a class converting a file.
```C#
using Metroit.IO;

class TestFileConverter : FileConverterBase
{
    public TestFileConverter() : base() { }

    protected override void ConvertFile(FileConvertParameter parameter)
        // I perform the conversion handling of file
        File.Copy(Parameter.SourceFilePath, Parameter.ConvertingPath);
    }
}

class Test
{
    public void Hoge()
    {
        var parameter = new FileConvertParameter()
        {
            SourceFilePath = "C:\test.txt",
            DestinationFilePath = "D:\test.dat",
            UseTemporary = true,
            Overwrite = true,

        };
        var converter = new TestFileConverter();
        converter.Prepare += (p, e) =>
        {
            var fp = p as FileConvertParameter;
            e.Cancel = false;
            Console.WriteLine("Convert prepare process.");
        };
        converter.ConvertCompleted += (p, e) =>
        {
            var fp = p as FileConvertParameter;
            Console.WriteLine(e.Result.ToString() + e.Error.Message);
            Console.WriteLine("Convert complete process.");
        };
        var result = converter.Convert(parameter);
        if (result == ConvertResultType.Cancelled)
        {
            return;
        }
    }
}
```
Using IFileConverterFactory, IFileConverterFactoryMetadata, I can realize simple MEF.
```C#
using Metroit.IO;

[Export(typeof(IFileTypeConverterFactory))]
[ExportMetadata("ConverterName", "XlsToPdf")]
[ExportMetadata("FromType", "xls")]
[ExportMetadata("ToType", "pdf")]
public class TestConverterFactory : IFileTypeConverterFactory
{
    public IO.FileTypeConverter Create()
    {
        return new TestConverter();
    }
}
```
---


## Metroit.Data ##
A library that contains functions to aid database operation.

#### Connect to the database ####
You do not have to memorize the immutable name of the provider.  
You can set connection information by Dictionary.
```C#
using Metroit.Data.Common;
using Metroit.Data.Extensions;

var pf = MetDbProviderFactories.GetFactory(DatabaseType.Oracle);
using (var conn = pf.CreateConnection()) {
    conn.SetConnectionString(pf, new Dictionary<string, string>()
        {
            {"Data Source", "192.168.11.102/ORACLE" },
            {"User ID", "TEST" },
            {"Password","test" }
        });
    conn.Open();
}
```

#### Execute the query ####
DbConnection.CreateQueryCommand() will automatically enforce BindByName().
```C#
// conn : DbConnection
var query = "SELECT * FROM TBL WHERE COLUMN1 = :COLUMN1 AND COLUMN2 = :COLUMN2";
var comm = conn.CreateQueryCommand(query);
// pf : ProviderFactiry
comm.Parameters.Add(pf.CreateParameter("COLUMN1", "value"));
comm.Parameters.Add(pf.CreateParameter("COLUMN2", "value"));
var dt = new DataTable();
comm.Fill(pf, dt);
```

#### Execute a query using a transaction ####
DbConnection.CreateQueryCommand() will automatically enforce BindByName().  
After creating DbTransaction, you do not need to manipulate DbConnection.
```C#
using Metroit.Data.Extensions;

// conn : DbConnection
using (var trans = conn.BeginTransaction())
{
    var query = "SELECT * FROM TBL WHERE COLUMN1 = :COLUMN1 AND COLUMN2 = :COLUMN2";
    var comm = trans.CreateQueryCommand(query);
    // pf : ProviderFactiry
    comm.Parameters.Add(pf.CreateParameter("COLUMN1", "value"));
    comm.Parameters.Add(pf.CreateParameter("COLUMN2", "value"));
    var dt = new DataTable();
    var da = comm.Fill(pf, dt, true, true);
    da.Update(dt);
    trans.Commit();
}
```

#### Retrieve execution result of procedure ####
```C#
using Metroit.Data.Extensions;

// conn : DbConnection
var query = "ProcedureSample";
var comm = conn.CreateProcedureCommand(query);
// pf : ProviderFactiry
var parameter = pf.CreateParameter("COLUMN1", "value");
parameter.Direction = ParameterDirection.ReturnValue;
comm.Parameters.Add(pf);
comm.ExecuteNonQuery();
var result = comm.GetProcedureResult()
Console.WriteLine(result.ReturnValue.ToString());
```

#### Manipulate acquired data with objects ####
Avoid handling DataTable raw.
```C#
using Metroit.Data.Extensions;

class Tbl1
{
    public string Column1 { get; set; }

    // When property name != Column name, use ColumnAttribute
    [Column("COLUMN2")]
    public string ColumnPrpoerty2 { get; set; }
}

var dt = new DataTable();
comm.Fill(pf, dt);

foreach(var row in dt.AsEnumerableEntity<Tbl1>())
{
    Console.WriteLine(row.Column1);
    Console.WriteLine(row.ColumnPrpoerty2);
}

// When done line by line
var row = dt[0].ToEntity<Tbl1>();
Console.WriteLine(row.Column1);
```

#### Create a query string ####
It helps to generate a query string a little.
```C#
using Metroit.Data;

var builder = new QueryBuilder("SELECT *, /* REP */");
builder.AddQueries(new List<string>() { "FROM TBL "});
builder.ReplaceQueries(new List<string, string>() { "REP", "COLUMN1" });
var query = builder.Build(); // SELECT *, COLUMN1 FROM TBL
```

#### Perform query parameter optimization ####
It is optimized to find the prefix or symbol of the parameter specified in the query.  
The prefix recognizes ":", "@", and the symbol "?".
```C#
using Metroit.Data;

var query = "SELECT * FROM TBL WHERE COLUMN1 = @COLUMN1";
var op = new QueryParameterOptimizer();
query = op.GetOptimizedText(query, QueryBindVariableType.ColonWithParam); // SELECT * FROM TBL WHERE COLUMN1 = :COLUMN1
```
---

## Metroit.Windows.Forms ##
It is a library that helps to create WinForms application.

#### Extended Form ####
- MetForm  
  I will help with a bit of UI behavior and logic.
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |EnterFocus          |Whether to make focus transition with Enter key.                 |
    |EscPush             |Operation of the ESC key.                                         |
    |Request             |Request data.                                      |
    |Response            |Response data.                                      |

  - Events

    |Name                |Meaning                                                 |
    |--------------------|--------------------------------------------------------|
    |ControlRollbacking  |Perform rollback with ESC key Verify.                   |
    |ControlLeaving      |Implementation of focus out by ESC key Verify.          |

  - Methods  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |Show                |Send the request to the screen for modeless display.            |
    |ShowDialog          |Send the request to the screen for modal display.              |

#### Extended TextBox ####
- MetTextBox  
  We will help with some UI behaviors and logic.
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |AutoFocus           |When the maximum input digit is input, it transits to the next control.|
    |FocusSelect         |Whether to invert characters when focus is obtained.          |
    |MultilineSelectAll  |When Multiline, enable Ctrl + A.                     |
    |BaseBackColor       |Basic background color.                                          |
    |BaseForeColor       |Basic character color.                                          |
    |FocusBackColor      |Background color when you get focus.                            |
    |FocusForeColor      |Character color when focus is obtained.                            |
    |ReadOnlyLabel       |Whether to replace with Label.                            |
    |CustomAutoCompleteBox   |Setting up custom autocomplete.                      |
    |CustomAutoCompleteKeys  |Key to display custom autocomplete.              |
    |CustomAutoCompleteMode  |How to use custom autocomplete.          |
    |BaseOuterFrameColor     |Basic outer frame color.                                          |
    |FocusOuterFrameColor    |Outer frame color when you get focus.                            |
    |ErrorOuterFrameColor    |Outer frame color when error.                                      |
    |Error                   |Whether it is an error.                                        |

        Replace ReadOnlyLabel with Label.
        BackColor, ForeColor is only available from logic.  
        However, due to focus transition, BaseBackColor, FocusBackColor, BaseForeColor, FocusForeColor takes precedence.

  - Events  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |TextChangeValidation|Validate acceptance of the entered value.                          |
    
        TextChangeValidation does not occur with the following operations.
        - AutoComplete
        - Undo (context menu, Ctrl+Z)


- MetLimitedTextBox  
  Inherit MetTextBox.  
  We will help you when you need to restrict character input.  
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptsChar         |Specify the type of character that accepts input.                  |
    |ByteEncoding        |Character encoding used to determine MaxByteLength.       |
    |CustomChars         |Specify characters to accept when custom is specified.              |
    |ExcludeChars        |Specify unacceptable characters within the character type.            |
    |FullSignSpecialChars|When specifying a double-byte symbol, specify other double-width symbols to accept.  |
    |MaxByteLength       |Maximum number of bytes to allow input.                            |

- MetNumericTextBox  
  Inherit MetTextBox.  
  We will help you when you need to restrict numeric input.
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptNegative      |Whether to accept negative numbers.                              |
    |AcceptNull          |Whether to accept null.                              |
    |CurrencySymbol      |Symbol when the numerical expression method is currency.                        |
    |DecimalDigits       |Number of decimal places that can be entered.                                    |
    |DecimalSeparator    |Integer and decimal delimiter.                                |
    |GroupSeparator      |An integer delimiter.                                      |
    |GroupSizes          |The position where the integer is to be separated.                                      |
    |MaxValue            |Maximum value to allow input.                                  |
    |MinValue            |The minimum value to allow input.                                  |
    |NegativePattern     |Expression method when negative number.                                    |
    |NegativeSign        |Representation of negative number.                                        |
    |PercentSymbol       |Symbol when the numerical expression method is percent.                  |
    |PositivePattern     |Expression method when positive number.                                    |
    |Mode                |How to represent numbers.                                        |
    |NegativeForeColor   |Text color when negative.                                      |
    |Value               |Input value.                                                |

        The following properties are not available.  
        ImeMode, MaxLength, Multiline, PasswordChar, UseSystemPasswordChar, AcceptsReturn, AcceptsTab, CharacterCasing, Lines, ScrollBars, RightToLeft, MultilineSelectAll

#### Extended DateTimePicker ####
- MetDateTimePicker  
  I will help with some input of dates.
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptNull          |Whether to accept null.                              |
    |ReadOnly            |Whether to make it read-only.                            |
    |ReadOnlyLabel       |Whether to replace with Label.                            |
    |Value               |Input value.                                                |
    |BaseBackColor       |Basic background color.                                          |
    |BaseForeColor       |Basic character color.                                          |
    |FocusBackColor      |Background color when you get focus.                            |
    |FocusForeColor      |Character color when focus is obtained.                            |
    |BaseOuterFrameColor     |Basic outer frame color.                                          |
    |FocusOuterFrameColor    |Outer frame color when you get focus.                            |
    |ErrorOuterFrameColor    |Outer frame color when error.                                      |
    |Error                   |Whether it is an error.                                        |

        Replace ReadOnly with TextBox.
        Replace ReadOnlyLabel with Label.

#### Extended ComboBox ####
- MetComboBox  
  I will help with some input of pulldown.
  - Properties  

    |Name                |Meaning                                                    |
    |--------------------|--------------------------------------------------------|
    |ReadOnly            |Whether to make it read-only.                            |
    |ReadOnlyLabel       |Whether to replace with Label.                            |
    |BaseBackColor       |Basic background color.                                          |
    |BaseForeColor       |Basic character color.                                          |
    |FocusBackColor      |Background color when you get focus.                            |
    |FocusForeColor      |Character color when focus is obtained.                            |
    |BaseOuterFrameColor     |Basic outer frame color.                                          |
    |FocusOuterFrameColor    |Outer frame color when you get focus.                            |
    |ErrorOuterFrameColor    |Outer frame color when error.                                      |
    |Error                   |Whether it is an error.                                        |
    
        Replace ReadOnly with TextBox.
        Replace ReadOnlyLabel with Label.
        When DropDownStyle = DropDownList, DrawMode must be OwnerDrawFixed or OwnerDrawVariable.

