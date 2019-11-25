using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class SquareVM : INotifyPropertyChanged
    {
        private Square square;

        public SquareVM(Square square)
        {
            this.square = square;
        }

        public double X
        {
            get
            {
                return this.square.X;
            }

            set
            {
                this.square.X = value;
                this.FireOnPropertyChanged();
            }
        }

        public double Y
        {
            get
            {
                return this.square.Y;
            }

            set
            {
                this.square.Y = value;
                this.FireOnPropertyChanged();
            }
        }

        public int Width
        {
            get
            {
                return this.square.Width;
            }

            set
            {
                this.square.Width = value;
                this.FireOnPropertyChanged();
            }
        }

        public int Height
        {
            get
            {
                return this.square.Height;
            }

            set
            {
                this.square.Height = value;
                this.FireOnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void FireOnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
