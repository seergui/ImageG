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
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
namespace ImageG
{
    class MainviewModel : INotifyPropertyChanged
    {
        readonly Stopwatch sw = new();
        private double[,] kenel;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ImageSource sourceOrigin;
        public ImageSource SourceOrigin
        {
            get => sourceOrigin;
            set
            {
                sourceOrigin = value;
                OnPropertyChanged(nameof(SourceOrigin));
            }
        }
        private ImageSource sourceProcessed;
        public ImageSource SourceProcessed
        {
            get => sourceProcessed;
            set
            {
                sourceProcessed = value;
                OnPropertyChanged(nameof(SourceProcessed));
            }
        }
        private string filename;
        public string Filename
        {
            get => filename;
            set
            {
                filename = value;
                OnPropertyChanged(nameof(Filename));
            }
        }
        private double processtime;
        public double Processtime
        {
            get => processtime;
            set
            {
                processtime = value;
                OnPropertyChanged(nameof(Processtime));
            }
        }
        private int pValue;
        public int PValue
        {
            get => pValue;
            set
            {
                pValue = value;
                OnPropertyChanged(nameof(PValue));
            }
        }
        private int Time111(double P)
        {
            int time111 = new();
            if (P <= 10.0)
            {
                time111 = 10;
            }
            if (P is > 10.0 and <= 100.0)
            {
                time111 = 20;
            }
            if (P is > 100.0 and <= 300.0)
            {
                time111 = 30;
            }
            if (P is > 300.0 and <= 600.0)
            {
                time111 = 40;
            }
            if (P is > 600.0 and <= 900.0)
            {
                time111 = 50;
            }
            if (P is > 900.0 and <= 1500.0)
            {
                time111 = 60;
            }
            if (P is > 1500.0 and <= 2000.0)
            {
                time111 = 70;
            }
            if (P is > 2000.0 and <= 3000.0)
            {
                time111 = 80;
            }
            if (P is > 3000.0 and <= 5000.0)
            {
                time111 = 90;
            }
            if (P > 5000.0)
            {
                time111 = 100;
            }
            return time111;
        }
        public PlotModel Mymodel { get; set; }
        public MainviewModel()
        {
            Filename = "No file are selected here !";
            Processtime = 0.000;
        }
        public static double[,] Kenel(int k, int type)
        {
            if (type == 1)
            {
                double[,] kenel = new double[,]{{(double)1/k,(double)4/k,(double)7/k,(double)4/k,(double)1/k},
                                                {(double)4/k,(double)16/k,(double)26/k,(double)16/k,(double)4/k},
                                                {(double)7/k,(double)26/k,(double)41/k,(double)26/k,(double)7/k},
                                                {(double)4/k,(double)16/k,(double)26/k,(double)16/k,(double)4/k},
                                                {(double)1/k,(double)4/k,(double)7/k,(double)4/k,(double)1/k}};
                return kenel;
            }
            if (type == 2)
            {
                double[,] kenel = new double[,]{{(double)1/k,(double)1/k,(double)1/k},
                                                {(double)1/k,(double)-12/k,(double)1/k},
                                                {(double)1/k,(double)1/k,(double)1/k}};
                return kenel;
            }
            else
            {
                double[,] kenel = new double[k, k];
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        kenel[i, j] = (double)Math.Round((decimal)1 / (k * k), 5);
                    }
                }
                return kenel;
            }
        }
        public void OpenImage()
        {
            OpenFileDialog open = new()
            {
                Filter = string.Format("照片|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;")
            };
            if (open.ShowDialog() == true)
            {
                string path = open.FileName;
                Filename = path;
                SourceOrigin = new BitmapImage(new Uri(Filename));
            }
        }
        public void SaveImage()
        {
            SourceOrigin = sourceProcessed;
        }
        public void Gray(int type, string Colorspace)
        {
            sw.Restart();
            sw.Start();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.ToArray();
            }
            byte[] R = new byte[(buffer.Length - 54) / 4];
            byte[] G = new byte[(buffer.Length - 54) / 4];
            byte[] B = new byte[(buffer.Length - 54) / 4];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            //// just for test
            //BitmapSource bitmap = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, buffer, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
            //SourceProcessed = bitmap;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    B[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 54];
                    G[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 1 + 54];
                    R[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 2 + 54];
                }
            }
            if (Colorspace == "RGB")
            {
                byte[] gray = ImageProcess.ColorToGray(type, R, G, B);
                BitmapSource bitmap = BitmapSource.Create(width, height, 72, 72, PixelFormats.Gray8, BitmapPalettes.Gray256, gray, ((width * PixelFormats.Gray8.BitsPerPixel) + 7) / 8);
                SourceProcessed = bitmap;
            }
            if (Colorspace == "HSI")
            {
                byte[] HSI = ImageProcess.RGBtoHSI(type, 1, R, G, B);
                BitmapSource bitmapHSI = BitmapSource.Create(width, height, 72, 72, PixelFormats.Gray8, BitmapPalettes.Gray256, HSI, ((width * PixelFormats.Gray8.BitsPerPixel) + 7) / 8);
                SourceProcessed = bitmapHSI;
            }
            if (Colorspace == "ColorI")
            {
                byte[] ColorI = ImageProcess.ColorInversion(R, G, B);
                BitmapSource bitmapCI = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, ColorI, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
                SourceProcessed = bitmapCI;
            }
            if (Colorspace == "HSIB")
            {
                byte[] HISB = ImageProcess.RGBtoHSI(type, 1, R, G, B);
                BitmapSource bitmap = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, HISB, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
                SourceProcessed = bitmap;
            }
            if (Colorspace == "Dark")
            {
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                double x = UD.Userdata.X(double.Parse(UD.Date.Text));
                sw.Start();
                byte[] Dark = ImageProcess.RGBtoHSI(type, x, R, G, B);
                BitmapSource bitmap = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, Dark, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
                SourceProcessed = bitmap;
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Processtime = ts.TotalMilliseconds;
            PValue = Time111(Processtime);
        }
        public void PseudoColor()
        {
            sw.Restart();
            sw.Start();
            //CachedBitmap cachedSource = new CachedBitmap(SourceOrigin as BitmapSource, BitmapCreateOptions.None,BitmapCacheOption.OnLoad);
            //// Create a new BitmapSource using a different format than the original one.
            //FormatConvertedBitmap newFormatSource = new FormatConvertedBitmap();
            //newFormatSource.BeginInit();
            //newFormatSource.Source = cachedSource;
            //newFormatSource.DestinationFormat = PixelFormats.Gray32Float;
            //newFormatSource.EndInit();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.GetBuffer();
                //BitmapEncoder enc = new PngBitmapEncoder();
                //enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                //enc.Save(ms);
                //buffer = ms.GetBuffer();
            }
            byte[] Source = new byte[(buffer.Length)];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Source[(i * width) + j] = buffer[((height - i - 1) * width) + j + 1078];
                }
            }
            //// just for test
            //BitmapSource bitmap = BitmapSource.Create(width, height, 72, 72, PixelFormats.Gray8, BitmapPalettes.Gray256, Source, ((width * PixelFormats.Gray8.BitsPerPixel) + 7) / 8);
            //SourceProcessed = bitmap;
            byte[] PColor = ImageProcess.PseudoColor(Source);
            BitmapSource bitmapPC = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, PColor, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
            SourceProcessed = bitmapPC;
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Processtime = ts.TotalMilliseconds;
            PValue = Time111(Processtime);
        }
        public void His(int type)
        {
            sw.Restart();
            sw.Start();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.ToArray();
            }
            byte[] Source = new byte[(buffer.Length - 54)];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            int size = height * width;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Source[(i * width) + j] = buffer[((height - i - 1) * width) + j + 1078];
                }
            }
            if (type == 1)
            {
                int[] his = new int[256];
                for (int i = 0; i < 256; i++)
                {
                    his[i] = Source.Where(x => x == i).Count();
                }
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Processtime = ts.TotalMilliseconds;
                PValue = Time111(Processtime);
                ColorHistogram CH = new(1, his, null, null, null);
                CH.ShowDialog();
            }
            if (type == 2)
            {
                int[] his = new int[256];
                double[] graydense = new double[256];
                for (int i = 0; i < 256; i++)
                {
                    his[i] = Source.Where(x => x == i).Count();
                    graydense[i] = his[i] * 1.0 / size;
                }
                for (int i = 1; i < 256; i++)
                {
                    graydense[i] = graydense[i] + graydense[i - 1];
                }
                for (int i = 0; i < Source.Length; i++)
                {
                    Source[i] = Source[i] == 0 ? (byte)0 : (byte)(255 * graydense[Source[i]]);
                }
                BitmapSource HisBalance = BitmapSource.Create(width, height, 72, 72, PixelFormats.Gray8, BitmapPalettes.Gray256, Source, ((width * PixelFormats.Gray8.BitsPerPixel) + 7) / 8); //test
                SourceProcessed = HisBalance;
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Processtime = ts.TotalMilliseconds;
                PValue = Time111(Processtime);
            }
        }
        public void His3(int type)
        {
            sw.Restart();
            sw.Start();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.ToArray();
            }
            byte[] R = new byte[(buffer.Length - 54) / 4];
            byte[] G = new byte[(buffer.Length - 54) / 4];
            byte[] B = new byte[(buffer.Length - 54) / 4];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            int size = height * width;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    B[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 54];
                    G[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 1 + 54];
                    R[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 2 + 54];
                }
            }
            if (type == 1)
            {
                int[] hisR = new int[256];
                int[] hisG = new int[256];
                int[] hisB = new int[256];
                for (int i = 0; i < 256; i++)
                {
                    hisR[i] = R.Where(x => x == i).Count();
                    hisG[i] = G.Where(x => x == i).Count();
                    hisB[i] = B.Where(x => x == i).Count();
                }
                ColorHistogram CH = new(2, null, hisR, hisG, hisB);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Processtime = ts.TotalMilliseconds;
                PValue = Time111(Processtime);
                CH.ShowDialog();
            }
            if (type == 2)
            {
                byte[] Color = new byte[3 * R.Length];
                List<byte> Source = new();
                int[] hisR = new int[256];
                int[] hisG = new int[256];
                int[] hisB = new int[256];
                double[] graydenseR = new double[256];
                double[] graydenseG = new double[256];
                double[] graydenseB = new double[256];
                for (int i = 0; i < 256; i++)
                {
                    hisR[i] = R.Where(x => x == i).Count();
                    graydenseR[i] = hisR[i] * 1.0 / size;
                    hisG[i] = G.Where(x => x == i).Count();
                    graydenseG[i] = hisG[i] * 1.0 / size;
                    hisB[i] = B.Where(x => x == i).Count();
                    graydenseB[i] = hisB[i] * 1.0 / size;
                }
                for (int i = 1; i < 256; i++)
                {
                    graydenseR[i] = graydenseR[i] + graydenseR[i - 1];
                    graydenseG[i] = graydenseG[i] + graydenseG[i - 1];
                    graydenseB[i] = graydenseB[i] + graydenseB[i - 1];
                }
                for (int i = 0; i < R.Length; i++)
                {
                    R[i] = R[i] == 0 ? (byte)0 : (byte)(255 * graydenseR[R[i]]);
                    G[i] = G[i] == 0 ? (byte)0 : (byte)(255 * graydenseG[G[i]]);
                    B[i] = R[i] == 0 ? (byte)0 : (byte)(255 * graydenseB[B[i]]);
                    Source.Add(R[i]);
                    Source.Add(G[i]);
                    Source.Add(B[i]);
                }
                Color = Source.ToArray();
                BitmapSource ColorHisB = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, Color, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
                SourceProcessed = ColorHisB;
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Processtime = ts.TotalMilliseconds;
                PValue = Time111(Processtime);
            }
        }
        public void Convolution3(string type)
        {
            sw.Restart();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.ToArray();
            }
            byte[] R = new byte[(buffer.Length - 54) / 4];
            byte[] G = new byte[(buffer.Length - 54) / 4];
            byte[] B = new byte[(buffer.Length - 54) / 4];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            double[,,] InputPicture = new double[3, height, width];//以GRB以及位图的长宽建立整数输入的位图的数组
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    B[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 54];
                    G[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 1 + 54];
                    R[(i * width) + j] = buffer[((((height - i - 1) * width) + j) * 4) + 2 + 54];
                    InputPicture[0, i, j] = R[(i * width) + j];
                    InputPicture[1, i, j] = G[(i * width) + j];
                    InputPicture[2, i, j] = B[(i * width) + j];
                }
            }
            byte[] convolution = new byte[3 * height * width];
            List<byte> Source = new();
            double[] rr = new double[height * width];
            double[] gg = new double[height * width];
            double[] bb = new double[height * width];
            if (type == "Mean")
            {
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                int x = (int)UD.Userdata.X(double.Parse(UD.Date.Text));
                kenel = Kenel(x, 0);
                sw.Start();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < x; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < x; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[0, index, row]);
                                gg[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[1, index, row]);
                                bb[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[2, index, row]);
                            }
                        }
                    }
                }
                byte[] rrr = new byte[R.Length];
                byte[] ggg = new byte[R.Length];
                byte[] bbb = new byte[R.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrr[i] = (byte)rr[i];
                    ggg[i] = (byte)gg[i];
                    bbb[i] = (byte)bb[i];
                    Source.Add(rrr[i]);
                    Source.Add(ggg[i]);
                    Source.Add(bbb[i]);
                }
            }
            if (type == "Gaussian")
            {
                kenel = Kenel(273, 1);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < 5; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < 5; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[0, index, row]);
                                gg[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[1, index, row]);
                                bb[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[2, index, row]);
                            }
                        }
                    }
                }
                byte[] rrr = new byte[R.Length];
                byte[] ggg = new byte[R.Length];
                byte[] bbb = new byte[R.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrr[i] = (byte)rr[i];
                    ggg[i] = (byte)gg[i];
                    bbb[i] = (byte)bb[i];
                    Source.Add(rrr[i]);
                    Source.Add(ggg[i]);
                    Source.Add(bbb[i]);
                }
            }
            if (type == "sharp")
            {
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                int x = (int)UD.Userdata.X(double.Parse(UD.Date.Text));
                kenel = Kenel(x, 2);
                sw.Start();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < 3; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < 3; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[0, index, row]);
                                gg[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[1, index, row]);
                                bb[(i * width) + j] += (byte)(kenel[r, f] * InputPicture[2, index, row]);
                            }
                        }
                    }
                }
                byte[] rrrr = new byte[R.Length];
                byte[] gggg = new byte[R.Length];
                byte[] bbbb = new byte[R.Length];
                double[] rrr = new double[R.Length];
                double[] ggg = new double[R.Length];
                double[] bbb = new double[R.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rr[i] = R[i] - rr[i];
                    gg[i] = G[i] - gg[i];
                    bb[i] = B[i] - bb[i];
                }
                double maxr = rr.Max(); double maxg = gg.Max(); double maxb = bb.Max();
                double minr = rr.Min(); double ming = gg.Min(); double minb = bb.Min();
                for (int i = 0; i < rr.Length; i++)
                {
                    rrr[i] = 255 * ((rr[i] + Math.Abs(minr)) / (maxr + Math.Abs(minr)));
                    ggg[i] = 255 * ((gg[i] + Math.Abs(ming)) / (maxg + Math.Abs(ming)));
                    bbb[i] = 255 * ((bb[i] + Math.Abs(minb)) / (maxb + Math.Abs(minb)));
                    //rrrr[i] = (byte)(255 * (rr[i] / (maxr)));
                    //gggg[i] = (byte)(255 * (gg[i] / (maxg)));
                    //bbbb[i] = (byte)(255 * (bb[i] / (maxb)));
                    rrrr[i] = (byte)rrr[i];
                    gggg[i] = (byte)ggg[i];
                    bbbb[i] = (byte)bbb[i];
                    Source.Add(rrrr[i]);
                    Source.Add(gggg[i]);
                    Source.Add(bbbb[i]);
                }
            }
            if (type == "sharpenI")
            {
                byte[] I = ImageProcess.RGBtoHSI(3, 1, R, G, B);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        InputPicture[0, i, j] = I[(i * width) + j];
                    }
                }
                byte[] H = ImageProcess.RGBtoHSI(1, 1, R, G, B);
                byte[] S = ImageProcess.RGBtoHSI(2, 1, R, G, B);
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                int x = (int)UD.Userdata.X(double.Parse(UD.Date.Text));
                kenel = Kenel(x, 2);
                sw.Start();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < 3; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < 3; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += kenel[r, f] * InputPicture[0, index, row];
                            }
                        }
                    }
                }
                for (int i = 0; i < rr.Length; i++)
                {
                    rr[i] = I[i] - rr[i];
                }
                double rmax = rr.Max(); double rmin = rr.Min();
                double[] rrrr = new double[rr.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrrr[i] = 255 * ((rr[i] + Math.Abs(rmin)) / (rmax + Math.Abs(rmin)));
                    I[i] = (byte)rrrr[i];
                }
                R = ImageProcess.HSItoRGB(1, H, S, I);
                G = ImageProcess.HSItoRGB(2, H, S, I);
                B = ImageProcess.HSItoRGB(3, H, S, I);
                for (int i = 0; i < R.Length; i++)
                {
                    Source.Add(R[i]);
                    Source.Add(G[i]);
                    Source.Add(B[i]);
                }
            }
            convolution = Source.ToArray();
            BitmapSource Smooth = BitmapSource.Create(width, height, 72, 72, PixelFormats.Rgb24, BitmapPalettes.Gray256, convolution, ((width * PixelFormats.Rgb24.BitsPerPixel) + 7) / 8);
            SourceProcessed = Smooth;
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Processtime = ts.TotalMilliseconds;
            PValue = Time111(Processtime);
        }
        public void Convolution(string type)
        {
            sw.Restart();
            byte[] buffer = null;
            using (MemoryStream ms = new())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(SourceOrigin as BitmapSource));
                enc.Save(ms);
                buffer = ms.ToArray();
            }
            byte[] R = new byte[buffer.Length];
            int height = (sourceOrigin as BitmapImage).PixelHeight;
            int width = (sourceOrigin as BitmapImage).PixelWidth;
            double[,] InputPicture = new double[height, width];//以GRB以及位图的长宽建立整数输入的位图的数组
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    R[(i * width) + j] = buffer[((height - i - 1) * width) + j + 1078];
                    InputPicture[i, j] = R[(i * width) + j];

                }
            }
            byte[] convolution = new byte[height * width];
            List<byte> Source = new();
            double[] rr = new double[height * width];
            if (type == "sharp")
            {
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                int x = (int)UD.Userdata.X(double.Parse(UD.Date.Text));
                kenel = Kenel(x, 2);
                sw.Start();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < 3; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < 3; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += kenel[r, f] * InputPicture[index, row];
                            }
                        }
                    }
                }
                for (int i = 0; i < rr.Length; i++)
                {
                    rr[i] = R[i] - rr[i];
                }
                double rmax = rr.Max(); double rmin = rr.Min();
                byte[] rrr = new byte[rr.Length];
                double[] rrrr = new double[rr.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrrr[i] = 255 * ((rr[i] + Math.Abs(rmin)) / (rmax + Math.Abs(rmin)));
                    rrr[i] = (byte)rrrr[i];
                    Source.Add(rrr[i]);
                }
            }
            if (type == "Gaussian")
            {
                kenel = Kenel(273, 1);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < 5; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < 5; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += kenel[r, f] * InputPicture[index, row];
                            }
                        }
                    }
                }
                byte[] rrr = new byte[rr.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrr[i] = (byte)rr[i];
                    Source.Add(rrr[i]);
                }
            }
            if (type == "Mean")
            {
                sw.Stop();
                UserDate UD = new();
                UD.ShowDialog();
                int x = (int)UD.Userdata.X(double.Parse(UD.Date.Text));
                kenel = Kenel(x, 0);
                sw.Start();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //每一个像素计算使用高斯模糊卷积核进行计算
                        for (int r = 0; r < x; r++)//循环卷积核的每一行
                        {
                            for (int f = 0; f < x; f++)//循环卷积核的每一列
                            {
                                //控制与卷积核相乘的元素
                                int row = j - 2 + r;
                                int index = i - 2 + f;
                                //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值
                                row = row < 0 ? 0 : row;
                                index = index < 0 ? 0 : index;
                                row = row >= width ? width - 1 : row;
                                index = index >= height ? height - 1 : index;
                                //输出得到像素的RGB值
                                rr[(i * width) + j] += kenel[r, f] * InputPicture[index, row];
                            }
                        }
                    }
                }
                byte[] rrr = new byte[rr.Length];
                for (int i = 0; i < rr.Length; i++)
                {
                    rrr[i] = (byte)rr[i];
                    Source.Add(rrr[i]);
                }
            }
            convolution = Source.ToArray();
            BitmapSource Smooth = BitmapSource.Create(width, height, 72, 72, PixelFormats.Gray8, BitmapPalettes.Gray256, convolution, ((width * PixelFormats.Gray8.BitsPerPixel) + 7) / 8);
            SourceProcessed = Smooth;
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Processtime = ts.TotalMilliseconds;
            PValue = Time111(Processtime);
        }
        public void SaveAsImage()
        {
            //用户自由选择指定路径保存文件
            SaveFileDialog savedialog = new();
            savedialog.Filter = "Jpeg 图片|*.jpeg|Bmp 图片|*.bmp|Png 图片|*.png|Tiff 图片|*.tiff";
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            savedialog.CheckPathExists = true;//检查目录                          
            if (savedialog.ShowDialog() == true)
            {
                string path = savedialog.FileName;
                string ext = path.Substring(path.LastIndexOf(".") + 1);
                FileStream fs = new(path, FileMode.Create);
                if (ext == "jpeg")
                {
                    JpegBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)SourceProcessed));
                    encoder.Save(fs);
                }
                if (ext == "bmp")
                {
                    BmpBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)SourceProcessed));
                    encoder.Save(fs);
                }
                if(ext == "png")
                {
                    PngBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)SourceProcessed));
                    encoder.Save(fs);
                }
                if (ext == "tiff")
                {
                    TiffBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)SourceProcessed));
                    encoder.Save(fs);
                }
            }
        }
        public void Edge()
        {
            sw.Restart();
            sw.Start();
            BitmapSource m = (BitmapSource)SourceOrigin;
            Bitmap bmp = new(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); // 坑点：选Format32bppRgb将不带透明度
            BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
            ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            m.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);                      //锁住数据流
            Bitmap bitmap_P = ImageProcess.Robert(bmp); //使用Robert算子进行处理
            using MemoryStream stream = new();
            bitmap_P.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度
            stream.Position = 0;
            BitmapImage result = new();
            result.BeginInit();
            // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
            // Force the bitmap to load right now so we can dispose the stream.
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = stream;
            result.EndInit();
            result.Freeze();
            SourceProcessed = result;
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Processtime = ts.TotalMilliseconds;
            PValue = Time111(Processtime);
        }
    }
}
