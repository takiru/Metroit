[Japanese](README.ja.md "Japanese")

[Tutorial](Tutorial/TUTORIAL.ja.md "Tutorial")

|Module                |NuGet | Target Framework |
|----------------------|------|------------------|
|Metroit               |[![NuGet](https://img.shields.io/badge/nuget-v3.4.0-blue.svg)](https://www.nuget.org/packages/Metroit/) | `net8.0` `net9.0` `netstandard2.0` `netstandard2.1` `net45` |
|Metroit.Data          |[![NuGet](https://img.shields.io/badge/nuget-v2.0.0-blue.svg)](https://www.nuget.org/packages/Metroit.Data/) | `netstandard2.0` `netstandard2.1` `net45` |
|Metroit.Windows.Forms |[![NuGet](https://img.shields.io/badge/nuget-v3.3.0-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms/) | `net6.0-windows` `net8.0-windows` `net462` |

Older Version  

|Module                 |NuGet | Target Framework |
|-----------------------|------|------------------|
|Metroit.2              |[![NuGet](https://img.shields.io/badge/nuget-v1.1.0-blue.svg)](https://www.nuget.org/packages/Metroit.2/) | `net20` |
|Metroit.Data.2         |[![NuGet](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/Metroit.Data.2/) | `net20` |
|Metroit.Windows.Forms2 |[![NuGet](https://img.shields.io/badge/nuget-v1.1.1-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.2/) | `net20` |
|Metroit.45             |[![NuGet](https://img.shields.io/badge/nuget-v1.5.0-blue.svg)](https://www.nuget.org/packages/Metroit.45/) | `net45` |
|Metroit.Data.45        |[![NuGet](https://img.shields.io/badge/nuget-v1.2.6-blue.svg)](https://www.nuget.org/packages/Metroit.Data.45/) | `net45` |
|Metroit.Windows.Forms45|[![NuGet](https://img.shields.io/badge/nuget-v1.5.0.3-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.45/) | `net45` |

# Metroit #
Several classes to support logic, and WinForms extension control.  
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

var value = "(aaa(bbb(ccc)ddd)eee)";
value = value.GetEnclosedText(); // aaa(bbb(ccc)ddd)eee
value = value.GetEnclosedText(); // bbb(ccc)ddd
value = value.GetEnclosedText(); // ccc
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
        // ファイルの変換処理など
        File.Copy(parameter.SourceConvertFileName, parameter.DestConvertFileName);
    }
}

class Test
{
    public void Hoge()
    {
        var converter = new TestFileConverter()
        {
            Parameter = new FileConvertParameter() {
                SourceFileName = "C:\test.txt",
                DestFileName = "D:\test.dat",
                UseDestTemporary = true,
                Overwrite = true
            },
            Prepare = (p, e) => {
                e.Cancel = false;
                Console.WriteLine("Convert prepare process.");
            },
            Complete(p, e) => {
                Console.WriteLine(e.Result.ToString() + e.Error?.Message);
                Console.WriteLine("Convert complete process.");
            }
        };
        var result = converter.Convert();
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

#### Converting Enums ####
Converts to a predefined enum reliably.

```C#
public enum ProcessType
{
    Succeed,
    Failed
}

var result = MetEnum<ProcessType>.Parse("1"); // ProcessType.Failed
var result = MetEnum<ProcessType>.Parse("Failed"); // ProcessType.Failed
var result = MetEnum<ProcessType>.Parse(2); // System.ArgumentException
```

#### Get the length of a string ####
This is mainly for Japanese and calculates the length by treating one full-width character as two characters.  
This is effective when you want to limit the number of input characters based on the number of half-width characters.

```C#
using Metroit.Extensions;

var text = "123あいう";
var length = text.GetTextCount(true); // 9
```

#### Check if it is a full-width character ####
This mainly targets Japanese and checks whether a specific character is a full-width character.

```C#
using Metroit.Extensions;

var text = "あ";
var isFullWidth = text.IsFullWidth(true); // true
```

#### Converts ASCII strings between half-width and full-width characters ####
It mainly targets Japanese and converts ASCII characters between half-width and full-width.

```C#
using Metroit.Text;

var halfText = @"aA=\";
var result = AsciiConverter.ToFullWidth(halfText);  // ａＡ＝＼
var result = AsciiConverter.ToFullWidth(halfText, true);  // ａＡ＝￥

var fullText = "ａＡ＝＼￥";
var result = AsciiConverter.ToHalfWidth(fullText);  // aA=\￥
var result = AsciiConverter.ToHalfWidth(fullText, true);  // aA=\\
```

#### Converts kana strings between half-width and full-width ####
It mainly targets Japanese and converts kana characters between half-width and full-width.

```C#
using Metroit.Text;

var halfText = "ｶﾅｶﾞﾊﾞﾊﾟﾟ";
var result = KatakanaConverter.ToFullWidth(halfText);  // カナガバパ゜

var fullText = "カナガバパ゜";
var result = KatakanaConverter.ToHalfWidth(fullText);  // ｶﾅｶﾞﾊﾞﾊﾟﾟ
```

#### Converting strings ####
It mainly targets Japanese and converts ASCII characters in strings and kana characters between half-width and full-width.

```C#
using Metroit.Text;

var halfText = @"aA=\ｶﾅｶﾞﾊﾞﾊﾟﾟ";
var result = TextConverter.ToFullWidth(halfText, true, true, true);  // ａＡ＝￥カナガバパ゜

var fullText = "ａＡ＝＼￥カナガバパ゜";
var result = TextConverter.ToHalfWidth(fullText, true, true, true);  // aA=\\ｶﾅｶﾞﾊﾞﾊﾟﾟ
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

    MetDbProviderFactories.GetFactory() is not available in netstandard20.

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

Since we are using [Microsoft.Web.WebView2](https://www.nuget.org/packages/Microsoft.Web.WebView2), you can suppress the warning by writing the following in the project file according to your project.
```xml
  <Target Name="RemoveUnnecessaryWebView2References" AfterTargets="ResolvePackageDependenciesForBuild">
    <ItemGroup>
      <ReferenceToBeRemoved Include="@(Reference)" Condition="'%(Reference.FileName)' == 'Microsoft.Web.WebView2.WinForms' And '$(UseWindowsForms)' != 'true'" />
      <ReferenceToBeRemoved Include="@(Reference)" Condition="'%(Reference.FileName)' == 'Microsoft.Web.WebView2.Wpf' And '$(UseWpf)' != 'true'" />
      <Reference Remove="@(ReferenceToBeRemoved)" />
    </ItemGroup>
  </Target>
```

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
    |BaseBorderColor     |Basic border color.                                          |
    |FocusBorderColor    |Border color when you get focus.                            |
    |ErrorBorderColor    |Border color when error.                                      |
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
    |BaseBorderColor     |Basic border color.                                          |
    |FocusBorderColor    |Border color when you get focus.                            |
    |ErrorBorderColor    |Border color when error.                                      |
    |Error               |Whether it is an error.                                        |
    |MinCalendarType     |Calendar to represent, editable month / day level.            |
    |ShowToday           |Whether to display today's date on the calendar.              |
    |ShowTodayCircle     |Whether to mark today's date on the calendar.            |
    |ShowTorailingDates  |Whether to display the dates of the previous month and the next month on the calendar for the current month.  |

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
    |BaseBorderColor     |Basic border color.                                          |
    |FocusBorderColor    |Border color when you get focus.                            |
    |ErrorBorderColor    |Border color when error.                                      |
    |Error               |Whether it is an error.                                        |
    
        Replace ReadOnly with TextBox.
        Replace ReadOnlyLabel with Label.
        When DropDownStyle = DropDownList, DrawMode must be OwnerDrawFixed or OwnerDrawVariable.

#### Button representing open/close icon ####
- MetExpanderButton  
  Represents an open/close icon.
  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    |State          |Open/closed state. |
    |Svg            |SVG image. |
    |Image          |Normal image. |
    |IconStyle      |The style of image to use. |
    |ShowIcon       |Whether to display the icon. |
    |ShowLine       |Whether to display separator lines. |
    |Text           |title. |
    |LineColor      |Separator line color. |
    |LineThickness  |Thickness of the separator line. |
    |HoverForeColor |The color of the title when the mouse enters the table area. |

  - Events  

    |Name               |Meaning                             |
    |-------------------|---------------------------------|
    |ExpandStateChanged |Occurs when the open/closed state changes. |

#### Panel with buttons representing open/close icons ####
- MetExpanderPanel  
  Express the open/close icon to enable opening/closing of the panel.
  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    |State          |Open/closed state. |
    |Svg            |SVG image. |
    |Image          |Normal image. |
    |IconStyle      |The style of image to use. |
    |ShowIcon       |Whether to display the icon. |
    |ShowLine       |Whether to display separator lines. |
    |Text           |title. |
    |LineColor      |Separator line color. |
    |LineThickness  |Thickness of the separator line. |
    |HoverForeColor |The color of the title when the mouse enters the table area. |
    |HeaderFont |Header text font. |
    |HeaderForeColor |Header text font color. |
    |HeaderPadding |Header padding. |
    |UseAnimation |Whether to use animation for opening and closing. |
    |Acceleration |Animation acceleration. |
    |CollapsedHeaderLineVisibled |Whether to display a separator line when closed. |

  - Methods  

    |Name           |Meaning |
    |---------------|-----|
    |Expand(bool)   |Open the panel. |
    |Collapse(bool) |Close the panel. |

  - Events  

    |Name               |Meaning                          |
    |-------------------|---------------------------------|
    |ExpandStateChanged |Occurs when the open/close state changes and the open/close is completed. |

#### Overlay ####
- MetOverlay  
  Covers the specified control with an overlay until the control is completed.  
  
      MetOverlay must be used directly in your code.


  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | Opacity | The opacity of the overlay. |
    | UseAnimation | Whether to show the loading animation. |
    | LoadingAnimationSetting | Loading animation settings. |
    | Cancellation | Whether cancellation is possible. |
    | OverlayShown | Running when the overlay appears. |
    | ProcessCompleted | Run when control is complete. |
    | ProcessCancelled | Running when control is cancelled. |
    | SynchronizationContext | The synchronization context for the UI thread. |

  - Methods

    |Name           |Meaning |
    |---------------|-----|
    | Show(Control, Func<CancellationToken, bool>) | Show the overlay. |
    | Cancel() | Cancel the overlay. |
    | Dispose() | Hides the overlay and destroys the object. |

#### Extended Panel ####
- MetPanel  
  Here's some help with the panel representation.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | ScrollPreserve | Gets or sets whether the scrollbar position is preserved when controls are added or removed from Controls. |

#### Rounded Corner Panel ####
- MetRoundedPanel  
  Creates rounded corners on the panel border.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | Radius | Gets or sets the corner radius. |
    | BorderColor | Gets or sets the border color. |
    | BorderWidth | Gets or sets the border width. |
    | FillColor | Gets or sets the fill color. |

#### Notification Panel ####
- MetNotificationPanel  
  Represents a panel that notifies users of information.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | ShowTitle | Gets or sets whether the title is displayed. |
    | Title | Gets or sets the title. |
    | TitleForeColor | Gets or sets the title text color. |
    | TitleFont | Gets or sets the title font. |
    | BorderColor | Gets or sets the border color. |
    | BorderWidth | Gets or sets the border thickness. |
    | BorderDashStyle | Gets or sets the border style. |
    | VerticalScrollPolicy | Gets or sets the display mode of the vertical scroll bar. |
    | HorizontalScrollPolicy | Gets or sets the visibility of the horizontal scroll bar. |
    | NotificationBackColor | Gets or sets the background color of the notification area. |
    | SeparatorColor | Gets or sets the color of the separator line that appears for each notification. |
    | SeparatorDashStyle | Gets or sets the style of the separator line. |
    | NotificationFont | Gets or sets the font of the notification. |
    | NotificationForeColor | Gets or sets the text color of the notification. |
    | NotificationHoverForeColor | Gets or sets the text color of the notification when hovered over. |

  - Methods

    |Name           |Meaning |
    |---------------|-----|
    | AddNotification(NotificationInfo) | Add a notification. |
    | AddNotifications(IEnumerable<NotificationInfo>) | Add a collection of notifications. |
    | RemoveNotification(int) | Delete the notification. |
    | GetNotificationCount() | Gets the number of notifications. |
    | Clear() | Clear all notifications. |

#### Toggle Switch ####
- MetToggleSwitch  
  Represents a toggle switch.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | Checked | Gets or sets the state of the toggle switch. |
    | CornerRadius | Gets or sets the corner radius of the toggle switch. |
    | FocusColor | Gets or sets the border color of the frame when it has focus. |
    | OffAppearance | Determines the appearance when OFF. |
    | OnAppearance | Determines the appearance when ON. |
    | ShowState | Gets or sets a value indicating whether to display the state of the toggle switch. |
    | StatePosition | Gets or sets the position at which the toggle switch state is displayed. |
    | AutoSize | Toggle switch to enable or disable autosize. |

  OffAppearance and OnAppearance each have the following properties.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | BackColor | Gets or sets the background color of the toggle switch. |
    | BorderColor | Gets or sets the border color of the toggle switch. |
    | DisabledBackColor | Gets or sets the background color of the toggle switch when Enabled = false. |
    | DisabledThumbColor | Gets or sets the color of the thumb circle when Enabled = false. |
    | Text | Gets or sets the text. |
    | TextForeColor | Gets or sets the foreground color of the text. |
    | ThumbColor | Gets or sets the color of the thumb circle. |

  - Events  

    |Name           |Meaning |
    |---------------|-----|
    | CheckedChanging | Occurs when the Checked property changing. |
    | CheckedChanged | Occurs when the Checked property changes. |

#### Extended Button ####
- MetRoundedButton  
  Represents a button with rounded corners.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | ExtendsAppearance | Determines the extended appearance. |
    | Radius | Gets or sets the radius of the rounded corners. |

  ExtendsAppearance has the following properties.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | BorderColor | Gets or sets the color of the border that surrounds the button. |
    | BorderSize | Gets or sets the size of the border that surrounds the button. |
    | FocusOverlayColor | Gets or sets the color of the overlay that is displayed when the control has focus. |
    | FocusOverlayWidth | Gets or sets the width of the overlay that is displayed when it has focus. |
    | MouseDownBackColor | Gets or sets the background color when the mouse is clicked within the bounds of the control. |
    | MouseDownForeColor | Gets or sets the foreground color when the mouse is clicked within the bounds of the control. |
    | MouseOverBackColor | Gets or sets the background color when the mouse is within the bounds of the control. |
    | MouseOverForeColor | Gets or sets the foreground color when the mouse is within the bounds of the control. |

#### Extended CheckBox ####
- MetCheckBox  
  Represents an expanded checkbox.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | CheckBoxRadius | Gets or sets the corner radius for drawing check boxes. |
    | CheckColor | Gets or sets the check color. |
    | CheckedAppearance | Gets or sets the appearance that the control appears when checked. |
    | FocusColor | Gets or sets the border color of the frame when it has focus. |
    | FocusWidth | Gets or sets the width of the frame that is displayed when it has focus. |
    | UncheckedAppearapce | Gets or sets the appearance when unchecked. |

  CheckedAppearance and UncheckedAppearance each have the following properties.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | Default | Gets or sets the default appearance. |
    | MouseDown | Gets or sets the appearance when the mouse is clicked. |
    | MouseOver | Gets or sets the appearance when the mouse cursor enters the region. |

  Default, MouseDown, and MouseOver each have the following properties.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | BackColor | Gets or sets the background color. |
    | BorderColor | Gets or sets the border color. |

  - イベント  

    |Name           |Meaning |
    |---------------|-----|
    | CheckedChanging | Occurs when the Checked property changes. |
    | CheckStateChanging | Occurs when the CheckState property changes. |


#### Extended RadioButton ####
- MetRoundedRadioButton  
  Represents an extended radio button.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | CheckedBackColor | Gets or sets the background color when checked. |
    | CheckedForeColor | Gets or sets the foreground color when checked. |
    | ExtendsAppearance | Gets or sets the appearance. |
    | Radius | Gets or sets the radius of the rounded corners. |

  ExtendsAppearapce has the following properties.

  - Properties  

    |Name           |Meaning |
    |---------------|-----|
    | BorderColor | Gets or sets the color of the border that surrounds the button. |
    | BorderSize | Gets or sets the size of the border that surrounds the button. |
    | FocusOverlayColor | Gets or sets the color of the overlay that is displayed when the control has focus. |
    | FocusOverlayWidth | Gets or sets the width of the overlay that is displayed when it has focus. |
    | MouseDownBackColor | Gets or sets the background color when the mouse is clicked within the bounds of the control. |
    | MouseDownForeColor | Gets or sets the foreground color when the mouse is clicked within the bounds of the control. |
    | MouseOverBackColor | Gets or sets the background color when the mouse is within the bounds of the control. |
    | MouseOverForeColor | Gets or sets the foreground color when the mouse is within the bounds of the control. |
