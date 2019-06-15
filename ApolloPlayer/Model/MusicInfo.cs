using System.ComponentModel;

namespace ApolloPlayer.Model
{
    public class MusicInfo : INotifyPropertyChanged
    {
        public string file_name { get; set; }
        public string music_title { get; set; }
        public string album { get; set; }
        public string length { get; set; }
        public string artist { get; set; }
        public string size { get; set; }
        public string file_path { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return file_name; }
            set
            {
                file_name = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public string Music_title
        {
            get { return music_title; }
            set
            {
                music_title = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Music_title"));
                }
            }
        }

        public string Album
        {
            get { return album; }
            set
            {
                album = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Album"));
                }
            }
        }

        public string Length
        {
            get { return length; }
            set
            {
                length = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Length"));
                }
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Artist"));
                }
            }
        }

        public string Size
        {
            get { return size; }
            set
            {
                size = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Size"));
                }
            }
        }

        public string File_path
        {
            get { return file_path; }
            set
            {
                file_path = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("File_path"));
                }
            }
        }
    }
}
