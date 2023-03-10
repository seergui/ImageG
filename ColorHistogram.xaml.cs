using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;
namespace ImageG
{
    /// <summary>
    /// ColorHistogram.xaml 的交互逻辑
    /// </summary>
    public partial class ColorHistogram : Window
    {
        public PlotModel Mymodel { get; set; }
        public ColorHistogram(int type, int[] y, int[] r, int[] g, int[] b)
        {
            InitializeComponent();
            if (type == 1)
            {
                Histogram.Draw(y);
            }
            if (type == 2)
            {
                Histogram.Draw3(r, g, b);
            }
        }
    }
}