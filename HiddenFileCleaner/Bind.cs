using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HiddenFileCleaner
{
    // バインディング クラス
    // 画面全体の変更通知を一括して行う
    // 変更通知が一部だけだと他のコントロールが初期化されるため
    public class Bind
    {
        public DataBind Data { get; set; }
        public StatusBind Status { get; set; }
        public ObservableCollection<ListBind> List { get; set; }

        public Bind()
        {
            Data = new DataBind();
            Status = new StatusBind();
            List = new ObservableCollection<ListBind>();
        }
    }

    // データ バインディング クラス
    // 変更を UI に通知するため、INotifyPropertyChanged を利用する
    public class DataBind : INotifyPropertyChanged
    {
        // 変更通知イベントを定義
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // フォルダー選択
        private string folder_path = "";
        public string FolderPath
        {
            get
            {
                return folder_path;
            }

            set
            {
                if (value != folder_path)
                {
                    folder_path = value;

                    // 変更通知イベント
                    NotifyPropertyChanged();
                }
            }
        }
    }

    // ステータス バインディング クラス
    // 変更を UI に通知するため、INotifyPropertyChanged を利用する
    public class StatusBind : INotifyPropertyChanged
    {
        // 変更通知イベントを定義
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ステータスバーの文字列
        private string status_text = "";
        public string StatusText
        {
            get
            {
                return status_text;
            }

            set
            {
                if (value != status_text)
                {
                    status_text = value;

                    // 変更通知イベント
                    NotifyPropertyChanged();
                }
            }
        }
    }

    // リストビュー バインディング クラス
    // 変更通知は ObservableCollection に含まれるため、INotifyPropertyChanged は不要
    public class ListBind
    {
        // リストビュー項目
        public bool Select { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public string FileCreateDate { get; set; }
        public string FileModifyDate { get; set; }
        public string FileSize { get; set; }
    }
}
