using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lab4
{
    public enum PhilosopherState { Eating, Thinking }

    public class Philosopher
    {
        public string Name { get; set; }

        public PhilosopherState State { get; set; }

        public readonly Fork RightFork;
        public readonly Fork LeftFork;

        public Label Label { get; set; }

        private readonly Random r;

        private int _meter = 0;

        private static int counter = 0;

        public Philosopher(Fork rightFork, Fork leftFork, string name, Label label)
        {
            RightFork = rightFork;
            LeftFork = leftFork;
            Name = name;
            State = PhilosopherState.Thinking;
            Label = label;

            r = new Random();
        }
        public void Eat()
        {
            while (true)
            {
                if (counter > 2) continue;

                if (TakeForkInLeftHand())
                {
                    counter++;
                    if (Label.Dispatcher.CheckAccess())
                    {
                        Label.Background = Brushes.Orange;
                        Label.Content = _meter + " take\n" + LeftFork.Label.Name;
                    }
                    else
                    {
                        Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            Label.Background = Brushes.Orange;
                            Label.Content = _meter + " take\n" + LeftFork.Label.Name;
                        }));
                    }
                    if (TakeForkInRightHand())
                    {
                        if (Label.Dispatcher.CheckAccess())
                        {
                            Label.Background = Brushes.Green;
                            Label.Content = _meter + " take\n" + RightFork.Label.Name + "\n" + LeftFork.Label.Name;
                        }
                        else
                        {
                            Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Label.Background = Brushes.Green;
                                Label.Content = _meter + " take\n" + RightFork.Label.Name + "\n" + LeftFork.Label.Name;
                            }));
                        }
                        this.State = PhilosopherState.Eating;

                        Thread.Sleep(r.Next(12000, 14000));
                        _meter++;
                        if (Label.Dispatcher.CheckAccess())
                        {
                            Label.Background = Brushes.Red;
                            Label.Content = _meter + " put\n" + RightFork.Label.Name + "\n" + LeftFork.Label.Name;
                        }
                        else
                        {
                            Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Label.Background = Brushes.Red;
                                Label.Content = _meter + " put\n" + RightFork.Label.Name + "\n" + LeftFork.Label.Name;
                            }));
                        }
                        RightFork.Put();
                        LeftFork.Put();
                        counter--;

                    }
                    else
                    {
                        LeftFork.Put();
                        counter--;
                    }
                }

                Think();
            }
        }

        public void Think()
        {
            if (Label.Dispatcher.CheckAccess())
            {
                Label.Background = Brushes.Red;
            }
            else
            {
                Label.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { Label.Background = Brushes.Red; }));
            }
            State = PhilosopherState.Thinking;

            Thread.Sleep(r.Next(1000, 2000));
        }

        private bool TakeForkInLeftHand()
        {
            return LeftFork.Take(takenBy: this);
        }

        private bool TakeForkInRightHand()
        {
            return RightFork.Take(takenBy: this);
        }
    }
}
