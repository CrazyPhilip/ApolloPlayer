using SourceChord.FluentWPF;
using System.IO;
using System.Windows.Forms;

namespace ApolloPlayer
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : AcrylicWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void SelectPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            PathTextBlock.Text = path.SelectedPath;

            string log_path = "./default_path.dat";
            FileStream fs = new FileStream(log_path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(path.SelectedPath);
            sw.Close();
            fs.Close();
        }
    }
}
