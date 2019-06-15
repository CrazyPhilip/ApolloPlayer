using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloPlayer.Model
{
    public class Settings : INotifyPropertyChanged
    {
        private string defaultPath { get; set; }
        private double windowTintOpacity { get; set; }

        public string DefaultPath
        {
            get { return defaultPath; }
            set
            {
                defaultPath = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DefaultPath"));
                }
            }
        }

        public double WindowTintOpacity
        {
            get { return windowTintOpacity; }
            set
            {
                windowTintOpacity = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("WindowTintOpacity"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
