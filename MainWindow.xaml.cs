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
namespace ImageG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OpenImage();
        }
        private void Gray1_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(1, "RGB");
        }
        private void Gray2_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(2, "RGB");
        }
        private void SaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveAsImage();
        }
        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveImage();
        }
        private void Edge_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Edge();
        }
        private void GetR_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(3, "RGB");
        }
        private void GetG_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(4, "RGB");
        }
        private void GetB_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(5, "RGB");
        }
        private void GetH_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(1, "HSI");
        }
        private void GetS_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(2, "HSI");
        }
        private void GetI_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(3, "HSI");
        }
        private void CI_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(1, "ColorI");
        }
        private void PseudoColor_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PseudoColor();
        }
        private void His_Click(object sender, RoutedEventArgs e)
        {
            viewModel.His(1);
        }
        private void ColorHis_Click(object sender, RoutedEventArgs e)
        {
            viewModel.His3(1);
        }
        private void Graybalance_Click(object sender, RoutedEventArgs e)
        {
            viewModel.His(2);
        }
        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(0, "Dark");
        }
        private void RGBbalance_Click(object sender, RoutedEventArgs e)
        {
            viewModel.His3(2);
        }
        private void Hsibalance_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Gray(4, "HSIB");
        }
        private void Smooth3GS_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution3("Gaussian");
        }
        private void Smooth3Mean_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution3("Mean");
        }
        private void SmoothGS_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution("Gaussian");
        }
        private void Sharpen3_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution3("sharp");
        }
        private void Sharpen_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution("sharp");
        }
        private void SmoothMean_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution("Mean");
        }
        private void SharpenI_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Convolution3("sharpenI");
        }
    }
}
