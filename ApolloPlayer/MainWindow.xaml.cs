using MahApps.Metro.Controls;
using System;
using System.Windows;
using ApolloPlayer.Model;
using ApolloPlayer.ViewModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;

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
                ((Storyboard)this.FindResource("Rotateright")).Begin();   //图片开始旋转
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
            title.Content = current_music.Music_title;
            artist.Content = current_music.Artist;
            album.Content = current_music.Album;
            media.Source = new Uri(current_music.file_path, UriKind.Absolute);
        }

        private void Stop_btn_Click(object sender, RoutedEventArgs e)
        {
            play_btn.Content = "▶";
            ((Storyboard)this.FindResource("Rotateright")).Stop();     //图片停止旋转
            media.Stop();
        }

        private void Pre_btn_Click(object sender, RoutedEventArgs e)
        {
            mlvm.Current_Index = (mlvm.Current_Index + mlvm.Music_List.Count - 1) % mlvm.Music_List.Count;
            Play_Current(mlvm.Current_Index);
        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            Next_Music();
        }

        private void Next_Music()
        {
            mlvm.Current_Index = (mlvm.Current_Index + 1) % mlvm.Music_List.Count;
            Play_Current(mlvm.Current_Index);
        }

        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            position_slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

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

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            Next_Music();
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if(mlvm.Current_Index > -1 && mlvm.Music_List.Count > 0)
            {
                mlvm.Music_List.RemoveAt(mlvm.Current_Index);
                mlvm.Current_Index = (mlvm.Current_Index - 1) % (mlvm.Music_List.Count + 1);
                play_list.Items.Refresh();
            }
            
        }
    }
}
