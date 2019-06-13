using ApolloPlayer.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using Shell32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ApolloPlayer.ViewModel
{
    class PlayViewModel : NotificationObject
    {
        /// <summary>
        /// 该文件暂时没用
        /// </summary>
        public DelegateCommand PlayCommand { get; set; }
        public DelegateCommand PreCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }
        private static MusicInfo current_music = new MusicInfo();    //当前音乐信息 
        DispatcherTimer timer = null;

        private List<MusicInfo> music_list;   //播放列表
        public List<MusicInfo> Music_List
        {
            get { return music_list; }
            set
            {
                music_list = value;
                this.RaisePropertyChanged("Music_List");
            }
        }

        private MediaElement media;
        public MediaElement Media
        {
            get { return media; }
            set
            {
                media = value;
                this.RaisePropertyChanged("Media");
            }
        }

        private static int current_index;    //当前音乐索引
        public int Current_Index
        {
            get { return current_index; }
            set
            {
                current_index = value;
                this.RaisePropertyChanged("Current_Index");
            }
        }

        private double maxlength;    //时长，slider长度
        public double Maxlength
        {
            get { return maxlength; }
            set
            {
                maxlength = value;
                this.RaisePropertyChanged("Maxlength");
            }
        }

        private string title_tb;
        public string Title_Tb
        {
            get { return title_tb; }
            set
            {
                title_tb = value;
                this.RaisePropertyChanged("Title_Tb");
            }
        }

        private double position;    //slider位置
        public double Position
        {
            get { return position; }
            set
            {
                position = value;
                this.RaisePropertyChanged("Position");
            }
        }

        private double volume = 20;    //音量
        public double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                this.RaisePropertyChanged("Volume");
            }
        }
        
        private string playbtntext = "▶";   //播放按钮文字
        public string Playbtntext
        {
            get { return playbtntext; }
            set
            {
                playbtntext = value;
                this.RaisePropertyChanged("Playbtntext");
            }
        }

        public PlayViewModel()
        {
            Media = new MediaElement();
            Media.LoadedBehavior = MediaState.Manual;
            Media.MediaOpened += Media_MediaOpened;

            this.PlayCommand = new DelegateCommand(new Action(this.PlayCommandExecute));
            this.PreCommand = new DelegateCommand(new Action(this.PreCommandExecute));
            this.NextCommand = new DelegateCommand(new Action(this.NextCommandExecute));
        }

        /// <summary>
        /// 播放按钮命令
        /// </summary>
        private void PlayCommandExecute()
        {
            if (playbtntext == "▶")
            {
                Play_Current(Current_Index);
                Playbtntext = "| |";
                //((Storyboard)this.FindResource("Rotateright")).Begin();   //图片开始旋转
                Media.Play();
            }
            else
            {
                Playbtntext = "▶";
                //((Storyboard)this.FindResource("Rotateright")).Pause();     //图片暂停旋转
                Media.Pause();
            }
        }

        private void Play_Current(int index = 0)
        {
            current_music = Music_List[index];
            Title_Tb = current_music.Name;
            Media.Source = new Uri(current_music.file_path, UriKind.Absolute);
        }

        /// <summary>
        /// 上一首
        /// </summary>
        private void PreCommandExecute()
        {
            Current_Index = (Current_Index + music_list.Count - 1) % music_list.Count;
            Play_Current(Current_Index);
        }

        /// <summary>
        /// 下一首
        /// </summary>
        private void NextCommandExecute()
        {
            Current_Index = (Current_Index + 1) % music_list.Count;
            Play_Current(Current_Index);
        }

        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            Maxlength = Media.NaturalDuration.TimeSpan.TotalSeconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Position = Media.Position.TotalSeconds;
        }
    }
}
