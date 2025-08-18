using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// 通知パネルを提供します。
    /// </summary>
    public partial class MetNotificationPanel : UserControl
    {
        /// <summary>
        /// 通知の追加/削除時に描画の更新を行うためのメッセージ送信。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

        /// <summary>
        /// ウィンドウの再描画メッセージ。
        /// </summary>
        private static readonly int WM_SETREDRAW = 0x000B;

        /// <summary>
        /// 通知パネル内に追加される通知ラベルとセパレーターの表示マージン。
        /// </summary>
        private static readonly int NotificationElementMargin = 5;

        /// <summary>
        /// 最後の通知に対するマージン用のパネル。
        /// </summary>
        private Panel _lastMarginPanel = new Panel()
        {
            Left = 0,
            Top = 0,
            Width = 0,
            Height = 0,
            BackColor = Color.Transparent,
            Tag = "LastMargin"
        };

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetNotificationPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コントロールが作成されたときに呼び出されます。
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            AdjustScrollablePanelArea();

            if (VerticalScrollPolicy == ScrollBarPolicy.Never && HorizontalScrollPolicy == ScrollBarPolicy.Never)
            {
                ScrollablePanel.AutoScroll = false;
                return;
            }

            if (HorizontalScrollPolicy == ScrollBarPolicy.Never)
            {
                ScrollablePanel.HorizontalScroll.Visible = false;
            }
            if (HorizontalScrollPolicy == ScrollBarPolicy.Always)
            {
                ScrollablePanel.AutoScrollMinSize = new Size(ScrollablePanel.Width + 1, ScrollablePanel.AutoScrollMinSize.Height);
            }

            if (VerticalScrollPolicy == ScrollBarPolicy.Never)
            {
                ScrollablePanel.VerticalScroll.Visible = false;
            }
            if (VerticalScrollPolicy == ScrollBarPolicy.Always)
            {
                ScrollablePanel.AutoScrollMinSize = new Size(ScrollablePanel.AutoScrollMinSize.Width, ScrollablePanel.Height + 1);
            }
        }

        /// <summary>
        /// AutoScrollプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("AutoScrollは使用できません。", true)]
#pragma warning disable CS0809
        public override bool AutoScroll { get => base.AutoScroll; set { } }
