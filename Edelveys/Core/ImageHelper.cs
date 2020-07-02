using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Edelveys.Core
{
	public class ImageHelper
	{
		private static int width = 180;
		private static int height = 180;

		private static List<string> imageExtenstion = new List<string>
		{
			"jpg","bmp","gif","png","jpeg"
		};

		public static bool IsImage(string filepath)
		{
			if (!File.Exists(filepath)) return false;


			foreach (var ex in imageExtenstion)
			{
				if (filepath.ToLower().EndsWith(ex)) return true;
			}

			return false;

		}

		public static void CreateAndFitImages(IEnumerable<ImageSource> files)
		{
			DeleteFiles();
			try
			{
				int i = 1;
				foreach (var file in files)
				{
					var outputImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
					var graphics = Graphics.FromImage(outputImage);
					var img = GetBitmap((BitmapSource)file);
					graphics.DrawImage(img, new Rectangle(0, 0, width, height),
					new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
					outputImage.Save(i.ToString()+".jpeg", ImageFormat.Jpeg);
					i++;
				}				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static Bitmap GetBitmap(BitmapSource source)
		{
			Bitmap bmp = new Bitmap(
			  source.PixelWidth,
			  source.PixelHeight,
			  System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			BitmapData data = bmp.LockBits(
			  new Rectangle(System.Drawing.Point.Empty, bmp.Size),
			  ImageLockMode.WriteOnly,
			  System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			source.CopyPixels(
			  Int32Rect.Empty,
			  data.Scan0,
			  data.Height * data.Stride,
			  data.Stride);
			bmp.UnlockBits(data);
			return bmp;
		}

		public static void DeleteFiles()
		{
			for (int i = 1; i <= 4; i++)
			{
				var file = i.ToString() + ".jpeg";
				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
		}
	}
}
