using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Fork> _forks;
        private List<Philosopher> _philosophers;
        private List<Thread> _threads;

        public MainWindow()
        {
            InitializeComponent();

            _forks = new List<Fork>
            {
                new Fork(Fork1),
                new Fork(Fork2),
                new Fork(Fork3),
                new Fork(Fork4),
                new Fork(Fork5)
            };

            _philosophers = new List<Philosopher>
            {
                new Philosopher(_forks[0],_forks[1],"A", PhilosopherA),
                new Philosopher(_forks[1],_forks[2],"B", PhilosopherB),
                new Philosopher(_forks[2],_forks[3],"C", PhilosopherC),
                new Philosopher(_forks[3],_forks[4],"D", PhilosopherD),
                new Philosopher(_forks[4],_forks[0],"E", PhilosopherE),
            };

        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            _threads = new List<Thread>
            {
                new Thread(new ThreadStart(_philosophers[0].Eat)),
                new Thread(new ThreadStart(_philosophers[1].Eat)),
                new Thread(new ThreadStart(_philosophers[2].Eat)),
                new Thread(new ThreadStart(_philosophers[3].Eat)),
                new Thread(new ThreadStart(_philosophers[4].Eat)),
            };

            foreach (var item in _threads)
            {
                item.Start();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in _threads)
            {
                item.Abort();
            }
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Fork1.Visibility = Visibility.Visible;
            Fork2.Visibility = Visibility.Visible;
            Fork3.Visibility = Visibility.Visible;
            Fork4.Visibility = Visibility.Visible;
            Fork5.Visibility = Visibility.Visible;

            PhilosopherA.Background = Brushes.Gray;
            PhilosopherB.Background = Brushes.Gray;
            PhilosopherC.Background = Brushes.Gray;
            PhilosopherD.Background = Brushes.Gray;
            PhilosopherE.Background = Brushes.Gray;

            PhilosopherA.Content = "A";
            PhilosopherB.Content = "B";
            PhilosopherC.Content = "C";
            PhilosopherD.Content = "D";
            PhilosopherE.Content = "E";


            foreach (var item in _threads)
            {
                item.Abort();
            }
        }
    }
}
