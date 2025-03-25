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
        private List<PointF> points;  // 存储轨迹点
        private Brush targetBrush;
        PointF pbLeftBottom, pbRightTop;
        KalmanFilter filter;
        public Trajectory(Color Pencolor, Color targetColor, PointF PbLeftBottom, PointF PbRightTop, PointF initial, double q, double r)
        {
            points = new List<PointF>();
            trajectoryPen = new Pen(Pencolor, 2);
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
                // 检查圆心是否在 PictureBox 的有效区域内
                if (center.X > pbLeftBottom.X && center.Y < pbLeftBottom.Y && center.X < pbRightTop.X && center.Y > pbRightTop.Y)
                {
                    float radius = 5;
                    // 计算圆的外接矩形
                    RectangleF rect = new RectangleF(
                        center.X - radius, // 左上角 X
                        center.Y - radius, // 左上角 Y
                        radius * 2,        // 宽度
                        radius * 2         // 高度
                    );
                    //// 计算需要刷新的区域
                    //Rectangle updateRect = new Rectangle(
                    //    (int)(rect.X),      // 更新区域的左上角 X
                    //    (int)(rect.Y),      // 更新区域的左上角 Y
                    //    (int)(rect.Width),  // 更新区域的宽度
                    //    (int)(rect.Height)  // 更新区域的高度
                    //);
                    graphics.FillEllipse(targetBrush, rect);
                    if (points.Count > 0)
                    {
                        filter.Update(center);
                        points.Add(filter.Predict());
                        graphics.DrawLine(trajectoryPen, points[^1], center); // 画线
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
        public static float Distance(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return dx * dx + dy * dy; // 计算欧几里得距离的平方，避免开方计算，提高性能
        }
    }
}
