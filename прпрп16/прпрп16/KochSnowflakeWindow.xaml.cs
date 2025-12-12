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
using System.Windows.Shapes;

namespace прпрп16
{
    public partial class KochSnowflakeWindow : Window
    {
        private Polyline pl;
        private Point snowflakePoint;
        private double snowflakeSize;
        private int depthLevel = 0;
        private int frameCounter = 0;

        private double distanceScale = 1.0 / 3.0;
        private double[] dTheta = { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        public KochSnowflakeWindow()
        {
            InitializeComponent();

            pl = new Polyline
            {
                Stroke = Brushes.Cyan,
                StrokeThickness = 1
            };
            canvas1.Children.Add(pl);

            Loaded += (s, e) =>
            {
                double ySize = 0.8 * canvas1.ActualHeight / (Math.Sqrt(3) * 4 / 3);
                double xSize = 0.8 * canvas1.ActualWidth / 2;
                snowflakeSize = 2 * Math.Min(xSize, ySize);

                CompositionTarget.Rendering += StartAnimation;
            };
        }

        private void StartAnimation(object sender, EventArgs e)
        {
            frameCounter++;
            if (frameCounter % 60 == 0)
            {
                depthLevel++;
                if (depthLevel <= 5)
                {
                    pl.Points.Clear();
                    DrawSnowFlake(canvas1, snowflakeSize, depthLevel);
                    tbLabel.Text = $"Snowflake - Depth = {depthLevel}";
                }
                else
                {
                    tbLabel.Text = "Snowflake - Depth = 5. Finished";
                    CompositionTarget.Rendering -= StartAnimation;
                }
            }
        }

        private void SnowFlakeEdge(Canvas canvas, int depth, double theta, double distance)
        {
            if (depth <= 0)
            {
                snowflakePoint = new Point(
                    snowflakePoint.X + distance * Math.Cos(theta),
                    snowflakePoint.Y + distance * Math.Sin(theta)
                );
                pl.Points.Add(snowflakePoint);
                return;
            }

            distance *= distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += dTheta[j];
                SnowFlakeEdge(canvas, depth - 1, theta, distance);
            }
        }

        private void DrawSnowFlake(Canvas canvas, double length, int depth)
        {
            double xmid = canvas.ActualWidth / 2;
            double ymid = canvas.ActualHeight / 2;

            Point[] pta = new Point[4];
            pta[0] = new Point(xmid, ymid - length * Math.Sqrt(3) / 3);
            pta[1] = new Point(xmid - length / 2, ymid + length * Math.Sqrt(3) / 6);
            pta[2] = new Point(xmid + length / 2, ymid + length * Math.Sqrt(3) / 6);
            pta[3] = pta[0];

            pl.Points.Add(pta[0]);

            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;

                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);

                snowflakePoint = new Point(x1, y1);
                SnowFlakeEdge(canvas, depth, theta, Math.Sqrt(dx * dx + dy * dy));
            }
        }
    }
}
