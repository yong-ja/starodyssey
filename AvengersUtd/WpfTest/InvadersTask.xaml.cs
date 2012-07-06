using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using AvengersUtd.Odyssey.Utils;
using System.Diagnostics;
using Microsoft.Surface.Presentation.Controls;
using AvengersUtd.Odyssey.Geometry;
using Ellipse = System.Windows.Shapes.Ellipse;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for InvadersTask.xaml
    /// </summary>
    public partial class InvadersTask : SurfaceWindow
    {
        const int Width = 1920;
        const int Height = 1080;
        Stopwatch frametimer;
        const double speed = 2E-1;
        TimeSpan previousTime;
        int pointsOnScreen;
        Dictionary<TouchDevice, Ellipse> touchDevices;
        Ellipse leftPoint, rightPoint;
        List<Target> targets;

        Random rand;
        public InvadersTask()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(InvadersTask_Loaded);
            rand = new Random();
            Timer timer = new Timer { Interval = 3000 };
            touchDevices = new Dictionary<TouchDevice, Ellipse>();
            targets = new List<Target>();
            SetupInvaders();
            timer.Elapsed += AnimationCompleted;
            timer.Start();



            foreach (Target target in targets)
                target.Velocity = GetRandomVelocity();
            frametimer = new Stopwatch();


            leftPoint = new Ellipse { Width = 64, Height = 64, Fill = Brushes.Red, Opacity = 0.5 };
            rightPoint = new Ellipse { Width = 64, Height = 64, Fill = Brushes.Red, Opacity = 0.5 };

            this.TouchDown += new EventHandler<TouchEventArgs>(InvadersTask_TouchDown);
            this.TouchMove += new EventHandler<TouchEventArgs>(InvadersTask_TouchMove);
            this.TouchUp += new EventHandler<TouchEventArgs>(InvadersTask_TouchUp);
        }

        void InvadersTask_TouchUp(object sender, TouchEventArgs e)
        {
            Ellipse touchMarker = touchDevices[e.TouchDevice];
            Canvas.Children.Remove(touchMarker);
            touchDevices.Remove(e.TouchDevice);
            pointsOnScreen--;

            foreach (Target target in targets)
                if (target.IsPinnedLeft && ((int)touchMarker.Tag) == 0)
                { target.Velocity = GetRandomVelocity(); target.IsPinnedLeft = false; }
                else if (target.IsPinnedRight && ((int)touchMarker.Tag)==1)
                { target.Velocity = GetRandomVelocity(); target.IsPinnedRight = false; }
        }

        void InvadersTask_TouchMove(object sender, TouchEventArgs e)
        {
            Ellipse touchMarker = touchDevices[e.TouchDevice];
            Point touchPoint = e.GetTouchPoint(this).Position;
            touchMarker.RenderTransform = new TranslateTransform(touchPoint.X - touchMarker.Width / 2, touchPoint.Y - touchMarker.Height / 2);

            foreach (Target target in targets)
                if (target.IntersectsWith(touchPoint))
                {
                    target.Velocity = new Vector();
                    if ((int)touchMarker.Tag == 0)
                        target.IsPinnedLeft= true;
                    else target.IsPinnedRight = true;
                }
        }

        void InvadersTask_TouchDown(object sender, TouchEventArgs e)
        {
            Ellipse touchMarker = new Ellipse { Width = 64, Height = 64, Fill = Brushes.Red, Opacity = 0.5, Tag = touchDevices.Count };
            touchDevices.Add(e.TouchDevice, touchMarker);
            
            Point touchPoint = e.GetTouchPoint(this).Position;
            touchMarker.RenderTransform = new TranslateTransform(touchPoint.X - touchMarker.Width / 2, touchPoint.Y - touchMarker.Height / 2);
            if (Canvas.Children.Contains(touchMarker))
                Canvas.Children.Remove(touchMarker);
            Canvas.Children.Add(touchMarker);


            foreach (Target target in targets)
                if (target.IntersectsWith(touchPoint))
                {
                    target.Velocity = new Vector();
                    if ((int)touchMarker.Tag == 0)
                        target.IsPinnedLeft = true;
                    else target.IsPinnedRight = true;
                }
        }

        Vector GetRandomVelocity()
        {
            //double randX = rand.NextDouble() * 2 - 1;
            //double randY = rand.NextDouble() * 2 - 1;

            int direction = rand.Next(8);
            Vector velocity;
            switch (direction)
            {
                case 0:
                    return velocity = new Vector(0, -speed);
                    
                case 1:
                    return velocity = new Vector(speed, -speed);

                case 2:
                    return velocity = new Vector(speed, 0);

                case 3:
                    return velocity = new Vector(speed, speed);

                case 4:
                    return velocity = new Vector(0, speed);

                case 5:
                    return velocity = new Vector(-speed, speed);
                
                case 6:
                    return velocity = new Vector(-speed, 0);

                case 7:
                    return velocity = new Vector(-speed, -speed);

                default:
                    return new Vector();

            }
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = (e as RenderingEventArgs).RenderingTime;
            TimeSpan delta = elapsedTime - previousTime;
            UpdateImage(Invader1, delta.Milliseconds);
            UpdateImage(Invader2,delta.Milliseconds);
            UpdateImage(Invader3, delta.Milliseconds);
            previousTime = elapsedTime;
        }

        void UpdateImage(Target target, double frameTime)
        {
            Vector velocity = target.Velocity;
            double newX = velocity.X * frameTime;
            double newY = velocity.Y * frameTime;
            TranslateTransform transform = (TranslateTransform)target.RenderTransform;
            transform.X += newX;
            transform.Y += newY;
            
            if (transform.X + target.Width/2 > Width)
                velocity.X = -speed;
            else if (transform.X <= 0)
                velocity.X = speed;

            else if (transform.Y + target.Width/2 > Height)
            { velocity.Y = -speed; }
            else if (transform.Y <= 0)
            { velocity.Y = speed; }

            target.Velocity = velocity;


        }

        void SetupInvaders()
        {
            targets.Add(Invader1);
            targets.Add(Invader2);
            targets.Add(Invader3);
            foreach (Target target in targets)
                target.RenderTransform = new TranslateTransform(rand.Next(Width), rand.Next(Height));
        }

        void InvadersTask_Loaded(object sender, RoutedEventArgs e)
        {
            frametimer.Start();
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
//            CreateAnimations(Invader1);
        }

       

        void AnimationCompleted(object sender, EventArgs e)
        {
            foreach (Target target in targets)
                if (!target.IsPinnedLeft && !target.IsPinnedRight)
                    target.Velocity = GetRandomVelocity();

        }
        
        bool CheckCollisionWith(Image target, Point location)
        {
            TranslateTransform transform = (TranslateTransform)target.RenderTransform;

            Rect rect = new Rect(transform.X, transform.Y, target.Width, target.Height);
            return rect.Contains(location);
        }

        

        
    }
}
