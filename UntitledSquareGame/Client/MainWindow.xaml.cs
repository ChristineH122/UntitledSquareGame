using Client.ViewModel;
using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameVM game;

        public MainWindow()
        {
            this.game = new GameVM();
            this.DataContext = this;
            InitializeComponent();

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            dispatcherTimer.Start();
        }

        public GameVM Game
        {
            get
            {
                return this.game;
            }

            set
            {
                this.game = value;
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;

            if (e.Key == Key.W)
            {
                this.game.ChangeDirection(Direction.Up);
            }

            if (e.Key == Key.S)
            {
                this.game.ChangeDirection(Direction.Down);
            }

            if (e.Key == Key.A)
            {
                this.game.ChangeDirection(Direction.Left);
            }

            if (e.Key == Key.D)
            {
                this.game.ChangeDirection(Direction.Right); 
            }
        }

        private void MyCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                this.game.ReleaseDirection(Direction.Up); 
            }

            if (e.Key == Key.S)
            {
                this.game.ReleaseDirection(Direction.Down);
            }

            if (e.Key == Key.A)
            {
                this.game.ReleaseDirection(Direction.Left);
            }

            if (e.Key == Key.D)
            {
                this.game.ReleaseDirection(Direction.Right);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.game.Move();
        }
    }
}
