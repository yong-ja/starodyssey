using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WpfTest
{
    /// <summary>
    /// Interaction logic for CountDownWpf.xaml
    /// </summary>
    public partial class CountDownWpf : UserControl
    {
        private readonly Timer countdownTimer;
        private int countdown = 3;

        public event EventHandler<EventArgs> Elapsed;

        public CountDownWpf()
        {
            InitializeComponent();
            countdownTimer = new Timer {Interval = 1000};
            countdownTimer.Elapsed += countdownTimer_Elapsed;
            RenderTransform = new TranslateTransform() {X = 900, Y=300};
        }

        public void Start()
        {
            countdownTimer.Start();
        }

        public void Reset()
        {
            countdown = 3;
            Label.Text = countdown.ToString();
            
        }

        void countdownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            countdown--;
            Dispatcher.BeginInvoke(new Action(delegate { Label.Text = countdown.ToString(); }));

            if (countdown == 0)
            {
                countdownTimer.Stop();
                OnElapsed(EventArgs.Empty);
            }
        }

        protected void OnElapsed(EventArgs e)
        {
            if (Elapsed != null)
                Elapsed(this, e);
        }
    }
}
