[English](README.md "English")

[チュートリアル](Tutorial/TUTORIAL.ja.md "チュートリアル")

|Module                 |NuGet                                                                                                                       |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------|
|Metroit.2              |[![NuGet](https://img.shields.io/badge/nuget-v1.1.0-blue.svg)](https://www.nuget.org/packages/Metroit.2/)                   |
|Metroit.Data.2         |[![NuGet](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/Metroit.Data.2/)              |
|Metroit.Windows.Forms2 |[![NuGet](https://img.shields.io/badge/nuget-v1.1.1-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.2/)     |
|Metroit.45             |[![NuGet](https://img.shields.io/badge/nuget-v1.4.4.0-blue.svg)](https://www.nuget.org/packages/Metroit.45/)                  |
|Metroit.Data.45        |[![NuGet](https://img.shields.io/badge/nuget-v1.2.6.0-blue.svg)](https://www.nuget.org/packages/Metroit.Data.45/)             |
|Metroit.Windows.Forms45|[![NuGet](https://img.shields.io/badge/nuget-v1.4.6.1-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.45/)    |

# Metroit #
ロジックをサポートするいくつかのクラス、およびWinFormsの拡張機能コントロール。  
ターゲットフレームワークは.NET 2.0, 4.5です。  
WinFormsを利用した開発を行っている環境において、いくつかの手助けをします。  
よくありそうな動作を抜き出すことにより、面倒な実装を省きます。  
よく困ることや、WPF、EntityFrameworkの利用に制限がある時に、少しだけ問題をクリアにしてくれるかもしれません。

## Metroit ##
基本的なクラスが含まれるライブラリです。

#### 上限を設けたDictionary ####
意図的に上限を必要とするDictionaryを利用します。
```C#
var dic = new Metroit.Collections.Generic.LimitedDictionary<string, string>(3);
dic.Add("key1", "value1");
dic.Add("key2", "value2");
dic.Add("key3", "value3");
if (!dic.CanAdd()) {
    dic.Add("key4", "value4");  // ArgumentException
}
```
#### string クラスの拡張 ####
```C#
using Metroit.Extensions;

// 大文字を判断に、区切り文字を挿入する。
var value = "TestTestTest";
Console.WriteLine(value.InsertSeparator("_", SeparateJudgeType.UpperChar));  // Test_Test_Test

var value = "(aaa(bbb(ccc)ddd)eee)";
value = value.GetEnclosedText(); // aaa(bbb(ccc)ddd)eee
value = value.GetEnclosedText(); // bbb(ccc)ddd
value = value.GetEnclosedText(); // ccc
```
#### 丸め計算を行う ####
```C#
// 小数第二位を、3捨4入する。
var value = 1.24;
value = Metroit.MetMath.Round(1, 4, MidpointRounding.AwayFromZero); // 1.3

// 小数第三位を切り上げる。
var value = 1.123;
value = Metroit.MetMath.Ceiling(1, 2); // 1.13

// 小数第三位を切り捨てる。
var value = 1.123;
value = Metroit.MetMath.Floor(1, 2); // 1.12

// 小数第三位を切り捨てる。
var value = 1.123;
value = Metroit.MetMath.Truncate(1, 2); // 1.12
```
#### 何かの変換を行う ####
ConverterBase クラスを使って、何かを変換するクラスを作成することができます。
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
        // なにかの変換処理
    }
    private void TestConverter_Prepare(IConvertParameter parameter, CancelEventArgs e)
    {
        // 変換前準備処理
        // e.Cancel = true で変換キャンセル
    }
    private void TestConverter_ConvertCompleted(IConvertParameter parameter, ConvertCompleteEventArgs e)
    {
        // 変換完了処理
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
#### ファイルの変換を行う ####
FileConverterBase クラスを使って、ファイルを変換するクラスを作成することができます。
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
IFileConverterFactory, IFileConverterFactoryMetadata を使って、簡単なMEFを実現することもできます。
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
データベース操作を助ける機能が含まれるライブラリです。

#### データベースへ接続する ####
プロバイダーの不変名を覚える必要がなくなります。  
Dictionary による接続情報を設定できます。
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

#### クエリを発行する ####
DbConnection.CreateQueryCommand()は、自動的にBindByName()を実施します。
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

#### トランザクションを利用したクエリを発行する ####
DbTransaction.CreateQueryCommand()は、自動的にBindByName()を実施します。  
DbTransaction を作成後は、DbConnection を操作する必要がありません。
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

#### プロシージャの実行結果を得る ####
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

#### 取得したデータをオブジェクトで操作する ####
DataTable を生で扱うことを回避します。
```C#
using Metroit.Data.Extensions;

class Tbl1
{
    public string Column1 { get; set; }

    // プロパティ名 != カラム名の時は、ColumnAttribute を利用する
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

// 行単位に行う場合
var row = dt[0].ToEntity<Tbl1>();
Console.WriteLine(row.Column1);
```

#### クエリ文字列の作成を行う ####
クエリ文字列の生成を少し助けます。
```C#
using Metroit.Data;

var builder = new QueryBuilder("SELECT *, /* REP */");
builder.AddQueries(new List<string>() { "FROM TBL "});
builder.ReplaceQueries(new List<string, string>() { "REP", "COLUMN1" });
var query = builder.Build(); // SELECT *, COLUMN1 FROM TBL
```

#### クエリパラメーターの最適化を行う ####
クエリに指定されているパラメーターの接頭辞や記号を求めるものに最適化します。  
接頭辞は":", "@"を、記号は"?"を認識します。
```C#
using Metroit.Data;

var query = "SELECT * FROM TBL WHERE COLUMN1 = @COLUMN1";
var op = new QueryParameterOptimizer();
query = op.GetOptimizedText(query, QueryBindVariableType.ColonWithParam); // SELECT * FROM TBL WHERE COLUMN1 = :COLUMN1
```
---

## Metroit.Windows.Forms ##
WinForms アプリケーションの作成を助けるライブラリです。

#### 拡張された Form ####
- MetForm  
  少しのUI動作とロジックを手助けします。
  - プロパティ  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |EnterFocus          |Enterキーでフォーカス遷移するかどうか。                 |
    |EscPush             |ESCキーの動作。                                         |
    |Request             |リクエストデータ。                                      |
    |Response            |レスポンスデータ。                                      |

  - イベント
  
    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |ControlRollbacking  |ESCキーによるロールバックの実施検証をする。             |
    |ControlLeaving      |ESCキーによるフォーカスアウトの実施検証をする。         |
    
  - メソッド  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |Show                |リクエストを画面に送ってモードレス表示する。            |
    |ShowDialog          |リクエストを画面に送ってモーダル表示する。              |

#### 拡張された TextBox ####
- MetTextBox  
  いくつかのUI動作とロジックを手助けします。
  - プロパティ  

    |名前                    |意味                                                    |
    |------------------------|--------------------------------------------------------|
    |AutoFocus               |最大入力桁まで入力されたら、次のコントロールへ遷移する。|
    |FocusSelect             |フォーカスを得た時、文字を反転させるかどうか。          |
    |MultilineSelectAll      |Multilineの時、Ctrl+Aを有効にする。                     |
    |BaseBackColor           |基本の背景色。                                          |
    |BaseForeColor           |基本の文字色。                                          |
    |FocusBackColor          |フォーカスを得た時の背景色。                            |
    |FocusForeColor          |フォーカスを得た時の文字色。                            |
    |ReadOnlyLabel           |Label に置き換えるかどうか。                            |
    |CustomAutoCompleteBox   |カスタムオートコンプリートの設定。                      |
    |CustomAutoCompleteKeys  |カスタムオートコンプリートを表示するキー。              |
    |CustomAutoCompleteMode  |カスタムオートコンプリートを利用する方法。              |
    |BaseBorderColor         |基本の外枠色。                                          |
    |FocusBorderColor        |フォーカスを得た時の外枠色。                            |
    |ErrorBorderColor        |エラー時の外枠色。                                      |
    |Error                   |エラーかどうか。                                        |

        ReadOnlyLabel は、 Label に置き換えます。
        BackColor, ForeColor は、ロジックからのみ利用可能です。  
        ただし、フォーカス遷移により、BaseBackColor, FocusBackColor, BaseForeColor, FocusForeColor が優先されます。

  - イベント  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |TextChangeValidation|入力された値の受入検証をする。                          |
    
        TextChangeValidationは、以下の操作では発生しません。
        - AutoComplete  
        - 元に戻す(コンテキストメニュー, Ctrl+Z)  

- MetLimitedTextBox  
  MetTextBox を継承します。  
  文字入力の制限を必要する場合の手助けをします。  
  - プロパティ  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptsChar         |入力を受け入れる文字の種類を指定する。                  |
    |ByteEncoding        |MaxByteLengthの判定に利用する文字エンコーディング。       |
    |CustomChars         |カスタム指定時の受け付ける文字を指定する。              |
    |ExcludeChars        |文字の種類内で、受け入れない文字を指定する。            |
    |FullSignSpecialChars|全角記号指定時に、他にも受け入れる全角記号を指定する。  |
    |MaxByteLength       |入力を許可する最大バイト数。                            |

- MetNumericTextBox  
  MetTextBox を継承します。  
  数値入力の制限を必要とする場合の手助けをします。
  - プロパティ  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptNegative      |負数を受け入れるかどうか。                              |
    |AcceptNull          |nullを受け入れるかどうか。                              |
    |CurrencySymbol      |数値の表現方法が通貨の時の記号。                        |
    |DecimalDigits       |入力可能な小数桁数。                                    |
    |DecimalSeparator    |整数と小数の区切り文字。                                |
    |GroupSeparator      |整数の区切り文字。                                      |
    |GroupSizes          |整数の区切る位置。                                      |
    |MaxValue            |入力を許可する最大値。                                  |
    |MinValue            |入力を許可する最小値。                                  |
    |NegativePattern     |負数の時の表現方法。                                    |
    |NegativeSign        |負数の表現符号。                                        |
    |PercentSymbol       |数値の表現方法がパーセントの時の記号。                  |
    |PositivePattern     |正数の時の表現方法。                                    |
    |Mode                |数値の表現方法。                                        |
    |NegativeForeColor   |負数の時の文字色。                                      |
    |Value               |入力値。                                                |

        下記のプロパティは利用できません。  
        ImeMode, MaxLength, Multiline, PasswordChar, UseSystemPasswordChar, AcceptsReturn, AcceptsTab, CharacterCasing, Lines, ScrollBars, RightToLeft, MultilineSelectAll

#### 拡張された DateTimePicker ####
- MetDateTimePicker  
  日付の入力について、いくつかの手助けをします。
  - プロパティ  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |AcceptNull          |nullを受け入れるかどうか。                              |
    |ReadOnly            |読み取り専用にするかどうか。                            |
    |ReadOnlyLabel       |Label に置き換えるかどうか。                            |
    |Value               |入力値。                                                |
    |BaseBackColor       |基本の背景色。                                          |
    |BaseForeColor       |基本の文字色。                                          |
    |FocusBackColor      |フォーカスを得た時の背景色。                            |
    |FocusForeColor      |フォーカスを得た時の文字色。                            |
    |BaseBorderColor     |基本の外枠色。                                          |
    |FocusBorderColor    |フォーカスを得た時の外枠色。                            |
    |ErrorBorderColor    |エラー時の外枠色。                                      |
    |Error               |エラーかどうか。                                        |
    |MinCalendarType     |表現するカレンダー、編集可能な月日のレベル。            |
    |ShowToday           |カレンダーに今日の日付を表示するかどうか。              |
    |ShowTodayCircle     |カレンダーに今日の日付をマークするかどうか。            |
    |ShowTorailingDates  |前月と次月の日付を当月のカレンダーに表示するかどうか。  |

        ReadOnly は、TextBox に置き換えます。
        ReadOnlyLabel は、 Label に置き換えます。

#### 拡張された ComboBox ####
- MetComboBox  
  プルダウンの入力について、いくつかの手助けをします。
  - プロパティ  

    |名前                |意味                                                    |
    |--------------------|--------------------------------------------------------|
    |ReadOnly            |読み取り専用にするかどうか。                            |
    |ReadOnlyLabel       |Label に置き換えるかどうか。                            |
    |BaseBackColor       |基本の背景色。                                          |
    |BaseForeColor       |基本の文字色。                                          |
    |FocusBackColor      |フォーカスを得た時の背景色。                            |
    |FocusForeColor      |フォーカスを得た時の文字色。                            |
    |BaseBorderColor     |基本の外枠色。                                          |
    |FocusBorderColor    |フォーカスを得た時の外枠色。                            |
    |ErrorBorderColor    |エラー時の外枠色。                                      |
    |Error               |エラーかどうか。                                        |
    
        ReadOnly は、TextBox に置き換えます。
        ReadOnlyLabel は、 Label に置き換えます。
        DropDownStyle=DropDownList の時、DrawModeはOwnerDrawFixedまたはOwnerDrawVariableでなければなりません。
