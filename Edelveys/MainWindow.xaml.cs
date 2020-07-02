using Edelveys.Core;
using Edelveys.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace Edelveys
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private WordHelper _wordHelper;
		private ImageContainer _imageContainer;
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			_wordHelper = new WordHelper(); 
			_imageContainer = new ImageContainer();
		}

		private void Image_PreviewDrop(object sender, DragEventArgs e)
		{

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var files = (string[])e.Data.GetData(DataFormats.FileDrop);

				if (files == null || files.Length == 0) return;

				if (_imageContainer.ImagePathes.Count >= 4)
				{
					_imageContainer.ImagePathes.Clear();
				}
				foreach (var file in files)
				{
					//if (_imageContainer.ImagePathes.Contains(file)) continue;

					if (ImageHelper.IsImage(file)) _imageContainer.ImagePathes.Add(file);

					if (_imageContainer.ImagePathes.Count >= 4) break;
				}


			}
			SetImages();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var p = new Person { Age = age.Text, FIO = fio.Text,Date=DateTime.Now };
			_wordHelper.Create(p, _imageContainer.ImagePathes);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{

		}

		private void SetImages()
		{
			var converter = new ImageSourceConverter();

			int imageCount = _imageContainer.ImagePathes.Count;
			var defaultImage = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/drag.png");
			switch (imageCount)
			{
				case 1:
					image1.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[0]);
					image2.Source = defaultImage;
					image3.Source = defaultImage;
					image4.Source = defaultImage;
					break;
				case 2:
					image1.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[0]);
					image2.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[1]);
					image3.Source = defaultImage;
					image4.Source = defaultImage;
					break;
				case 3:
					image1.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[0]);
					image2.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[1]);
					image3.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[2]);
					image4.Source = defaultImage;

					break;
				case 4:
					image1.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[0]);
					image2.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[1]);
					image3.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[2]);
					image4.Source = (ImageSource)converter.ConvertFromString(_imageContainer.ImagePathes[3]);
					break;
				default:
					break;
			}

		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{			
			fio.Text = string.Empty;
			age.Text = string.Empty;

			var converter = new ImageSourceConverter();
			var defaultImage = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/drag.png");
			_imageContainer.ImagePathes.Clear();
			image1.Source = defaultImage;
			image2.Source = defaultImage;
			image3.Source = defaultImage;
			image4.Source = defaultImage;
		}

	}
}
