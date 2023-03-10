using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using OxyPlot.Legends;

namespace ImageG
{
    internal class HisviewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PlotModel Mymodel { get; set; }
        public HisviewModel()
        {
            Mymodel = new PlotModel() { Title = "ColorHistogram" };
            Mymodel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, IsAxisVisible = true });
            Mymodel.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, IsAxisVisible = true });
            Mymodel.Legends.Add(new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 10,
                LegendTextColor = OxyColors.LightGray
            });
        }
        public void Draw(int[] y)
        {
            LinearBarSeries linearBarSeries = new LinearBarSeries();
            linearBarSeries.BarWidth = 5;
            for (int i = 0; i < 256; i++)
            {
                linearBarSeries.Points.Add(new DataPoint(i, y[i]));
            }
            Mymodel.Series.Add(linearBarSeries);
            Mymodel.InvalidatePlot(true);
        }
        public void Draw3(int[] r, int[] g, int[] b)
        {
            // RED
            LinearBarSeries linearBarSeriesR = new LinearBarSeries();
            linearBarSeriesR.Title = "Red component";
            linearBarSeriesR.BarWidth = 1;
            linearBarSeriesR.FillColor = OxyColors.LightPink;
            // GREEEN
            LinearBarSeries linearBarSeriesG = new LinearBarSeries();
            linearBarSeriesG.Title = "Green component";
            linearBarSeriesG.BarWidth = 1;
            linearBarSeriesG.FillColor = OxyColors.LightGreen;
            // BLUE
            LinearBarSeries linearBarSeriesB = new LinearBarSeries();
            linearBarSeriesB.Title = "Blue component";
            linearBarSeriesB.BarWidth = 1;
            linearBarSeriesB.FillColor = OxyColors.LightBlue;
            for (int i = 0; i < 256; i++)
            {
                linearBarSeriesR.Points.Add(new DataPoint(i, r[i]));
                linearBarSeriesG.Points.Add(new DataPoint(i, g[i]));
                linearBarSeriesB.Points.Add(new DataPoint(i, b[i]));
            }
            Mymodel.Series.Add(linearBarSeriesR);
            Mymodel.Series.Add(linearBarSeriesG);
            Mymodel.Series.Add(linearBarSeriesB);
            Mymodel.InvalidatePlot(true);
        }
    }
}
