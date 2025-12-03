namespace Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が行われたプロパティのイベントを処理するメソッドを表します。
    /// </summary>
    /// <param name="sender">イベントのソース。</param>
    /// <param name="e">イベント データを格納している<see cref="PropertyChangedTrackingEventArgs"/>。</param>
    public delegate void PropertyChangedTrackingEventHandler(object sender, PropertyChangedTrackingEventArgs e);
}
