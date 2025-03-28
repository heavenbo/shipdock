namespace shipdock
{
    internal static class Program
    {
        //The main entry point for the application.
        //[STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new shipdock());
        }
        //static void Main()
        //{
        //    string filePath = "output.txt";
        //    StreamWriter writer = new StreamWriter(filePath, true);
        //    writer.AutoFlush = true; // 立即写入文件
        //    // 初始化卡尔曼滤波器，初始位置 (0,0)，Q=1，R=10


        //    KalmanFilter kf = new KalmanFilter(0, 0, q: 1, r: 10);

        //    // 模拟输入点 (每个时间点的测量值)
        //    double[,] measurements = {
        //    { 1, 2 }, { 2, 3 }, { 3, 5 }, { 4, 7 }, { 5, 10 }
        //};
        //    string output="Time\tMeasured (X, Y)\tPredicted (X, Y)";
        //    writer.WriteLine(output);

        //    for (int i = 0; i < measurements.GetLength(0); i++)
        //    {
        //        var (predX, predY) = kf.Predict(); // 预测下一步
        //        kf.Update(measurements[i, 0], measurements[i, 1]); // 更新状态

        //        writer.WriteLine($"{i + 1}\t({measurements[i, 0]}, {measurements[i, 1]})\t({predX:F2}, {predY:F2})");
        //    }
        //}
    }
}