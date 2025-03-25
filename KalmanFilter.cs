using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
namespace shipdock
{
    public class KalmanFilter
    {
        private Matrix<double> X;  // 状态向量 [x, y, vx, vy]
        private Matrix<double> P;  // 误差协方差矩阵
        private readonly Matrix<double> F;  // 状态转移矩阵
        private readonly Matrix<double> H;  // 观测矩阵
        private readonly Matrix<double> I;  // 单位矩阵
        private readonly Matrix<double> Q;  // 过程噪声协方差
        private readonly Matrix<double> R;  // 观测噪声协方差
        public KalmanFilter(PointF initia, double q, double r)
        {
            // 状态向量 (初始位置，速度设为 0)
            X = Matrix<double>.Build.DenseOfArray(new double[,] { { initia.X }, { initia.Y }, { 0 }, { 0 } });
            // 误差协方差矩阵（初始化为较大值，表示对初始状态的不确定性）
            P = Matrix<double>.Build.DenseIdentity(4) * 1000;
            // 状态转移矩阵 F (假设 Δt=1，单位时间内的运动)
            F = Matrix<double>.Build.DenseOfArray(new double[,] {
            { 1, 0, 1, 0 },
            { 0, 1, 0, 1 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        });

            // 观测矩阵 H (只测量位置)
            H = Matrix<double>.Build.DenseOfArray(new double[,] {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 }
        });

            // 单位矩阵 I
            I = Matrix<double>.Build.DenseIdentity(4);
            // 过程噪声协方差 Q (可调节参数)
            Q = Matrix<double>.Build.DenseIdentity(4) * q;
            // 观测噪声协方差 R (可调节参数)
            R = Matrix<double>.Build.DenseIdentity(2) * r;
        }
        // 预测下一步的状态
        public PointF Predict()
        {
            // 预测状态 X' = F * X
            X = F * X;
            // 预测误差协方差 P' = F * P * F^T + Q
            P = F * P * F.Transpose() + Q;
            // 返回预测的位置 (x, y)
            return (new PointF((float)X[0, 0], (float)X[1, 0]));
        }
        // 用新测量的点 (x, y) 更新卡尔曼滤波器
        public void Update(PointF measured)
        {
            // 观测值 Z
            var Z = Matrix<double>.Build.DenseOfArray(new double[,] { { measured.X }, { measured.Y } });
            // 计算卡尔曼增益 K = P * H^T * (H * P * H^T + R)^-1
            var S = H * P * H.Transpose() + R;
            var K = P * H.Transpose() * S.Inverse();
            // 计算更新的状态 X = X + K * (Z - H * X)
            X = X + K * (Z - H * X);
            // 更新误差协方差矩阵 P = (I - K * H) * P
            P = (I - K * H) * P;
        }
    }
}
