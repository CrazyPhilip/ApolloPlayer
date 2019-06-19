using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using ApolloPlayer.Model;
using Shell32;
using Microsoft.Win32;
using Microsoft.Practices.Prism.Commands;
using System.IO;
using System.Collections.ObjectModel;

namespace ApolloPlayer.ViewModel
{
    class MusicListViewModel : NotificationObject
    {
        public DelegateCommand AddMusicCommand { get; set; }
        public DelegateCommand ShuffleCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        private ObservableCollection<MusicInfo> music_list;   //播放列表
        public ObservableCollection<MusicInfo> Music_List
        {
            get { return music_list; }
            set
            {
                music_list = value;
                this.RaisePropertyChanged("Music_List");
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

        public MusicListViewModel()
        {
            Music_List = new ObservableCollection<MusicInfo>();
            Init();   //初始化，读取默认目录

            this.AddMusicCommand = new DelegateCommand(new Action(this.AddMusicCommandExecute));
            this.ShuffleCommand = new DelegateCommand(new Action(this.ShuffleCommandExecute));
            this.DeleteCommand = new DelegateCommand(new Action(this.DeleteCommandExecute));
        }
        
        /// <summary>
        /// 初始化，读取默认目录
        /// </summary>
        private void Init()
        {
            string default_path;
            string log_path = "./default_path.dat";

            if (File.Exists(log_path))
            {
                StreamReader sr = new StreamReader(log_path);
                default_path = sr.ReadLine();
            }
            else
            {
                default_path = (Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)).ToString();
            }
            
            string[] files = Directory.GetFiles(default_path, "*.mp3");
            AddMusic(files);
        }

        /// <summary>
        /// 选择音频文件
        /// </summary>
        private void AddMusicCommandExecute()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "请选择文件";
            dialog.Filter = "音频文件(*.mp3)|*.mp3";

            var result = dialog.ShowDialog();
            if (result == true)
            {
                AddMusic(dialog.FileNames);
            }
        }

        /// <summary>
        /// 添加音乐文件信息
        /// </summary>
        /// <param name="files"></param>
        private void AddMusic(string[] files)
        {
            MusicInfo temp;
            //ObservableCollection<MusicInfo> temp_list = new ObservableCollection<MusicInfo>();
            ShellClass shell = new ShellClass();
            
            //Music_List.Clear();
            foreach (string file in files)
            {
                temp = new MusicInfo();
                Folder dir = shell.NameSpace(Path.GetDirectoryName(file));
                FolderItem item = dir.ParseName(Path.GetFileName(file));

                temp.file_name = dir.GetDetailsOf(item, 0);
                temp.music_title = dir.GetDetailsOf(item, 21);
                temp.album = dir.GetDetailsOf(item, 14);
                temp.artist = dir.GetDetailsOf(item, 13);
                temp.length = dir.GetDetailsOf(item, 27);
                temp.size = dir.GetDetailsOf(item, 1);
                temp.file_path = file;

                if (!Music_List.Any(t => t.file_name.Equals(temp.file_name)))
                {
                    Music_List.Add(temp);
                }
                else
                {
                    continue;
                }
                //Music_List.Add(temp);
            }
        }
        
        /// <summary>
        /// 随机按钮事件
        /// </summary>
        private void ShuffleCommandExecute()
        {
            Random random = new Random();
            ObservableCollection<MusicInfo> temp_list = new ObservableCollection<MusicInfo>();

            foreach (MusicInfo item in Music_List)
            {
                temp_list.Insert(random.Next(temp_list.Count), item);
            }

            Music_List.Clear();
            Music_List = temp_list;
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        private void DeleteCommandExecute()
        {
            if (Current_Index > -1 && Music_List.Count > 0)
            {
                Music_List.RemoveAt(Current_Index);
                Current_Index = (Current_Index - 1) % (Music_List.Count + 1);
                //play_list.Items.Refresh();
            }
        }
    }
}
