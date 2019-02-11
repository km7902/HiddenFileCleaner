using Microsoft.VisualBasic.ApplicationServices;
using System.Windows;

namespace HiddenFileCleaner
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            // バージョン情報を表示するために必要
            DataContext = new AssemblyInfo(System.Reflection.Assembly.GetExecutingAssembly());
        }

        // ウィンドウを閉じる
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
