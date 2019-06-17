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

namespace ApolloPlayer.ViewModel
{
    class MusicListViewModel : NotificationObject
    {
        public DelegateCommand AddMusicCommand { get; set; }
        public DelegateCommand ShuffleCommand { get; set; }

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
            Init();   //初始化，读取默认目录

            this.AddMusicCommand = new DelegateCommand(new Action(this.AddMusicCommandExecute));
            this.ShuffleCommand = new DelegateCommand(new Action(this.ShuffleCommandExecute));
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
            List<MusicInfo> temp_list = new List<MusicInfo>();
            ShellClass shell = new ShellClass();

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

                temp_list.Add(temp);
            }
            Music_List = temp_list;
        }
        
        private void ShuffleCommandExecute()
        {
            Random random = new Random();
            List<MusicInfo> temp_list = new List<MusicInfo>();

            foreach (MusicInfo item in Music_List)
            {
                temp_list.Insert(random.Next(temp_list.Count), item);
            }

            Music_List.Clear();
            Music_List = temp_list;
        }
    }
}
