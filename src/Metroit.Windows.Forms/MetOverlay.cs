using Metroit.Windows.Forms.Properties;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metroit.Windows.Forms
{
    /// <summary>
    /// オーバーレイを提供します。
    /// </summary>
    public sealed class MetOverlay : IDisposable
    {
        private double _opacity = 0.3;

        /// <summary>
        /// オーバーレイの不透明度を取得または設定します。
        /// </summary>
        public double Opacity
        {
            get => _opacity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Opacity));
                }
                if (value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Opacity));
                }

                _opacity = value;
            }
        }

        /// <summary>
        /// ローディングアニメーションを表示するかどうかを取得または設定します。
        /// </summary>
        public bool UseAnimation { get; set; } = true;

        /// <summary>
        /// ローディングアニメーションの設定を取得します。
        /// </summary>
        public LoadingAnimationSetting LoadingAnimationSetting { get; set; } = new LoadingAnimationSetting();

        /// <summary>
        /// キャンセル可能かどうかを取得または設定します。
        /// </summary>
        public bool Cancellation { get; set; } = true;

        /// <summary>
        /// オーバーレイが表示されたときに走行します。
        /// </summary>
        public Action OverlayShown { get; set; } = null;

        /// <summary>
        /// 制御が完了したときに走行します。
        /// </summary>
        public Action ProcessCompleted { get; set; } = null;

        /// <summary>
        /// 制御がキャンセルされたときに走行します。
        /// </summary>
        public Action ProcessCancelled { get; set; } = null;

        /// <summary>
        /// UIスレッドの同期コンテキストを取得します。
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; private set; } = null;

        /// <summary>
        /// オーバーレイを表示させるコントロール。
        /// </summary>
        private Control _onControl;

        /// <summary>
        /// オーバーレイを表示させるコントロールが配置されているフォーム。
        /// </summary>
        private Control _parentForm;

        /// <summary>
        /// オーバーレイを表示中に行う制御。
        /// </summary>
        private Func<CancellationToken, bool> _process = null;

        /// <summary>
        /// WebView。
        /// </summary>
        private WebView2 _webView = null;

        /// <summary>
        /// オーバーレイが表示されているかどうか。
        /// </summary>
        private bool _isOverlayShowed = false;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MetOverlay()
        {

        }

        /// <summary>
        /// オーバーレイを表示します。
        /// </summary>
        /// <param name="onControl">オーバーレイを表示するコントロール。</param>
        /// <param name="process">オーバーレイを表示中に行う制御。</param>
        /// <exception cref="ArgumentNullException"><paramref name="onControl"/> が null です。</exception>
        /// <exception cref="ArgumentException"><paramref name="onControl"/> が オーバーレイを表示可能なコントロールではありません。</exception>
        /// <exception cref="InvalidOperationException">オーバーレイが表示済みか、<paramref name="onControl"/> に親フォームが存在しません。</exception>
        public async void Show(Control onControl, Func<CancellationToken, bool> process = null)
        {
            if (_isOverlayShowed)
            {
                throw new InvalidOperationException(ExceptionResources.GetString("OverlayAlreadyDisplayed"));
            }

            SynchronizationContext = SynchronizationContext.Current;

            if (onControl == null)
            {
                throw new ArgumentNullException(nameof(onControl));
            }

            if (onControl is FlowLayoutPanel)
            {
                throw new ArgumentException(string.Format(ExceptionResources.GetString("CannotUseOverlayControl"), nameof(FlowLayoutPanel)), nameof(onControl));
            }
            if (onControl is TableLayoutPanel)
            {
                throw new ArgumentException(string.Format(ExceptionResources.GetString("CannotUseOverlayControl"), nameof(TableLayoutPanel)), nameof(onControl));
            }
            if (onControl is SplitContainer)
            {
                throw new ArgumentException(string.Format(ExceptionResources.GetString("CannotUseOverlayControl"), nameof(SplitContainer)), nameof(onControl));
            }

            var parentForm = onControl.FindForm();
            if (parentForm == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionResources.GetString("UnableFindParentForm"), nameof(onControl)));
            }

            _onControl = onControl;
            _parentForm = parentForm;
            _process = process;

            _onControl.Enter += OnControlEnter;
            _parentForm.Resize += ParentFormResize;
            var webView = CreateWebView();

            await webView.EnsureCoreWebView2Async();
            NavigateOverlay();
            _manualCancel = false;
            _isOverlayShowed = true;
        }

        /// <summary>
        /// Shift+Tab によって、OnControl 内部の何かにフォーカスがあたりそうになったら、強制的に WebView にフォーカスする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnControlEnter(object sender, EventArgs e)
        {
            if (!_isOverlayShowed)
            {
                return;
            }

            _webView.Focus();
        }

        /// <summary>
        /// フォームのサイズ変更に伴って、フォームのリフレッシュが必要かどうか。
        /// </summary>
        private bool _needRefreshByFormResize = false;

        /// <summary>
        /// サイズ変更に伴う描画領域の見え隠れによって、WebView の描画が行われなくなるので、強制的に再描画する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentFormResize(object sender, EventArgs e)
        {
            if (IsPanelFullyVisible(_onControl))
            {
                if (_needRefreshByFormResize)
                {
                    _parentForm.Refresh();
                    _needRefreshByFormResize = false;
                }
            }
            else
            {
                // NOTE: _onControl が一部隠れてる間はリフレッシュし直すのと、完全に表示された時のリフレッシュ用フラグは立てる
                _parentForm.Refresh();
                _needRefreshByFormResize = true;
            }
        }

        /// <summary>
        /// コントロールがフォームの描画領域全体に含まれるかどうかを取得する。
        /// </summary>
        /// <param name="control">検査を行うコントロール。</param>
        /// <returns>フォームの描画領域全体に含まれる場合は true, それ以外は false。</returns>
        private bool IsPanelFullyVisible(Control control)
        {
            var controlRectInForm = control.RectangleToScreen(control.ClientRectangle);
            var formClientRectInScreen = _parentForm.RectangleToScreen(_parentForm.ClientRectangle);

            return formClientRectInScreen.Contains(controlRectInForm);
        }

        /// <summary>
        /// WebView を生成する。
        /// </summary>
        /// <returns>WebView2 オブジェクト。</returns>
        private WebView2 CreateWebView()
        {
            if (_webView != null)
            {
                return _webView;
            }

            _webView = new WebView2
            {
                DefaultBackgroundColor = Color.Transparent,
                // ドロップを受け付けない
                AllowExternalDrop = false,
                Enabled = true,
                TabIndex = 0,
                TabStop = true,
                Dock = DockStyle.Fill
            };

            _webView.CoreWebView2InitializationCompleted += CoreWebView2InitializationCompleted;

            return _webView;
        }

        /// <summary>
        /// WebView が初期化されたとき、CoreWebView2 の設定を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            // F5などのブラウザ操作キーを受け付けない
            _webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            // コンテキストメニューを受け付けない
            _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            // ダイアログ表示を受け付けない
            _webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            // 開発者ツールを受け付けない
            _webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
            // ズーム操作を受け付けない
            _webView.CoreWebView2.Settings.IsZoomControlEnabled = false;

            _webView.CoreWebView2.WebMessageReceived += CoreWebView2WebMessageReceived;
            _webView.CoreWebView2.NavigationCompleted += CoreWebView2NavigationCompleted;
        }

        /// <summary>
        /// キャンセルトークンソース。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// オーバーレイが表示されたとき、非同期処理を実施する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CoreWebView2NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            _onControl.Controls.Add(_webView);
            _webView.BringToFront();

            if (_onControl is Form)
            {
                _webView.Focus();
            }

            await Task.Run(() =>
            {
                _cancellationTokenSource = new CancellationTokenSource();
                if (_process != null)
                {
                    if (!_process.Invoke(_cancellationTokenSource.Token))
                    {
                        _cancellationTokenSource.Cancel();
                    }
                }
            });

            SynchronizationContext.Post(_ => FinalizeProcess(), null);
        }

        /// <summary>
        /// WebView に表示されているページを操作したときの制御を行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoreWebView2WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var message = e.TryGetWebMessageAsString();

            // Tab が押されたとき、オーバーレイが表示されているコントロールの次のタブオーダーのコントロールへ遷移させる
            if (message == "Tab")
            {
                if (_onControl is Form)
                {
                    return;
                }

                var nextControl = GetNextControl();
                if (nextControl != null)
                {
                    nextControl.Focus();
                }
            }

            // Shift+Tab が押されたとき、オーバーレイが表示されているコントロールの前のタブオーダーのコントロールへ遷移させる
            if (message == "ShiftTab")
            {
                if (_onControl is Form)
                {
                    return;
                }

                _parentForm.SelectNextControl(_onControl, false, true, true, true);
            }

            // Esc が押されたとき、オーバーレイをキャンセルする
            if (message == "Escape")
            {
                if (Cancellation)
                {
                    _manualCancel = true;
                    Cancel();
                }
            }
        }

        /// <summary>
        /// オーバーレイを表示しているコントロールの次のタブオーダーのコントロールを取得する。
        /// </summary>
        /// <returns>オーバーレイを表示しているコントロールの次のタブオーダーのコントロール。</returns>
        private Control GetNextControl()
        {
            Control currentControl = _webView;
            Control nextControl = null;
            do
            {
                nextControl = _parentForm.GetNextControl(currentControl, true);
                currentControl = nextControl;
            }
            while (nextControl != null && _onControl.Contains(nextControl));

            return nextControl;
        }

        /// <summary>
        /// 非同期処理の終了を処理する。
        /// </summary>
        private void FinalizeProcess()
        {
            Hide();

            var e = new EventArgs();
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                ProcessCancelled?.Invoke();
            }
            else
            {
                ProcessCompleted?.Invoke();
            }
        }

        /// <summary>
        /// 手動でキャンセルしかたどうか。
        /// </summary>
        private bool _manualCancel = false;

        /// <summary>
        /// オーバーレイを非表示にする。
        /// </summary>
        private void Hide()
        {
            _isOverlayShowed = false;

            if (!_onControl.Controls.Contains(_webView))
            {
                return;
            }

            _onControl.Enter -= OnControlEnter;
            _parentForm.Resize -= ParentFormResize;

            // NOTE: 手動でキャンセルされたとき、オーバーレイ内に含まれる最初のコントロールへフォーカスする
            if (_manualCancel)
            {
                _parentForm.SelectNextControl(_webView, true, true, true, true);
            }
            _onControl.Controls.Remove(_webView);
        }

        /// <summary>
        /// オーバーレイにナビゲーションする。
        /// </summary>
        private void NavigateOverlay()
        {
            string overlayHtml = default;

            if (UseAnimation)
            {
                switch (LoadingAnimationSetting.Image)
                {
                    case LoadingAnimationImage.DualRing:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.DualRing);
                        break;

                    case LoadingAnimationImage.Eclipse:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.Eclipse);
                        break;

                    case LoadingAnimationImage.Ripple:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.Ripple);
                        break;

                    case LoadingAnimationImage.Rolling:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.Rolling);
                        break;

                    case LoadingAnimationImage.Spin:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.Spin);
                        break;

                    case LoadingAnimationImage.Spinner:
                        overlayHtml = CreateAnimationOverlayHtml(OverlayResources.Spinner);
                        break;

                    case LoadingAnimationImage.Custom:
                        overlayHtml = CreateAnimationOverlayHtml(LoadingAnimationSetting.CustomImageBytes);
                        break;
                }
            }
            else
            {
                overlayHtml = CreateNotAnimationOverlayHtml();
            }

            _webView.CoreWebView2.NavigateToString(overlayHtml);
            OverlayShown?.Invoke();
        }

        /// <summary>
        /// アニメーションのないオーバーレイhtml文字列を生成する。
        /// </summary>
        /// <returns>オーバーレイhtml文字列。</returns>
        private string CreateNotAnimationOverlayHtml()
        {
            var templateHtml = OverlayResources.Overlay;

            var adaptedHtml = AdaptCssOpecity(templateHtml);
            adaptedHtml = AdaptNotAnimationCssDisplay(adaptedHtml);
            adaptedHtml = AdaptNotAnimationCssAnimationImage(adaptedHtml);

            return adaptedHtml;
        }

        /// <summary>
        /// アニメーションのあるオーバーレイhtml文字列を生成する。
        /// </summary>
        /// <param name="image">アニメーションイメージ。</param>
        /// <returns>オーバーレイhtml文字列。</returns>
        private string CreateAnimationOverlayHtml(byte[] image)
        {
            var templateHtml = OverlayResources.Overlay;

            var adaptedHtml = AdaptCssOpecity(templateHtml);
            adaptedHtml = AdaptAnimationCssDisplay(adaptedHtml);
            adaptedHtml = AdaptAnimationCssAnimationImage(adaptedHtml, image);

            return adaptedHtml;
        }

        /// <summary>
        /// CSSで表現するオーバーレイの不透明度を当てはめる。
        /// </summary>
        /// <param name="html">当てはめ対象となるhtml文字列。</param>
        /// <returns>当てはめたhtml文字列。</returns>
        private string AdaptCssOpecity(string html)
        {
            return html.Replace("%Opacity%", Opacity.ToString());
        }

        /// <summary>
        /// CSSで表現するアニメーションの利用利用なしを当てはめる。
        /// </summary>
        /// <param name="html">当てはめ対象となるhtml文字列。</param>
        /// <returns>当てはめたhtml文字列。</returns>
        private string AdaptNotAnimationCssDisplay(string html)
        {
            return html.Replace("%Display%", "none");
        }

        /// <summary>
        /// CSSで表現するアニメーションの利用利用ありを当てはめる。
        /// </summary>
        /// <param name="html">当てはめ対象となるhtml文字列。</param>
        /// <returns>当てはめたhtml文字列。</returns>
        private string AdaptAnimationCssDisplay(string html)
        {
            return html.Replace("%Display%", "flex");
        }

        /// <summary>
        /// CSSで表現するアニメーションイメージなしを当てはめる。
        /// </summary>
        /// <param name="html">当てはめ対象となるhtml文字列。</param>
        /// <returns>当てはめたhtml文字列。</returns>
        private string AdaptNotAnimationCssAnimationImage(string html)
        {
            return html
                .Replace("%Base64SvgImage%", "")
                .Replace("%BackgroundSize%", "auto");
        }

        /// <summary>
        /// CSSで表現するアニメーションイメージありを当てはめる。
        /// </summary>
        /// <param name="html">当てはめ対象となるhtml文字列。</param>
        /// <param name="image"></param>
        /// <returns>当てはめたhtml文字列。</returns>
        private string AdaptAnimationCssAnimationImage(string html, byte[] image)
        {
            var base64ImageValue = Convert.ToBase64String(image);

            string size = default;
            switch (LoadingAnimationSetting.SizeUnit)
            {
                case LoadingAnimationSizeUnit.Contain:
                    size = LoadingAnimationSizeUnit.Contain.ToString().ToLower();
                    break;

                case LoadingAnimationSizeUnit.Percent:
                    size = $"{LoadingAnimationSetting.Size}%";
                    break;

                case LoadingAnimationSizeUnit.Pixel:
                    size = $"{LoadingAnimationSetting.Size}px";
                    break;
            }

            return html
                .Replace("%Base64SvgImage%", base64ImageValue)
                .Replace("%BackgroundSize%", size);
        }

        /// <summary>
        /// オーバーレイをキャンセルします。
        /// </summary>
        public void Cancel()
        {
            if (_cancellationTokenSource == null)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// オーバーレイを非表示にして、オブジェクトを破棄します。
        /// </summary>
        public void Dispose()
        {
            Cancel();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// <param name="disposing">マネージオブジェクトを破棄するかどうか。</param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Hide();
            _webView.Dispose();
            _webView = null;
        }

        /// <summary>
        /// インスタンスを破棄する。
        /// </summary>
        ~MetOverlay()
        {
            Cancel();
            Dispose(false);
        }
    }
}
