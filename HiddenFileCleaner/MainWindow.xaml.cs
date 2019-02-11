using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HiddenFileCleaner
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bind bind;
        public bool Abort = false;

        public MainWindow()
        {
            InitializeComponent();

            // バインディング クラスのインスタンスを作成
            bind = new Bind();

            // ステータスバーの初期表示
            bind.Status.StatusText = Properties.Resources.StatusReady;
            DataContext = bind;
        }

        // フォルダー選択ダイアログ
        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            // FolderBrowserDialog クラスのインスタンスを作成
            // WPF では System.Windows.Forms を参照設定しなければならない
            // C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Windows.Forms.dll
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                // タイトル
                Description = Properties.Resources.DialogSelect,

                // ルートフォルダー（デスクトップ）
                RootFolder = Environment.SpecialFolder.Desktop,

                // 既定のフォルダー（Windows）
                SelectedPath = @"C:\Windows",

                // 新しいフォルダーの作成を許可しない
                ShowNewFolderButton = false
            };

            // ダイアログを表示する
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーのパス名を表示する
                bind.Data.FolderPath = fbd.SelectedPath;
                DataContext = bind;
            }
        }

        // フォルダー検索
        private void SearchFolder(object sender, RoutedEventArgs e)
        {
            // フォルダーパスをテキストボックスから取得
            string path = TextFolderPath.Text;

            // フォルダーが指定されていない
            if (path == "")
            {
                MessageBox.Show(Properties.Resources.FolderEmpty, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // フォルダーが見つからない
            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show(Properties.Resources.FolderNotFound, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // コントロール無効化
            TextFolderPath.IsEnabled = false;
            BtnSelectFolder.IsEnabled = false;
            BtnDeleteFile.IsEnabled = false;

            // 検索 → 中止
            BtnSearchFolder.Visibility = Visibility.Hidden;
            BtnAbortSearchFolder.Visibility = Visibility.Visible;

            // ListView をクリア
            bind.List.Clear();
            DataContext = bind;

            // フォルダーのファイル一覧をサブフォルダーも含めて取得
            // ※アクセスエラー回避版
            IEnumerable<string> files = Directory.SafeEnumerateFilesInAllDirectories(path);
            FileInfo fi;
            long count = 0;
            
            // ファイルを検索する
            foreach (string f in files)
            {
                System.Windows.Forms.Application.DoEvents();

                try
                {
                    // ファイルの情報を取得
                    fi = new FileInfo(f);

                    // リストアップ判定
                    switch (fi.Name.ToLower())
                    {
                        case ".ds_store":   // MenuOption0
                            if (!MenuOption0.IsChecked) continue;
                            break;
                        case ".apdisk":     // MenuOption2
                            if (!MenuOption2.IsChecked) continue;
                            break;
                        case "thumbs.db":   // MenuOption3
                            if (!MenuOption3.IsChecked) continue;
                            break;
                        case "desktop.ini": // MenuOption4
                            if (!MenuOption4.IsChecked) continue;
                            break;
                        default:
                            if (fi.Name.IndexOf("._") > -1 && MenuOption1.IsChecked) break; // MenuOption1
                            else continue;
                    }

                    // ListView に追加
                    bind.List.Add(new ListBind
                    {
                        Select = true,
                        FileName = fi.Name,
                        FolderName = fi.DirectoryName,
                        FileCreateDate = fi.CreationTime.ToString("yyyy/MM/dd hh:mm"),
                        FileModifyDate = fi.LastWriteTime.ToString("yyyy/MM/dd hh:mm"),
                        FileSize = fi.Length.ToString("#,0")
                    });

                    // ファイルカウンタ
                    count++;

                    // ステータスバーを更新
                    bind.Status.StatusText = Properties.Resources.StatusFindProgress + fi.DirectoryName;
                    DataContext = bind;

                    // 中止ボタンが押された場合
                    if (Abort)
                    {
                        // コントロール有効化
                        TextFolderPath.IsEnabled = true;
                        BtnSelectFolder.IsEnabled = true;
                        BtnDeleteFile.IsEnabled = true;

                        // 中止 → 検索
                        BtnSearchFolder.Visibility = Visibility.Visible;
                        BtnAbortSearchFolder.Visibility = Visibility.Hidden;

                        // 検索終了
                        bind.Status.StatusText = Properties.Resources.StatusFindAbort;
                        bind.Data.FolderPath = path;
                        DataContext = bind;

                        Abort = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // 例外はコンソール出力のみ
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            // コントロール有効化
            TextFolderPath.IsEnabled = true;
            BtnSelectFolder.IsEnabled = true;
            BtnDeleteFile.IsEnabled = true;

            // 中止 → 検索
            BtnSearchFolder.Visibility = Visibility.Visible;
            BtnAbortSearchFolder.Visibility = Visibility.Hidden;

            // 検索終了
            bind.Status.StatusText = count.ToString("#,0") + " " + Properties.Resources.StatusFindComplete;
            bind.Data.FolderPath = path;
            DataContext = bind;
        }

        // フォルダー検索の中止
        private void AbortSearchFolder(object sender, RoutedEventArgs e)
        {
            Abort = true;
        }

        // ファイルの削除
        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            // リストに何も表示されていない
            if (bind.List.Count == 0)
            {
                MessageBox.Show(Properties.Resources.ListEmpty, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 処理続行確認
            if (MessageBox.Show(Properties.Resources.DialogDeleteFile, "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            // foreach 用にコレクションを複製
            ObservableCollection<ListBind> temp = new ObservableCollection<ListBind>(bind.List);

            // コントロール無効化
            TextFolderPath.IsEnabled = false;
            BtnSelectFolder.IsEnabled = false;
            BtnSearchFolder.IsEnabled = false;

            // 削除 → 中止
            BtnDeleteFile.Visibility = Visibility.Hidden;
            BtnAbortDeleteFile.Visibility = Visibility.Visible;

            // ステータスバーを更新
            bind.Status.StatusText = Properties.Resources.StatusDelProgress;
            DataContext = bind;

            // ファイルを削除する
            foreach (ListBind item in temp)
            {
                System.Windows.Forms.Application.DoEvents();

                // 選択されているファイルのみ
                if (item.Select)
                {
                    try
                    {
                        // ごみ箱に入れる Microsoft.VisualBasic.FileIO パッケージ
                        // 参照を追加 → アセンブリ → フレームワーク → Microsoft.VisualBasic
                        FileSystem.DeleteFile(item.FolderName + @"\" + item.FileName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);

                        // ごみ箱に入ったらリストから削除する
                        bind.List.Remove(item);

                        // 中止ボタンが押された場合
                        if (Abort)
                        {
                            // コントロール有効化
                            TextFolderPath.IsEnabled = true;
                            BtnSelectFolder.IsEnabled = true;
                            BtnSearchFolder.IsEnabled = true;

                            // 中止 → 削除
                            BtnDeleteFile.Visibility = Visibility.Visible;
                            BtnAbortDeleteFile.Visibility = Visibility.Hidden;

                            // 検索終了
                            bind.Status.StatusText = Properties.Resources.StatusDelAbort;
                            DataContext = bind;

                            Abort = false;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 例外はコンソール出力のみ
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            // コントロール有効化
            TextFolderPath.IsEnabled = true;
            BtnSelectFolder.IsEnabled = true;
            BtnSearchFolder.IsEnabled = true;

            // 中止 → 削除
            BtnDeleteFile.Visibility = Visibility.Visible;
            BtnAbortDeleteFile.Visibility = Visibility.Hidden;

            // 削除終了
            bind.Status.StatusText = Properties.Resources.StatusDelComplete;
            DataContext = bind;
        }

        // ファイル削除の中止
        private void AbortDeleteFile(object sender, RoutedEventArgs e)
        {
            Abort = true;
        }

        // アプリケーション終了
        private void MenuClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // バージョン情報
        private void About(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
