using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace TowerDefender
{
    public class FrameProcessor
    {
        private RGB _color = new RGB(255, 255, 255);
        private int thresh_val = 120;
        private AForge.Point center = new AForge.Point(240, 160);
        private LaserController _controller;
        private BlobCounter bc;

        public FrameProcessor(LaserController controller)
        {
            _controller = controller;
            var min_size_val = 4;

            bc = new BlobCounter();
            bc.FilterBlobs = true;
            bc.MinHeight = min_size_val;
            bc.MinWidth = min_size_val;
            bc.MaxHeight = min_size_val + 50;
            bc.MaxWidth = min_size_val + 50;
        }

        public Bitmap ProcessFrame(Bitmap frame)
        {
            var filter = new EuclideanColorFiltering();
            filter.CenterColor = _color;
            filter.Radius = 20;
            filter.ApplyInPlace(frame);

            //var filter = new Grayscale(0.2125, 0.7154, 0.0721);
            //var thresh = new Threshold(thresh_val);
            //frame = filter.Apply(frame);
            //thresh.ApplyInPlace(frame);

            bc.ProcessImage(frame);
            Rectangle[] rects = bc.GetObjectsRectangles();

            if (rects.Length > 0)
            {
                AForge.Point closest = new AForge.Point(100000, 100000);

                foreach (var r in rects)
                {
                    var p = new AForge.Point(r.Left + r.Width / 2, r.Top + r.Height / 2);

                    var d2 = center.SquaredDistanceTo(closest);
                    var d1 = center.SquaredDistanceTo(p);

                    if (d1 < d2)
                    {
                        closest = p;
                    }
                }

                var closestDistance = center.SquaredDistanceTo(closest);
                var shouldFire = closestDistance < 20;
                var delta = closest - center;
                _controller.Update(delta.X, delta.Y, shouldFire);

                var g = Graphics.FromImage(frame);
                using (Pen p = new Pen(Color.Red))
                {
                    foreach (Rectangle r in rects)
                    {
                        g.DrawRectangle(p, r);
                        g.DrawString("+", new Font("Consolas", 10), Brushes.Red, r.X, r.Y);
                    }
                }
                using (Pen p = new Pen(Color.Green))
                {
                    g.DrawRectangle(p, closest.X - 2, closest.Y - 2, 5, 5);
                }
            }

            return frame;
        }

        public void SetColor(Color color)
        {
            _color = new RGB(color);
        }

        public void SetCenter(int p1, int p2)
        {
            center = new AForge.Point(p1, p2);
        }
    }
}
