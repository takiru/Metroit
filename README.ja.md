[English](README.md "English")

[チュートリアル](Tutorial/TUTORIAL.ja.md "チュートリアル")

|Module                |NuGet | Target Framework |
|----------------------|------|------------------|
|Metroit               |[![NuGet](https://img.shields.io/badge/nuget-v3.4.0-blue.svg)](https://www.nuget.org/packages/Metroit/) | `net8.0` `net9.0` `netstandard2.0` `netstandard2.1` `net45` |
|Metroit.Data          |[![NuGet](https://img.shields.io/badge/nuget-v2.0.0-blue.svg)](https://www.nuget.org/packages/Metroit.Data/) | `netstandard2.0` `netstandard2.1` `net45` |
|Metroit.Windows.Forms |[![NuGet](https://img.shields.io/badge/nuget-v3.4.3-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms/) | `net6.0-windows` `net8.0-windows` `net462` |

旧バージョン  

|Module                 |NuGet | Target Framework |
|-----------------------|------|------------------|
|Metroit.2              |[![NuGet](https://img.shields.io/badge/nuget-v1.1.0-blue.svg)](https://www.nuget.org/packages/Metroit.2/) | `net20` |
|Metroit.Data.2         |[![NuGet](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/Metroit.Data.2/) | `net20` |
|Metroit.Windows.Forms2 |[![NuGet](https://img.shields.io/badge/nuget-v1.1.1-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.2/) | `net20` |
|Metroit.45             |[![NuGet](https://img.shields.io/badge/nuget-v1.5.0-blue.svg)](https://www.nuget.org/packages/Metroit.45/) | `net45` |
|Metroit.Data.45        |[![NuGet](https://img.shields.io/badge/nuget-v1.2.6-blue.svg)](https://www.nuget.org/packages/Metroit.Data.45/) | `net45` |
|Metroit.Windows.Forms45|[![NuGet](https://img.shields.io/badge/nuget-v1.5.0.3-blue.svg)](https://www.nuget.org/packages/Metroit.Windows.Forms.45/) | `net45` |

# Metroit #
ロジックをサポートするいくつかのクラス、およびWinFormsの拡張機能コントロール。  
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

#### Enum を変換する ####
確実に定義済みEnumに変換します。

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

#### 文字列の長さを取得する ####
主に日本語を対象として、全角1文字を2文字として扱って長さを求めます。  
半角文字数を基準とした入力文字数を制限させる場合などに有効です。

```C#
using Metroit.Extensions;

var text = "123あいう";
var length = text.GetTextCount(true); // 9
```

#### 全角文字かどうか調べる ####
主に日本語を対象として、特定の1文字が全角文字かどうかを求めます。

```C#
using Metroit.Extensions;

var text = "あ";
var isFullWidth = text.IsFullWidth(true); // true
```

#### ASCII文字列の半角／全角変換を行う ####
主に日本語を対象として、ASCII文字の半角／全角変換を行います。

```C#
using Metroit.Text;

var halfText = @"aA=\";
var result = AsciiConverter.ToFullWidth(halfText);  // ａＡ＝＼
var result = AsciiConverter.ToFullWidth(halfText, true);  // ａＡ＝￥

var fullText = "ａＡ＝＼￥";
var result = AsciiConverter.ToHalfWidth(fullText);  // aA=\￥
var result = AsciiConverter.ToHalfWidth(fullText, true);  // aA=\\
```

#### カナ文字列の半角／全角変換を行う ####
主に日本語を対象として、カナ文字の半角／全角変換を行います。

```C#
using Metroit.Text;

var halfText = "ｶﾅｶﾞﾊﾞﾊﾟﾟ";
var result = KatakanaConverter.ToFullWidth(halfText);  // カナガバパ゜

var fullText = "カナガバパ゜";
var result = KatakanaConverter.ToHalfWidth(fullText);  // ｶﾅｶﾞﾊﾞﾊﾟﾟ
```

#### 文字列の変換を行う ####
主に日本語を対象として、文字列のASCII文字、カナ文字の半角／全角変換を行います。

```C#
using Metroit.Text;

var halfText = @"aA=\ｶﾅｶﾞﾊﾞﾊﾟﾟ";
var result = TextConverter.ToFullWidth(halfText, true, true, true);  // ａＡ＝￥カナガバパ゜

var fullText = "ａＡ＝＼￥カナガバパ゜";
var result = TextConverter.ToHalfWidth(fullText, true, true, true);  // aA=\\ｶﾅｶﾞﾊﾞﾊﾟﾟ
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

    MetDbProviderFactories.GetFactory() は、netstandard20 では利用できません。

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

[Microsoft.Web.WebView2](https://www.nuget.org/packages/Microsoft.Web.WebView2) を利用しているため、プロジェクトに合わせてプロジェクトファイルに以下を記載すると警告を抑制することができます。
```xml
  <Target Name="RemoveUnnecessaryWebView2References" AfterTargets="ResolvePackageDependenciesForBuild">
    <ItemGroup>
      <ReferenceToBeRemoved Include="@(Reference)" Condition="'%(Reference.FileName)' == 'Microsoft.Web.WebView2.WinForms' And '$(UseWindowsForms)' != 'true'" />
      <ReferenceToBeRemoved Include="@(Reference)" Condition="'%(Reference.FileName)' == 'Microsoft.Web.WebView2.Wpf' And '$(UseWpf)' != 'true'" />
      <Reference Remove="@(ReferenceToBeRemoved)" />
    </ItemGroup>
  </Target>
```

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

#### 開閉アイコンを表現するボタン ####
- MetExpanderButton  
  開閉アイコンを表現します。
  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    |State          |開閉状態。 |
    |Svg            |SVGイメージ。 |
    |Image          |通常画像イメージ。 |
    |IconStyle      |利用するイメージのスタイル。 |
    |ShowIcon       |アイコンを表示するかどうか。 |
    |ShowLine       |区切り線を表示するかどうか。 |
    |Text           |タイトル。 |
    |LineColor      |区切り線の色。 |
    |LineThickness  |区切り線の太さ。 |
    |HoverForeColor |マウスが表域内に入った時のタイトルの色。 |

  - イベント  

    |名前               |意味                             |
    |-------------------|---------------------------------|
    |ExpandStateChanged |開閉状態が変更した時に発生する。 |

#### 開閉アイコンを表現するボタンを有するパネル ####
- MetExpanderPanel  
  開閉アイコンを表現してパネルの開閉を可能にする。
  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    |State          |開閉状態。 |
    |Svg            |SVGイメージ。 |
    |Image          |通常画像イメージ。 |
    |IconStyle      |利用するイメージのスタイル。 |
    |ShowIcon       |アイコンを表示するかどうか。 |
    |ShowLine       |区切り線を表示するかどうか。 |
    |Text           |タイトル。 |
    |LineColor      |区切り線の色。 |
    |LineThickness  |区切り線の太さ。 |
    |HoverForeColor |マウスが表域内に入った時のタイトルの色。 |
    |HeaderFont |ヘッダーテキストのフォント。 |
    |HeaderForeColor |ヘッダーテキストの文字色。 |
    |HeaderPadding |ヘッダーのパディング。 |
    |UseAnimation |開閉にアニメーションを利用するかどうか。 |
    |Acceleration |アニメーションの加速度。 |
    |CollapsedHeaderLineVisibled |閉じた時に区切り線を表示するかどうか。 |

  - メソッド

    |名前           |意味 |
    |---------------|-----|
    |Expand(bool)   |パネルを開く。 |
    |Collapse(bool) |パネルを閉じる。 |

  - イベント  

    |名前               |意味                             |
    |-------------------|---------------------------------|
    |ExpandStateChanged |開閉状態が変更して開閉が完了した時に発生する。 |

#### オーバーレイ ####
- MetOverlay  
  指定したコントロールを、制御が完了するまでオーバーレイで覆う。  
  
      MetOverlay は、コードで直接利用する必要があります。


  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | Opacity | オーバーレイの不透明度。 |
    | UseAnimation | ローディングアニメーションを表示するかどうか。 |
    | LoadingAnimationSetting | ローディングアニメーションの設定。 |
    | Cancellation | キャンセル可能かどうか。 |
    | OverlayShown | オーバーレイが表示されたときに走行。 |
    | ProcessCompleted | 制御が完了したときに走行。 |
    | ProcessCancelled | 制御がキャンセルされたときに走行。 |
    | SynchronizationContext | UIスレッドの同期コンテキスト。 |

  - メソッド

    |名前           |意味 |
    |---------------|-----|
    | Show(Control, Func<CancellationToken, bool>) | オーバーレイを表示します。 |
    | Cancel() | オーバーレイをキャンセルします。 |
    | Dispose() | オーバーレイを非表示にして、オブジェクトを破棄します。 |

#### 拡張された Panel ####
- MetPanel  
  パネルの表現について、いくつかの手助けをします。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | ScrollPreserve | Controls にコントロールが追加または削除されたとき、スクロールバーの位置を保持するかどうかを取得または設定します。 |

#### 角丸の Panel ####
- MetRoundedPanel  
  パネルのボーダーに角丸を表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | Radius | 角丸の半径を取得または設定します。 |
    | BorderColor | 枠線の色を取得または設定します。 |
    | BorderWidth | 枠線の幅を取得または設定します。 |
    | FillColor | 塗りつぶし色を取得または設定します。 |

#### 通知 Panel ####
- MetNotificationPanel  
  情報の通知を行うパネルを表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | ShowTitle | タイトルを表示するかどうかを取得または設定します。 |
    | Title | タイトルを取得または設定します。 |
    | TitleForeColor | タイトルの文字色を取得または設定します。 |
    | TitleFont | タイトルのフォントを取得または設定します。 |
    | BorderColor | 枠線の色を取得または設定します。 |
    | BorderWidth | 枠線の太さを取得または設定します。 |
    | BorderDashStyle | 枠線のスタイルを取得または設定します。 |
    | VerticalScrollPolicy | 縦スクロールバーの表示方法を取得または設定します。 |
    | HorizontalScrollPolicy | 横スクロールバーの表示方法を取得または設定します。 |
    | NotificationBackColor | 通知領域の背景色を取得または設定します。 |
    | SeparatorColor | 通知情報ごとに表示される区切り線の色を取得または設定します。 |
    | SeparatorDashStyle | 区切り線のスタイルを取得または設定します。 |
    | NotificationFont | 通知情報のフォントを取得または設定します。 |
    | NotificationForeColor | 通知情報の文字色を取得または設定します。 |
    | NotificationHoverForeColor | 通知情報をホバーしたときの文字色を取得または設定します。 |

  - メソッド

    |名前           |意味 |
    |---------------|-----|
    | AddNotification(NotificationInfo) | 通知を追加します。 |
    | AddNotifications(IEnumerable<NotificationInfo>) | 通知のコレクションを追加します。 |
    | RemoveNotification(int) | 通知を削除します。 |
    | GetNotificationCount() | 通知の件数を取得します。 |
    | Clear() | すべての通知をクリアします。 |

#### トグルスイッチ ####
- MetToggleSwitch  
  トグルスイッチを表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | Checked | トグルスイッチの状態を取得または設定します。 |
    | CornerRadius | トグルスイッチの角丸半径を取得または設定します。 |
    | FocusColor | フォーカスを得たときに表示されるフレームの境界線色を取得または設定します。 |
    | OffAppearance | OFFのときの外観を決定します。 |
    | OnAppearance | ONのときの外観を決定します。 |
    | ShowState | トグルスイッチの状態を表示するかどうかを示す値を取得または設定します。 |
    | StatePosition | トグルスイッチの状態を表示する位置を取得または設定します。 |
    | AutoSize | トグルスイッチの自動サイズ調整を有効または無効にします。 |

  OffAppearance, OnAppearance はそれぞれ、下記プロパティを持ちます。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | BackColor | トグルスイッチの背景色を取得または設定します。 |
    | BorderColor | トグルスイッチのボーダー色を取得または設定します。 |
    | DisabledBackColor | Enabled = false のときのトグルスイッチの背景色を取得または設定します。 |
    | DisabledThumbColor | Enabled = false のときのサム円の色を取得または設定します。 |
    | Text | テキストを取得または設定します。 |
    | TextForeColor | テキストの前景色を取得または設定します。 |
    | ThumbColor | サム円の色を取得または設定します。 |

  - イベント  

    |名前           |意味 |
    |---------------|-----|
    | CheckedChanging | Checked プロパティが変更されるときに発生します。 |
    | CheckedChanged | Checked プロパティが変更されたときに発生します。 |

#### 拡張されたボタン ####
- MetRoundedButton  
  角丸なボタンを表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | ExtendsAppearance | 拡張された外観を決定します。 |
    | Radius | 角丸の丸みの半径を取得または設定します。 |

  ExtendsAppearance は下記プロパティを持ちます。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | BorderColor | ボタンを囲む境界線の色を取得または設定します。 |
    | BorderSize | ボタンを囲む境界線のサイズを取得または設定します。 |
    | FocusOverlayColor | フォーカスを得たときに表示されるオーバーレイの色を取得または設定します。 |
    | FocusOverlayWidth | フォーカスを得たときに表示されるオーバーレイの幅を取得または設定します。 |
    | MouseDownBackColor | マウスがコントロールの境界内でクリックされたときの背景色を取得または設定します。 |
    | MouseDownForeColor | マウスがコントロールの境界内でクリックされたときの前景色を取得または設定します。 |
    | MouseOverBackColor | マウスがコントロールの境界内にあるときの背景色を取得または設定します。 |
    | MouseOverForeColor | マウスがコントロールの境界内にあるときの前景色を取得または設定します。 |

#### 拡張されたチェックボックス ####
- MetCheckBox  
  拡張されたチェックボックスを表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | CheckBoxRadius | チェックボックス描画の角丸半径を取得または設定します。 |
    | CheckColor | チェックの色を取得または設定します。 |
    | CheckedAppearance | チェックされたときの外観を取得または設定します。 |
    | FocusColor | フォーカスを得たときに表示されるフレームの境界線色を取得または設定します。 |
    | FocusWidth | フォーカスを得たときに表示されるフレームの幅を取得または設定します。 |
    | UncheckedAppearapce | チェックされていないときの外観を取得または設定します。 |

  CheckedAppearance, UncheckedAppearapce はそれぞれ、下記プロパティを持ちます。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | Default | 既定の外観を取得または設定します。 |
    | MouseDown | マウスがクリックされたときの外観を取得または設定します。 |
    | MouseOver | マウスカーソルが領域内に入ったときの外観を取得または設定します。 |

  Default, MouseDown, MouseOver はそれぞれ、下記プロパティを持ちます。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | BackColor | 背景色を取得または設定します。 |
    | BorderColor | ボーダー色を取得または設定します。 |

  - イベント  

    |名前           |意味 |
    |---------------|-----|
    | CheckedChanging | Checked プロパティが変更されるときに発生します。 |
    | CheckStateChanging | CheckState プロパティが変更されるときに発生します。 |


#### 拡張されたラジオボタン ####
- MetRoundedRadioButton  
  拡張されたラジオボタンを表現します。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | CheckedBackColor | チェック時の背景色を取得または設定します。 |
    | CheckedForeColor | チェック時の前景色を取得または設定します。 |
    | ExtendsAppearance | 外観を取得または設定します。 |
    | Radius | 角丸の丸みの半径を取得または設定します。 |

  ExtendsAppearapce は下記プロパティを持ちます。

  - プロパティ  

    |名前           |意味 |
    |---------------|-----|
    | BorderColor | ボタンを囲む境界線の色を取得または設定します。 |
    | BorderSize | ボタンを囲む境界線のサイズを取得または設定します。 |
    | FocusOverlayColor | フォーカスを得たときに表示されるオーバーレイの色を取得または設定します。 |
    | FocusOverlayWidth | フォーカスを得たときに表示されるオーバーレイの幅を取得または設定します。 |
    | MouseDownBackColor | マウスがコントロールの境界内でクリックされたときの背景色を取得または設定します。 |
    | MouseDownForeColor | マウスがコントロールの境界内でクリックされたときの前景色を取得または設定します。 |
    | MouseOverBackColor | マウスがコントロールの境界内にあるときの背景色を取得または設定します。 |
    | MouseOverForeColor | マウスがコントロールの境界内にあるときの前景色を取得または設定します。 |
