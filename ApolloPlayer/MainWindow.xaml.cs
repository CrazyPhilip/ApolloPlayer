using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using ApolloPlayer.Model;
using ApolloPlayer.ViewModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ApolloPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MusicListViewModel mlvm = new MusicListViewModel();
        //private PlayViewModel pvm = new PlayViewModel();
        private static MusicInfo current_music = new MusicInfo();    //当前音乐信息 
        DispatcherTimer timer = null;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = mlvm;
            //this.DataContext = pvm;
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param file_name="sender"></param>
        /// <param file_name="e"></param>
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            media.Stop();
        }

        /// <summary>
        /// 播放按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_btn_Click(object sender, RoutedEventArgs e)
        {
            if (play_btn.Content.ToString() == "▶")
            {
                Play_Current(mlvm.Current_Index);
                play_btn.Content = "| |";
                media.Play();
            }
            else
            {
                play_btn.Content = "▶";
                ((Storyboard)this.FindResource("Rotateright")).Stop();     //图片停止旋转
                media.Pause();
            }
        }

        private void Play_Current(int index = 0)
        {
            current_music = mlvm.Music_List[index];
            title.Content = current_music.Music_title;   //音乐标题
            artist.Content = current_music.Artist;    //歌手
            album.Content = current_music.Album;    //专辑名
            media.Source = new Uri(current_music.file_path, UriKind.Absolute);   //媒体源

            //专辑封面
            TagLib.File x = TagLib.File.Create(current_music.File_path);
            if (x.Tag.Pictures.Length >= 1)
            {
                byte[] pic = x.Tag.Pictures[0].Data.Data;
                albumpic.Source = ByteArrayToBitmapImage(pic);
            }
            else
            {
                albumpic.Source = new BitmapImage(new Uri(@"Image/Music.png"));
                ((Storyboard)this.FindResource("Rotateright")).Begin();   //图片开始旋转
            }
        }

        /// <summary>
        /// byte[]转source
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
        }

        /// <summary>
        /// 停止按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_btn_Click(object sender, RoutedEventArgs e)
        {
            play_btn.Content = "▶";
            ((Storyboard)this.FindResource("Rotateright")).Stop();     //图片停止旋转
            media.Stop();
        }

        /// <summary>
        /// 上一曲按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pre_btn_Click(object sender, RoutedEventArgs e)
        {
            mlvm.Current_Index = (mlvm.Current_Index + mlvm.Music_List.Count - 1) % mlvm.Music_List.Count;
            Play_Current(mlvm.Current_Index);
        }

        /// <summary>
        /// 下一曲按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            Next_Music();
        }

        private void Next_Music()
        {
            mlvm.Current_Index = (mlvm.Current_Index + 1) % mlvm.Music_List.Count;
            Play_Current(mlvm.Current_Index);
        }

        /// <summary>
        /// 媒体开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            position_slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_tick(object sender, EventArgs e)
        {
            position_slider.Value = media.Position.TotalSeconds;
            time_textblock.Text = TimeSpan.FromSeconds(position_slider.Value).ToString().Substring(0,8);
            //time_textblock.Text = media.Position.Minutes.ToString() + ":" + media.Position.Seconds.ToString() + "/" + current_music.length;
        }
        
        /// <summary>
        /// 控制进度条
        /// </summary>
        /// <param file_name="sender"></param>
        /// <param file_name="e"></param>
        private void Position_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = TimeSpan.FromSeconds(position_slider.Value);
        }

        /// <summary>
        /// 媒体结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            Next_Music();
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if(mlvm.Current_Index > -1 && mlvm.Music_List.Count > 0)
            {
                mlvm.Music_List.RemoveAt(mlvm.Current_Index);
                mlvm.Current_Index = (mlvm.Current_Index - 1) % (mlvm.Music_List.Count + 1);
                play_list.Items.Refresh();
            }
            
        }
        
        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            aboutWindow.Left = this.Left;
            aboutWindow.Top = this.Top;
            aboutWindow.Show();
        }
        
    }
}
