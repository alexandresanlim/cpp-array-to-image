using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace cpp_array_to_image
{
    class Program
    {
        static void Main()
        {
            //SET IMAGE WIDTH
            int width = 320;

            //SET IMAGE HEIGHT
            int height = 170;

            //SET YOUR ARRAY HERE
            ushort[] input = { 0x1082, 0x1082, 0x1082 };

            ConvertToImage(input, width, height);
        }

        static void ConvertToImage(ushort[] data, int width, int height)
        {
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb565))
            {
                try
                {

                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                    IntPtr ptr = bmpData.Scan0;
                    int bytes = Math.Abs(bmpData.Stride) * height;

                    byte[] byteData = new byte[bytes];
                    for (int i = 0; i < data.Length; i++)
                    {
                        byteData[i * 2] = (byte)(data[i] & 0xFF);
                        byteData[i * 2 + 1] = (byte)((data[i] >> 8) & 0xFF);
                    }

                    Marshal.Copy(byteData, 0, ptr, bytes);
                    bmp.UnlockBits(bmpData);

                    var fileName = $"../../output/{DateTime.Now.ToString("HHmmss")}.png";

                    bmp.Save(fileName, ImageFormat.Png);

                    Console.WriteLine("File created success 🎉 \nCheck the output folder.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }
                finally
                {
                    Console.ReadKey();
                }
            }
        }


    }
}
