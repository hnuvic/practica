using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BlinkingGarlandApp
{
    public partial class MainWindow : Window
    {
        private Button selectedButton = null;
        private Random rand = new Random();
        private Button[] garlandButtons = new Button[6];
        private Color[] colors = { Colors.Red, Colors.Blue, Colors.Green, Colors.Orange, Colors.Purple, Colors.Yellow };
        private DispatcherTimer colorTimer;

        public MainWindow()
        {
            InitializeComponent();
            CreateGarland();
            SetupTimer();
        }

        private void CreateGarland()
        {
            int x = 50;
            for (int i = 0; i < 6; i++)
            {
                Button btn = new Button
                {
                    Content = (i + 1).ToString(),
                    Width = 40,
                    Height = 40
                };

                Canvas.SetLeft(btn, x);
                Canvas.SetTop(btn, 20);

                btn.Click += (s, ev) => selectedButton = btn;

                MainCanvas.Children.Add(btn);
                garlandButtons[i] = btn;
                x += 50;
            }
            UpdateGarlandColors();
        }

        private void UpdateGarlandColors()
        {
            for (int i = 0; i < garlandButtons.Length; i++)
            {
                int colorIndex;
                do
                {
                    colorIndex = rand.Next(colors.Length);
                } while (i > 0 &&
                         colors[colorIndex] == ((SolidColorBrush)garlandButtons[i - 1].Background).Color);

                garlandButtons[i].Background = new SolidColorBrush(colors[colorIndex]);
            }
        }

        private void SetupTimer()
        {
            colorTimer = new DispatcherTimer();
            colorTimer.Interval = TimeSpan.FromSeconds(1);
            colorTimer.Tick += (s, ev) => UpdateGarlandColors(); 
            colorTimer.Start();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(this);
            Button btn = new Button
            {
                Content = "Кнопка",
                Width = 50,
                Height = 30
            };

            Canvas.SetLeft(btn, pos.X);
            Canvas.SetTop(btn, pos.Y);

            btn.Click += (s, ev) => selectedButton = btn;

            MainCanvas.Children.Add(btn);
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                double top = Canvas.GetTop(selectedButton) - 10;
                if (top >= 0) Canvas.SetTop(selectedButton, top);
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                double top = Canvas.GetTop(selectedButton) + 10;
                if (top <= this.ActualHeight - selectedButton.Height - 50)
                    Canvas.SetTop(selectedButton, top);
            }
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                double left = Canvas.GetLeft(selectedButton) - 10;
                if (left >= 0) Canvas.SetLeft(selectedButton, left);
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                double left = Canvas.GetLeft(selectedButton) + 10;
                if (left <= this.ActualWidth - selectedButton.Width)
                    Canvas.SetLeft(selectedButton, left);
            }
        }
    }
}