using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lab4
{
    public class Fork
    {
        public Fork(Label label)
        {
            IsUse = false;
            WhichPhilosopher = null;
            Label = label;
        }

        public bool IsUse { get; set; }
        public Label Label { get; set; }

        public Philosopher WhichPhilosopher { get; set; }

        public bool Take(Philosopher takenBy)
        {
            lock (this)
            {
                if (!IsUse)
                {
                    IsUse = true;
                    WhichPhilosopher = takenBy;

                    if (Label.Dispatcher.CheckAccess())
                    {
                        WhichPhilosopher.Label.Content = "take\n" + Label.Name;
                        Label.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            //WhichPhilosopher.Label.Content = "take\n" + Label.Name;
                            Label.Visibility = System.Windows.Visibility.Hidden;
                        }));
                    }
                    Thread.Sleep(100);
                    return true;
                }
                else
                {
                    IsUse = true;
                    return false;
                }
            }
        }

        public void Put()
        {
            IsUse = false;
            if (Label.Dispatcher.CheckAccess())
            {
                Label.Visibility = System.Windows.Visibility.Visible;
                WhichPhilosopher.Label.Background = Brushes.Red;
                WhichPhilosopher.Label.Content = "put\n" + Label.Name;


            }
            else
            {
                Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Label.Visibility = System.Windows.Visibility.Visible;
                    /*WhichPhilosopher.Label.Background = Brushes.Red;
                    WhichPhilosopher.Label.Content = "put\n" + Label.Name;*/
                }));
            }
            WhichPhilosopher = null;
        }
    }
}
