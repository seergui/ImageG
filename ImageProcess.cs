using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace ImageG
{
    class ImageProcess
    {
        public static byte[] ColorToGray(int type, byte[] R, byte[] G, byte[] B)
        {
            byte[] gray = new byte[R.Length];
            if (type == 1)
            {
                for (int i = 0; i < gray.Length; i++)
                {
                    gray[i] = (byte)((R[i] + G[i] + B[i]) / 3);
                }
            }
            if (type == 2)
            {
                for (int i = 0; i < gray.Length; i++)
                {
                    gray[i] = (byte)((.299 * R[i]) + (.587 * G[i]) + (.114 * B[i]));
                }
            }
            if (type == 3)
            {
                for (int i = 0; i < gray.Length; i++)
                {
                    gray[i] = R[i];
                }
            }
            if (type == 4)
            {
                for (int i = 0; i < gray.Length; i++)
                {
                    gray[i] = G[i];
                }
            }
            if (type == 5)
            {
                for (int i = 0; i < gray.Length; i++)
                {
                    gray[i] = B[i];
                }
            }
            return gray;
        }
        public static byte[] RGBtoHSI(int type, double x, byte[] R, byte[] G, byte[] B)
        {
            int[] his = new int[256];
            double[] graydense = new double[256];
            byte[] HSI = new byte[R.Length];
            double[] sh = new double[HSI.Length];
            double[] ss = new double[HSI.Length];
            double[] si = new double[HSI.Length];
            double[] r = new double[R.Length];
            double[] g = new double[G.Length];
            double[] b = new double[B.Length];
            byte[] H = new byte[HSI.Length];
            byte[] S = new byte[HSI.Length];
            byte[] I = new byte[HSI.Length];
            byte[] RR = new byte[HSI.Length];
            byte[] GG = new byte[HSI.Length];
            byte[] BB = new byte[HSI.Length];
            List<byte> Source = new();
            for (int i = 0; i < R.Length; i++)
            {
                r[i] = R[i] / 255.0;
                g[i] = G[i] / 255.0;
                b[i] = B[i] / 255.0;
                double theta = Math.Acos(0.5 * (r[i] - g[i] + (r[i] - b[i])) /
                Math.Sqrt(((r[i] - g[i]) * (r[i] - g[i])) + ((r[i] - b[i]) * (g[i] - b[i])))) / (2 * Math.PI);
                sh[i] = (b[i] <= g[i]) ? theta : (1 - theta);
                ss[i] = 1 - (3 * Math.Min(Math.Min(r[i], g[i]), b[i]) / (r[i] + g[i] + b[i]));
                si[i] = (r[i] + g[i] + b[i]) * x / 3;
                H[i] = (byte)((255 * sh[i]) + .5);
                S[i] = (byte)((255 * ss[i]) + .5);
                I[i] = (byte)((255 * si[i]) + .5);
            }
            if (type == 1)
            {
                HSI = H;
                return HSI;
            }
            if (type == 2)
            {
                HSI = S;
                return HSI;
            }
            if (type == 3)
            {
                HSI = I;
                return HSI;
            }
            if (type == 4)
            {
                for (int i = 0; i < 256; i++)
                {
                    his[i] = I.Where(x => x == i).Count();
                    graydense[i] = his[i] * 1.0 / I.Length;
                }
                //ColorHistogram CH = new(1, his, null, null, null);
                //CH.ShowDialog();
                for (int i = 1; i < 256; i++)
                {
                    graydense[i] = graydense[i] + graydense[i - 1];
                }
                for (int i = 0; i < I.Length; i++)
                {
                    I[i] = I[i] == 0 ? (byte)0 : (byte)(255.0 * graydense[I[i]]);
                }
                for (int i = 0; i < 256; i++)
                {
                    his[i] = I.Where(x => x == i).Count();
                }
                //CH = new(1, his, null, null, null);
                //CH.ShowDialog();
                RR = HSItoRGB(1, H, S, I);
                GG = HSItoRGB(2, H, S, I);
                BB = HSItoRGB(3, H, S, I);
                for (int i = 0; i < R.Length; i++)
                {
                    Source.Add(RR[i]);
                    Source.Add(GG[i]);
                    Source.Add(BB[i]);
                }
                return Source.ToArray();
            }
            else
            {
                RR = HSItoRGB(1, H, S, I);
                GG = HSItoRGB(2, H, S, I);
                BB = HSItoRGB(3, H, S, I);
                for (int i = 0; i < R.Length; i++)
                {
                    Source.Add(RR[i]);
                    Source.Add(GG[i]);
                    Source.Add(BB[i]);
                }
                return Source.ToArray();
            }
        }
        public static byte[] HSItoRGB(int type,byte[] H, byte[] S, byte[] I)
        {
            byte[] Results = new byte[H.Length];
            double[] sh = new double[H.Length];
            double[] ss = new double[H.Length];
            double[] si = new double[H.Length];
            double[] rr = new double[H.Length];
            double[] gg = new double[H.Length];
            double[] bb = new double[H.Length];
            byte[] RR = new byte[H.Length];
            byte[] GG = new byte[H.Length];
            byte[] BB = new byte[H.Length];
            for (int i = 0; i < H.Length; i++)
            {
                sh[i] = (H[i] - 0.5) / 255.0;
                ss[i] = (S[i] - 0.5) / 255.0;
                si[i] = (I[i] - 0.5) / 255.0;
                sh[i] = sh[i] * 2 * Math.PI;
                if (sh[i] is >= 0 and < (2 * Math.PI / 3))
                {
                    bb[i] = si[i] * (1 - ss[i]);
                    rr[i] = si[i] * (1 + (ss[i] * Math.Cos(sh[i]) / Math.Cos(Math.PI / 3 - sh[i])));
                    gg[i] = (3 * si[i]) - (rr[i] + bb[i]);
                }
                else if (sh[i] is >= (2 * Math.PI / 3) and < (4 * Math.PI / 3))
                {
                    rr[i] = si[i] * (1 - ss[i]);
                    gg[i] = si[i] * (1 + (ss[i] * Math.Cos(sh[i] - (2 * Math.PI / 3)) / Math.Cos(Math.PI - sh[i])));
                    bb[i] = (3 * si[i]) - (rr[i] + gg[i]);
                }
                else   // if (h >= 4 * Math.PI / 3 && h <= 2 * Math.PI)
                {
                    gg[i] = si[i] * (1 - ss[i]);
                    bb[i] = si[i] * (1 + (ss[i] * Math.Cos(sh[i] - (4 * Math.PI / 3)) / Math.Cos((5 * Math.PI / 3) - sh[i])));
                    rr[i] = (3 * si[i]) - (gg[i] + bb[i]);
                }
            }
            double maxr = rr.Max(); double maxg = gg.Max(); double maxb = bb.Max();
            for (int i = 0; i < rr.Length; i++)
            {
                RR[i] = (byte)((255 * rr[i]) + 0.5);
                GG[i] = (byte)((255 * gg[i]) + 0.5);
                BB[i] = (byte)((255 * bb[i]) + 0.5);
                if (maxr > 1)
                    RR[i] = (byte)((255.0 * rr[i] / maxr) + .5);
                if (maxb > 1)
                    GG[i] = (byte)((255.0 * gg[i] / maxg) + .5);
                if (maxg > 1)
                    BB[i] = (byte)((255.0 * bb[i] / maxb) + .5);
            }
            if (type == 1)
            {
                Results = RR;
            }
            if (type == 2)
            {
                Results = GG;
            }
            if (type == 3)
            {
                Results = BB;
            }
            return Results;
        }
        public static byte[] ColorInversion(byte[] R, byte[] G, byte[] B)
        {
            byte[] ColorI = new byte[3 * R.Length];
            List<byte> Source = new();
            byte[] RF = new byte[R.Length];
            byte[] GF = new byte[G.Length];
            byte[] BF = new byte[B.Length];
            for (int i = 0; i < R.Length; i++)
            {
                RF[i] = (byte)(255 - R[i]);
                GF[i] = (byte)(255 - G[i]);
                BF[i] = (byte)(255 - B[i]);
                Source.Add(RF[i]);
                Source.Add(GF[i]);
                Source.Add(BF[i]);
            }
            ColorI = Source.ToArray();
            return ColorI;
        }
        public static byte[] PseudoColor(byte[] Source)
        {
            byte[] PColor = new byte[3 * Source.Length];
            List<byte> Output = new();
            byte[] R = new byte[Source.Length];
            byte[] G = new byte[Source.Length];
            byte[] B = new byte[Source.Length];
            for (int i = 0; i < Source.Length; i++)
            {
                //红色分量
                if (Source[i] < 128)
                {
                    R[i] = 0;
                }
                else if (Source[i] < 192)
                {
                    R[i] = (byte)(255 / 64 * (Source[i] - 128));
                }
                else
                {
                    R[i] = 255;
                }
                //绿色分量
                if (Source[i] < 64)
                {
                    G[i] = (byte)(255 / 64 * Source[i]);
                }
                else if (Source[i] < 192)
                {
                    G[i] = 255;
                }
                else
                {
                    G[i] = (byte)((-255 / 63 * (Source[i] - 192)) + 255);
                }
                //蓝色分量
                if (Source[i] < 64)
                {
                    B[i] = 255;
                }
                else if (Source[i] < 128)
                {
                    B[i] = (byte)(-255 / 63 * (Source[i] - 192) + 255);
                }
                else
                {
                    B[i] = 0;
                }
                Output.Add(R[i]);
                Output.Add(G[i]);
                Output.Add(B[i]);
            }
            PColor = Output.ToArray();
            return PColor;
        }
        private static float Gamma(byte x)
        {
            return x > 0.04045f ? MathF.Pow((x + 0.055f) / 1.055f, 2.4f) : x / 12.92f;
        }
        public static float[] RGBtoLAB(byte[] R, byte[] G, byte[] B)
        {

            float[] LAB = new float[3 * R.Length];
            List<float> Source = new();
            float[] RR = new float[R.Length];
            float[] GG = new float[G.Length];
            float[] BB = new float[B.Length];
            float[] X = new float[R.Length];
            float[] Y = new float[G.Length];
            float[] Z = new float[B.Length];
            float[] FX = new float[R.Length];
            float[] FY = new float[G.Length];
            float[] FZ = new float[B.Length];
            float[] LLL = new float[R.Length];
            float[] AAA = new float[G.Length];
            float[] BBB = new float[B.Length];
            for (int i = 0; i < R.Length; i++)
            {
                BB[i] = Gamma(B[i]);
                GG[i] = Gamma(G[i]);
                RR[i] = Gamma(R[i]);
                X[i] = (0.412453f * RR[i]) + (0.357580f * GG[i]) + (0.180423f * BB[i]);
                Y[i] = (0.212671f * RR[i]) + (0.715160f * GG[i]) + (0.072169f * BB[i]);
                Z[i] = (0.019334f * RR[i]) + (0.119193f * GG[i]) + (0.950227f * BB[i]);
                X[i] /= 0.95047f;
                Y[i] /= 1.0f;
                Z[i] /= 1.08883f;
                FX[i] = X[i] > 0.008856f ? MathF.Pow(X[i], 1.0f / 3.0f) : ((7.787f * X[i]) + 0.137931f);
                FY[i] = Y[i] > 0.008856f ? MathF.Pow(Y[i], 1.0f / 3.0f) : ((7.787f * Y[i]) + 0.137931f);
                FZ[i] = Z[i] > 0.008856f ? MathF.Pow(Z[i], 1.0f / 3.0f) : ((7.787f * Z[i]) + 0.137931f);
                LLL[i] = Y[i] > 0.008856f ? ((116.0f * FY[i]) - 16.0f) : (903.3f * Y[i]);
                AAA[i] = 500f * (FX[i] - FY[i]);
                BBB[i] = 200f * (FY[i] - FZ[i]);
                Source.Add(LLL[i]);
                Source.Add(AAA[i]);
                Source.Add(BBB[i]);
            }
            LAB = Source.ToArray();
            return LAB;
        }
        public static Bitmap Robert(Bitmap a)
        {
            int w = a.Width;
            int h = a.Height;
            Bitmap bitmap = new(w, h, PixelFormat.Format24bppRgb); //将 Bitmap 锁定到系统内存中
            BitmapData oldData = a.LockBits(new Rectangle(0, 0, w, h),
            ImageLockMode.ReadOnly,
            PixelFormat.Format24bppRgb);                              //指定原图的范围、只读、图片的格式
            BitmapData newData = bitmap.LockBits(new Rectangle(0, 0, w, h),
            ImageLockMode.WriteOnly,
            PixelFormat.Format24bppRgb);                             //指定处理后图片的范围、只写、图片的格式
            unsafe
            {
                byte* pIn = (byte*)oldData.Scan0.ToPointer();  //获得位图中像素数据的第一个地址
                byte* pOut = (byte*)newData.Scan0.ToPointer();   //
                byte* p;
                int stride = oldData.Stride;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        //边缘八个点像素不变
                        if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                        {
                            pOut[0] = pIn[0];
                            pOut[1] = pIn[1];
                            pOut[2] = pIn[2];
                        }
                        else
                        {
                            int r0, r5, r6, r7;
                            int g5, g6, g7, g0;
                            int b5, b6, b7, b0;
                            double vR, vG, vB;
                            //右
                            p = pIn + 3;
                            r5 = p[2];
                            g5 = p[1];
                            b5 = p[0];
                            //左下
                            p = pIn + stride - 3;
                            r6 = p[2];
                            g6 = p[1];
                            b6 = p[0];
                            //正下
                            p = pIn + stride;
                            r7 = p[2];
                            g7 = p[1];
                            b7 = p[0];
                            //中心点
                            p = pIn;
                            r0 = p[2];
                            g0 = p[1];
                            b0 = p[0];
                            vR = (double)(Math.Abs(r0 - r5) + Math.Abs(r5 - r7));
                            vG = (double)(Math.Abs(g0 - g5) + Math.Abs(g5 - g7));
                            vB = (double)(Math.Abs(b0 - b5) + Math.Abs(b5 - b7));
                            if (vR > 0)
                            {
                                vR = Math.Min(255, vR);
                            }
                            else
                            {
                                vR = Math.Max(0, vR);
                            }
                            if (vG > 0)
                            {
                                vG = Math.Min(255, vG);
                            }
                            else
                            {
                                vG = Math.Max(0, vG);
                            }
                            if (vB > 0)
                            {
                                vB = Math.Min(255, vB);
                            }
                            else
                            {
                                vB = Math.Max(0, vB);
                            }
                            pOut[0] = (byte)vB;
                            pOut[1] = (byte)vG;
                            pOut[2] = (byte)vR;
                        }
                        pIn += 3;
                        pOut += 3;
                    }
                    pIn += oldData.Stride - (w * 3);
                    pOut += oldData.Stride - (w * 3);
                }
            }
            a.UnlockBits(oldData);
            bitmap.UnlockBits(newData);
            return bitmap;
        }
    }
}
