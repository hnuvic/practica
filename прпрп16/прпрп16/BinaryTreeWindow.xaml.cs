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
    public partial class BinaryTreeWindow : Window
    {
        private int depthLevel = 0;
        private int frameCounter = 0;
        private Canvas tempCanvas; 

        private double LengthScale = 0.75;
        private double DeltaTheta = Math.PI / 5;

        public BinaryTreeWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                tempCanvas = new Canvas();
                DrawBinaryTree(tempCanvas, 10,
                    new Point(300, 300), 
                    100, 
                    -Math.PI / 2);

                CompositionTarget.Rendering += StartAnimation;
            };
        }

        private void StartAnimation(object sender, EventArgs e)
        {
            frameCounter++;
            if (frameCounter % 60 == 0)
            {
                if (depthLevel < 10)
                {
                    depthLevel++;
                    canvas1.Children.Clear();
                    DrawScaledBinaryTree(canvas1, depthLevel);
                    tbLabel.Text = $"Binary Tree - Depth = {depthLevel}";
                }
                else
                {
                    canvas1.Children.Clear();
                    DrawScaledBinaryTree(canvas1, 10);
                    tbLabel.Text = "Binary Tree - Depth = 10. Finished";
                    CompositionTarget.Rendering -= StartAnimation;
                }
            }
        }

        private void DrawScaledBinaryTree(Canvas canvas, int depth)
        {
            tempCanvas.Children.Clear();

            DrawBinaryTree(tempCanvas, depth,
                new Point(300, 300), 
                100, 
                -Math.PI / 2);

            double minX = double.MaxValue, minY = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue;

            foreach (var child in tempCanvas.Children)
            {
                if (child is Line line)
                {
                    var points = new[] { new Point(line.X1, line.Y1), new Point(line.X2, line.Y2) };
                    foreach (var p in points)
                    {
                        if (p.X < minX) minX = p.X;
                        if (p.Y < minY) minY = p.Y;
                        if (p.X > maxX) maxX = p.X;
                        if (p.Y > maxY) maxY = p.Y;
                    }
                }
            }

            double treeWidth = maxX - minX;
            double treeHeight = maxY - minY;

            double canvasWidth = canvas.ActualWidth - 20; 
            double canvasHeight = canvas.ActualHeight - 20;

            double scaleX = canvasWidth / treeWidth;
            double scaleY = canvasHeight / treeHeight;
            double scale = Math.Min(scaleX, scaleY) * 0.9; 

            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;

            double treeCenterX = (minX + maxX) / 2;
            double treeCenterY = (minY + maxY) / 2;

            foreach (var child in tempCanvas.Children)
            {
                if (child is Line line)
                {
                    var newLine = new Line
                    {
                        Stroke = line.Stroke,
                        StrokeThickness = line.StrokeThickness
                    };

                    newLine.X1 = centerX + (line.X1 - treeCenterX) * scale;
                    newLine.Y1 = centerY + (line.Y1 - treeCenterY) * scale;
                    newLine.X2 = centerX + (line.X2 - treeCenterX) * scale;
                    newLine.Y2 = centerY + (line.Y2 - treeCenterY) * scale;

                    canvas.Children.Add(newLine);
                }
            }
        }

        private void DrawBinaryTree(Canvas canvas, int depth, Point pt, double length, double theta)
        {
            double x1 = pt.X + length * Math.Cos(theta);
            double y1 = pt.Y + length * Math.Sin(theta);
            Line line = new Line
            {
                Stroke = Brushes.GreenYellow,
                X1 = pt.X,
                Y1 = pt.Y,
                X2 = x1,
                Y2 = y1
            };
            canvas.Children.Add(line);

            if (depth > 1)
            {
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * LengthScale, theta + DeltaTheta);
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * LengthScale, theta - DeltaTheta);
            }
        }
    }
}