#pragma warning restore CS0809

        /// <summary>
        /// AutoScrollMinSizeプロパティを隠します（このコントロールでは使用しません）
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("AutoScrollMinSizeは使用できません。", true)]
        public new Size AutoScrollMinSize { get => base.AutoScrollMinSize; set { } }

        /// <summary>
        /// タイトルを表示するかどうかを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Metroit 表示")]
        [Description("タイトルを表示するかどうかを取得または設定します。")]
        public bool ShowTitle
        {
            get => TitleLabel.Visible;
            set => TitleLabel.Visible = value;
        }

        /// <summary>
        /// タイトルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Metroit 表示")]
        [Description("タイトルを取得または設定します。")]
        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        /// <summary>
        /// タイトルの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "ControlText")]
        [Category("Metroit 表示")]
        [Description("タイトルの文字色を取得または設定します。")]
        public Color TitleForeColor
        {
            get => TitleLabel.ForeColor;
            set => TitleLabel.ForeColor = value;
        }

        /// <summary>
        /// タイトルのフォントを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Metroit 表示")]
        [Description("タイトルのフォントを取得または設定します。")]
        public Font TitleFont
        {
            get => TitleLabel.Font;
            set => TitleLabel.Font = value;
        }

        /// <summary>
        /// タイトルのフォントが変更されたかどうかを取得する。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeTitleFont()
        {
            return TitleLabel.Font != Parent.Font;
        }

        /// <summary>
        /// タイトルのフォントを既定値にリセットする。
        /// </summary>
        private void ResetTitleFont()
        {
            TitleLabel.Font = Parent.Font;
        }

        private Color _borderColor = Color.Gray;

        /// <summary>
        /// 枠線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Gray")]
        [Category("Metroit 表示")]
        [Description("枠線の色を取得または設定します。")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                FramePanel.Invalidate();
            }
        }

        private int _borderWidth = 1;

        /// <summary>
        /// 枠線の太さを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        [Category("Metroit 表示")]
        [Description("枠線の太さを取得または設定します。")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                FramePanel.Invalidate();
            }
        }

        /// <summary>
        /// 枠線のスタイルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(DashStyle), "Solid")]
        [Category("Metroit 表示")]
        [Description("枠線のスタイルを取得または設定します。")]
        public DashStyle BorderDashStyle { get; set; } = DashStyle.Solid;

        private ScrollBarPolicy _verticalScroll = ScrollBarPolicy.AsNeeded;

        /// <summary>
        /// 縦スクロールバーの表示方法を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ScrollBarPolicy), "AsNeeded")]
        [Category("Metroit 表示")]
        [Description("縦スクロールバーの表示方法を取得または設定します。")]
        public ScrollBarPolicy VerticalScrollPolicy
        {
            get => _verticalScroll;
            set
            {
                _verticalScroll = value;

            }
        }

        private ScrollBarPolicy _horizontalScroll = ScrollBarPolicy.Never;

        /// <summary>
        /// 横スクロールバーの表示方法を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(ScrollBarPolicy), "Never")]
        [Category("Metroit 表示")]
        [Description("横スクロールバーの表示方法を取得または設定します。")]
        public ScrollBarPolicy HorizontalScrollPolicy
        {
            get => _horizontalScroll;
            set
            {
                _horizontalScroll = value;

            }
        }

        /// <summary>
        /// 通知領域の背景色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [Category("Metroit 表示")]
        [Description("通知領域の背景色を取得または設定します。")]
        public Color NotificationBackColor
        {
            get => ScrollablePanel.BackColor;
            set => ScrollablePanel.BackColor = value;
        }

        private static Color _defaultSeparatorColor = SystemColors.ActiveBorder;
        private Color _separatorColor = _defaultSeparatorColor;

        /// <summary>
        /// 通知情報ごとに表示される区切り線の色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Metroit 表示")]
        [Description("通知情報ごとに表示される区切り線の色を取得または設定します。")]
        public Color SeparatorColor
        {
            get => _separatorColor;
            set => _separatorColor = value;
        }

        /// <summary>
        /// 区切り線の色が変更されたかどうかを取得する。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeSeparatorColor()
        {
            return SeparatorColor != _defaultSeparatorColor;
        }

        /// <summary>
        /// 区切り線の色を既定値にリセットする。
        /// </summary>
        private void ResetSeparatorColor()
        {
            SeparatorColor = _defaultSeparatorColor;
        }
        /// <summary>
        /// 区切り線のスタイルを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(DashStyle), "Dash")]
        [Category("Metroit 表示")]
        [Description("通知情報ごとに表示される区切り線の色を取得または設定します。")]
        public DashStyle SeparatorDashStyle { get; set; } = DashStyle.Dash;

        /// <summary>
        /// 通知情報のフォントを取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Metroit 表示")]
        [Description("通知情報のフォントを取得または設定します。")]
        public Font NotificationFont
        {
            get => NotificationLabelStyle.Font;
            set
            {
                NotificationLabelStyle.Font = value;

                if (Created)
                {
                    RedrawNotificationItems();
                }
            }
        }

        /// <summary>
        /// 通知情報のフォントが変更されたかどうかを取得する。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeNotificationFont()
        {
            return NotificationLabelStyle.Font != Parent.Font;
        }

        /// <summary>
        /// 通知情報のフォントを既定値にリセットする。
        /// </summary>
        private void ResetNotificationFont()
        {
            NotificationLabelStyle.Font = Parent.Font;
        }

        /// <summary>
        /// 通知情報の文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Metroit 表示")]
        [Description("通知情報の文字色を取得または設定します。")]
        public Color NotificationForeColor
        {
            get => NotificationLabelStyle.ForeColor;
            set
            {
                NotificationLabelStyle.ForeColor = value;

                if (Created)
                {
                    RedrawNotificationItems();
                }
            }
        }

        /// <summary>
        /// 通知情報の文字色が変更されたかどうかを取得する。
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeNotificationForeColor()
        {
            return NotificationLabelStyle.ForeColor != Parent.ForeColor;
        }

        /// <summary>
        /// 通知情報の文字色を既定値にリセットする。
        /// </summary>
        private void ResetNotificationForeColor()
        {
            NotificationLabelStyle.ForeColor = Parent.ForeColor;
        }

        private Color _notificationHoverForeColor = Color.RoyalBlue;

        /// <summary>
        /// 通知情報をホバーしたときの文字色を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Color), "RoyalBlue")]
        [Category("Metroit 表示")]
        [Description("通知情報をホバーしたときの文字色を取得または設定します。")]
        public Color NotificationHoverForeColor
        {
            get => _notificationHoverForeColor;
            set
            {
                _notificationHoverForeColor = value;

                if (Created)
                {
                    RedrawNotificationItems();
                }
            }
        }

        /// <summary>
        /// 通知を追加します。
        /// </summary>
        /// <param name="notification">追加する通知。</param>
        public void AddNotification(NotificationInfo notification)
        {
            // 追加と幅の位置が確定するまで描画を止める
            SetRedrawNotifications(false);

            AddNotificatonCore(notification);
            AddLastMargin();
            RedrawNotificationItems();

            // 追加と幅の位置が確定したので描画を再開する
            SetRedrawNotifications(true);
            ScrollablePanel.Refresh();
        }

        /// <summary>
        /// 通知のコレクションを追加します。
        /// </summary>
        /// <param name="notifications">追加する通知のコレクション。</param>
        public void AddNotifications(IEnumerable<NotificationInfo> notifications)
        {
            // 追加と幅の位置が確定するまで描画を止める
            SetRedrawNotifications(false);

            foreach (var notification in notifications)
            {
                AddNotificatonCore(notification);
            }
            AddLastMargin();
            RedrawNotificationItems();

            // 追加と幅の位置が確定したので描画を再開する
            SetRedrawNotifications(true);
            ScrollablePanel.Refresh();
        }

        /// <summary>
        /// 通知を削除します。
        /// </summary>
        /// <param name="index">0 から始まる通知情報のインデックス。</param>
        public void RemoveNotification(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "インデックスは0から始まらなければなりません。");
            }

            // 通知ラベルのみを走査して、対象ラベルを求める
            var labels = ScrollablePanel.Controls
                .OfType<Label>()
                .Select((Value, Index) => new { Index, Value })
                .ToList();

            if (index > labels.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "インデックスが大きすぎます");
            }

            var label = labels.Where(x => x.Index == index).Single();

            var labelIndex = ScrollablePanel.Controls.IndexOf(label.Value);

            Control separator = null;
            if (labelIndex == 0)
            {
                // 2件以上の通知がある場合、次のセパレーターを削除対象にする
                if (GetNotificationCount() > 1)
                {
                    separator = ScrollablePanel.Controls[labelIndex + 1];
                }
            }
            else
            {
                // 最後の通知の場合、手前のセパレーターを削除対象にする
                // NOTE: コントロールの最後にはマージン用パネルがあるため、最後の通知は Controls.Count - 2 で取得できる。
                if (labelIndex == ScrollablePanel.Controls.Count - 2)
                {
                    separator = ScrollablePanel.Controls[labelIndex - 1];
                }
            }

            // 追加と幅の位置が確定するまで描画を止める
            SetRedrawNotifications(false);

            ScrollablePanel.Controls.Remove(label.Value);
            ScrollablePanel.Controls.Remove(separator);

            // すべての通知が削除されたとき、末尾のマージンも削除する
            if (ScrollablePanel.Controls.Count == 1)
            {
                ScrollablePanel.Controls.Clear();
            }

            RedrawNotificationItems();

            // 追加と幅の位置が確定したので描画を再開する
            SetRedrawNotifications(true);
            ScrollablePanel.Refresh();
        }

        /// <summary>
        /// 通知の件数を取得します。
        /// </summary>
        /// <returns>通知の件数。</returns>
        public int GetNotificationCount()
        {
            return ScrollablePanel.Controls
                .OfType<Label>()
                .Count();
        }

        /// <summary>
        /// すべての通知をクリアします。
        /// </summary>
        public void Clear()
        {
            ScrollablePanel.Controls.Clear();
        }

        /// <summary>
        /// 通知の描画を更新する。
        /// </summary>
        /// <param name="redraw">描画を再開する場合は true, それ以外は false を返却する。</param>
        private void SetRedrawNotifications(bool redraw)
        {
            SendMessage(ScrollablePanel.Handle, WM_SETREDRAW, redraw, 0);
        }

        /// <summary>
        /// マージン用パネルを取り除き、通知を追加する。
        /// 正確なポジションとサイズは、<see cref="RedrawNotificationItems"/> メソッドで調整される。
        /// </summary>
        /// <param name="notification">追加する通知情報。</param>
        private void AddNotificatonCore(NotificationInfo notification)
        {
            List<Control> additionalControls = new List<Control>();

            // マージン用パネルを取り除く
            if (ScrollablePanel.Controls
                .OfType<Panel>()
                .Any(x => x == _lastMarginPanel))
            {
                RemoveLastMargin();
            }

            if (GetNotificationCount() > 0)
            {
                additionalControls.Add(NewSeparator());
            }

            // NOTE: RedrawNotificationItems で縦スクロールバーの出現想定を行うために、一時的に1行で表現できた場合の高さを求める。
            var textWidth = int.MaxValue;
            var textSize = TextRenderer.MeasureText(notification.Text, NotificationFont,
                                                  new Size(textWidth, 0),
                                                  TextFormatFlags.WordBreak);

            var notificatonLabel = new Label()
            {
                Left = 0,
                Top = 0,
                Width = 0,
                Height = textSize.Height,
                Text = notification.Text,
                Font = NotificationFont,
                ForeColor = NotificationForeColor,
                Cursor = Cursors.Hand,
                Tag = notification,
                AutoSize = false
            };

            notificatonLabel.Click += (s, e) =>
            {
                var clickedItem = (NotificationInfo)((Control)s).Tag;
                clickedItem.Selected?.Invoke();
            };

            notificatonLabel.MouseEnter += (s, e) =>
            {
                ((Control)s).ForeColor = NotificationHoverForeColor;
            };

            notificatonLabel.MouseLeave += (s, e) =>
            {
                ((Control)s).ForeColor = NotificationForeColor;
            };

            additionalControls.Add(notificatonLabel);

            ScrollablePanel.Controls.AddRange(additionalControls.ToArray());
            notification.Registered?.Invoke();
        }

        /// <summary>
        /// セパレーターを追加する。
        /// 正確なポジションと幅は、<see cref="RedrawNotificationItems"/> メソッドで調整される。
        /// </summary>
        private Panel NewSeparator()
        {
            var separatorPanel = new Panel()
            {
                Left = 0,
                Top = 0,
                Width = 0,
                Height = 1,
                BackColor = Color.Transparent,
                Tag = "Separator"
            };

            separatorPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(SeparatorColor, 1))
                {
                    pen.DashStyle = SeparatorDashStyle;
                    e.Graphics.DrawLine(pen, 0, 0, separatorPanel.Width, 0);
                }
            };

            return separatorPanel;
        }

        /// <summary>
        /// 最後の通知に対するマージンを削除する。
        /// </summary>
        private void RemoveLastMargin()
        {
            ScrollablePanel.Controls.Remove(_lastMarginPanel);
        }

        /// <summary>
        /// 最後の通知に対するマージンを追加する。
        /// 正確なポジションと幅は、<see cref="RedrawNotificationItems"/> メソッドで調整される。
        /// </summary>
        private void AddLastMargin()
        {
            ScrollablePanel.Controls.Add(_lastMarginPanel);
        }

        /// <summary>
        /// 通知ラベルやセパレーターの表示X位置を求める。
        /// </summary>
        /// <returns></returns>
        private int GetDrawLeftPosition()
        {
            return -ScrollablePanel.HorizontalScroll.Value;
        }

        /// <summary>
        /// 通知パネル領域を、枠の太さに合わせた位置とサイズに調整する。
        /// </summary>
        private void AdjustScrollablePanelArea()
        {
            ScrollablePanel.Location = new Point(BorderWidth, BorderWidth);
            ScrollablePanel.Size = new Size(FramePanel.Width - (BorderWidth * 2), FramePanel.Height - (BorderWidth * 2));
        }

        /// <summary>
        /// フレームの色を設定し、必要な場合に通知パネルを描画し直す。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FramePanel_Paint(object sender, PaintEventArgs e)
        {
            var beforeSize = new Size(ScrollablePanel.Size.Width, ScrollablePanel.Size.Height);
            AdjustScrollablePanelArea();

            float offset = BorderWidth / 2.0f;
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                pen.DashStyle = BorderDashStyle;
                var rect = new RectangleF(offset, offset, FramePanel.Width - BorderWidth, FramePanel.Height - BorderWidth);
                e.Graphics.DrawRectangles(pen, new[] { rect });
            }

            // 通知パネルのサイズが変更になったときだけ通知の位置やサイズを調整する
            if (ScrollablePanel.Size != beforeSize)
            {
                RedrawNotificationItems();
            }
        }

        /// <summary>
        /// 通知の位置とサイズを再描画する。
        /// </summary>
        private void RedrawNotificationItems()
        {
            if (ScrollablePanel.Controls.Count == 0)
            {
                return;
            }

            foreach (var control in ScrollablePanel.Controls
                .OfType<Control>()
                .Select((Value, Index) => new { Index, Value }))
            {
                if (control.Value is Label)
                {
                    control.Value.Font = NotificationFont;
                    control.Value.ForeColor = NotificationForeColor;

                    // 想定される幅から、必要なサイズを求める
                    var textMaybeWidth = GetDrawNotificationMaybeWidth();
                    var textSize = TextRenderer.MeasureText(control.Value.Text, control.Value.Font,
                                                          new Size(textMaybeWidth, 0),
                                                          TextFormatFlags.WordBreak);
                    control.Value.Width = textSize.Width;
                    control.Value.Height = textSize.Height;
                }

                if (control.Index == 0)
                {
                    control.Value.Top = NotificationElementMargin - ScrollablePanel.VerticalScroll.Value;
                }
                else
                {
                    control.Value.Top = ScrollablePanel.Controls[control.Index - 1].Top + ScrollablePanel.Controls[control.Index - 1].Height + NotificationElementMargin;
                }
            }

            AdjustAllNotificationArea();
        }

        /// <summary>
        /// すべての通知要素の幅を、縦スクロールバーを除いた幅に設定し直す。
        /// また、横スクロールバーの位置を考慮して描画位置を調整する。
        /// </summary>
        private void AdjustAllNotificationArea()
        {
            //// ラベルの最大幅
            //var labelMaxWidth = ScrollablePanel.Controls.OfType<Label>().Max(x => x.Width);

            //int lineWidth = default;
            //if (ScrollablePanel.VerticalScroll.Visible)
            //{
            //    // コントロール全体の幅の方が大きい場合
            //    if (labelMaxWidth < ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth)
            //    {
            //        lineWidth = ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            //    }
            //    else
            //    {
            //        // 横スクロールの表示を行わないときに、縦スクロールを除いた描画領域より大きい場合は狭める
            //        if (HorizontalScrollPolicy == ScrollBarPolicy.Never)
            //        {
            //            lineWidth = ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            //        }
            //        else
            //        {
            //            lineWidth = labelMaxWidth;
            //        }
            //    }
            //}
            //else
            //{
            //    if (labelMaxWidth < ScrollablePanel.Width)
            //    {
            //        lineWidth = ScrollablePanel.Width;
            //    }
            //    else
            //    {
            //        lineWidth = labelMaxWidth;
            //    }
            //}

            var lineWidth = GetDrawNotificationFixedWidth();
            foreach (var control in ScrollablePanel.Controls.OfType<Control>())
            {
                // 横スクロールバーが一番左にないとき、通知の描画位置がずれるため、左位置の調整も行う
                control.Left = GetDrawLeftPosition();
                control.Width = lineWidth;
            }
        }

        /// <summary>
        /// 通知ラベルを描画するための必要と想定される幅を求める。
        /// </summary>
        /// <returns>通知ラベルの想定幅。</returns>
        private int GetDrawNotificationMaybeWidth()
        {
            // 横スクロールが出現する可能性がある場合は最大幅とする
            if (HorizontalScrollPolicy != ScrollBarPolicy.Never)
            {
                return int.MaxValue;
            }

            // 縦スクロールバーが表示されない場合は、スクロールパネルの幅をそのままとする
            if (VerticalScrollPolicy == ScrollBarPolicy.Never)
            {
                return ScrollablePanel.Width;
            }

            // 縦スクロールバーが常に表示される場合は、スクロールパネルの幅から縦スクロールバーの幅を除いた値とする
            if (VerticalScrollPolicy == ScrollBarPolicy.Always)
            {
                return ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            }

            // NOTE: スクロールバーの表示を行うとき、描画領域の高さと、通知情報の高さを求めて、縦スクロールバーが出る状況なら、縦スクロールバーの幅分を除いた幅とする
            int textWidth = default;
            var controlsHeight = ScrollablePanel.Controls.OfType<Control>().Sum(x => x.Height)
                + (ScrollablePanel.Controls.OfType<Control>().Count() * NotificationElementMargin);
            var height = ScrollablePanel.Height;

            // 横スクロールバーが常に表示される場合は、スクロールパネルの高さから横スクロールバーの高さを除いた値とする
            if (HorizontalScrollPolicy == ScrollBarPolicy.Always)
            {
                height -= SystemInformation.HorizontalScrollBarHeight;
            }

            // 通知全体の描画をする高さが足りない場合は縦スクロールバーが表示される想定とする
            if (controlsHeight > height)
            {
                textWidth = ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            }
            else
            {
                textWidth = ScrollablePanel.Width;
            }

            return textWidth;
        }

        /// <summary>
        /// 通知を描画するための必要とされる幅を求める。
        /// 幅は下記にによって決定される。<br/>
        ///  縦スクロールバーが表示されるとき、下記の判断で決定する。<br/>
        ///   1. 通知の幅が <see cref="ScrollablePanel"/> から縦スクロールの幅を除いた幅未満<br/>
        ///      もしくは、<see cref="HorizontalScrollPolicy"/> が <see cref="ScrollBarPolicy.Never"/> のとき、縦スクロールの幅を除いた幅とする。<br/>
        ///    2. それ以外はラベルの最大幅。<br/>
        ///  縦スクロールバーが表示されないとき、下記の判断で決定する。<br/>
        ///   1. 通知の幅が <see cref="ScrollablePanel"/> の幅以上であればラベルの幅とする。<br/>
        ///   2. それ以外は<see cref="ScrollablePanel"/> の幅とする。
        /// </summary>
        /// <returns>通知の確定幅。</returns>
        private int GetDrawNotificationFixedWidth()
        {
            // ラベルの最大幅
            var labelMaxWidth = ScrollablePanel.Controls.OfType<Label>().Max(x => x.Width);

            // 縦スクロールバーが表示されていないときは、ラベルの最大幅もしくはScrollablePanelの幅とする
            // NOTE: 縦スクロールバーが表示されていないということは、幅が足りている、もしくは1行での出力表現である。
            if (!ScrollablePanel.VerticalScroll.Visible)
            {
                if (labelMaxWidth >= ScrollablePanel.Width)
                {
                    return labelMaxWidth;
                }

                return ScrollablePanel.Width;
            }

            // 縦スクロールの幅を除いた幅より小さいときは、縦スクロールの幅を除いた幅とする
            if (labelMaxWidth < ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth)
            {
                return ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            }

            // 縦スクロールの幅を除いた描画領域以上のとき、横スクロールの表示を行わないのであれば、縦スクロールの幅を除いた幅とする
            if (HorizontalScrollPolicy == ScrollBarPolicy.Never)
            {
                return ScrollablePanel.Width - SystemInformation.VerticalScrollBarWidth;
            }

            return labelMaxWidth;
        }
    }
}
