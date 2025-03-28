using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MathNet.Numerics.LinearAlgebra;
namespace shipdock
{
    internal class Trajectory
    {
        private Pen trajectoryPen;
        public List<PointF> realPoint;
        private List<PointF> points;  // 存储轨迹点
        private Brush targetBrush;
        private PointF pbLeftBottom, pbRightTop;
        private KalmanFilter filter;
        public bool Exists = true;
        public Trajectory(Color Pencolor, Color targetColor, PointF PbLeftBottom, PointF PbRightTop, PointF initial, double q, double r)
        {
            points = new List<PointF>();
            realPoint = new List<PointF>();
            trajectoryPen = new Pen(Pencolor, 3);
            targetBrush = new SolidBrush(targetColor);
            pbLeftBottom = PbLeftBottom;
            pbRightTop = PbRightTop;
            filter = new KalmanFilter(initial, q, r);
        }
        // 添加新点并绘制轨迹
        public void AddPoint(PictureBox pb, PointF center, Graphics graphics)
        {
            if (pb.InvokeRequired)
            {
                // 如果在非 UI 线程，使用 Invoke 切换到 UI 线程
                pb.Invoke(new Action<PictureBox, PointF, Graphics>(AddPoint), new object[] { pb, center, graphics });
            }
            else
            {
                PointF nextPoint;
                if (points.Count > 0)
                {
                    filter.Update(center);
                    nextPoint = filter.Predict();
                }
                else
                {
                    nextPoint = center;
                }
                // 检查圆心是否在 PictureBox 的有效区域内
                if (nextPoint.X > pbLeftBottom.X && nextPoint.Y < pbLeftBottom.Y && nextPoint.X < pbRightTop.X && nextPoint.Y > pbRightTop.Y)
                {
                    float radius = 2;
                    // 计算圆的外接矩形
                    RectangleF rect = new RectangleF(
                        nextPoint.X - radius, // 左上角 X
                        nextPoint.Y - radius, // 左上角 Y
                        radius * 2,        // 宽度
                        radius * 2         // 高度
                    );
                    graphics.FillEllipse(targetBrush, rect);
                    if (points.Count > 0)
                    {
                        graphics.DrawLine(trajectoryPen, points[^1], nextPoint); // 画线
                        points.Add(nextPoint);
                    }
                    else
                    {
                        points.Add(center);
                    }
                }
            }
        }
        public PointF getLastPoint()
        {
            return points[^1];
        }
        public static double Distance(PointF p1, PointF p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return dx * dx + dy * dy; // 计算欧几里得距离的平方，避免开方计算，提高性能
        }
    }
}
