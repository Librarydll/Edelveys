using Edelveys.Core;
using Edelveys.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace Edelveys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ImageSource> _listImg;
        private WordHelper _wordHelper;
        private ImageContainer _imageContainer;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _wordHelper = new WordHelper();
            _imageContainer = new ImageContainer();
            _listImg = new List<ImageSource>();
        }


        private void Image_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            var converter = new ImageSourceConverter();

            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);

                if (files == null || files.Length == 0) return;

                if (_listImg.Count >= 4)
                {
                    _listImg.Clear();
                }
                foreach (var file in files)
                {
                    if (ImageHelper.IsImage(file)) _listImg.Add((ImageSource)converter.ConvertFromString(file.ToString()));

                    if (_listImg.Count >= 4) break;
                }
            }
            SetImages();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> listStr = new List<string>();
            var p = new Person { Age = age.Text, FIO = fio.Text, Date = DateTime.Now };
            _wordHelper.Create(p, _listImg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            fio.Text = string.Empty;
            age.Text = string.Empty;

            var converter = new ImageSourceConverter();
            var defaultImage = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/drag.png");
            _listImg.Clear();
            image1.Source = defaultImage;
            image2.Source = defaultImage;
            image3.Source = defaultImage;
            image4.Source = defaultImage;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _wordHelper.CreateImage(_listImg);
        }

        private void SetImages()
        {
            var converter = new ImageSourceConverter();

            int imageCount = _listImg.Count;
            var defaultImage = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/drag.png");
            switch (imageCount)
            {
                case 1:
                    image1.Source = _listImg[0];
                    image2.Source = defaultImage;
                    image3.Source = defaultImage;
                    image4.Source = defaultImage;
                    break;
                case 2:
                    image1.Source = _listImg[1];
                    image2.Source = _listImg[0];
                    image3.Source = defaultImage;
                    image4.Source = defaultImage;
                    break;
                case 3:
                    image1.Source = _listImg[2];
                    image2.Source = _listImg[1];
                    image3.Source = _listImg[0];
                    image4.Source = defaultImage;

                    break;
                case 4:
                    image1.Source = _listImg[3];
                    image2.Source = _listImg[2];
                    image3.Source = _listImg[1];
                    image4.Source = _listImg[0];
                    break;
                default:
                    break;
            }
        }

        private void SetImagesX(ImageSource ClipboardImg)
        {
            _listImg.Add(ClipboardImg);
            if (_listImg.Count > 4)
            {
                _listImg.RemoveAt(0);
            }
            var converter = new ImageSourceConverter();

            int imageCount = _listImg.Count;
            var defaultImage = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/drag.png");

            switch (imageCount)
            {
                case 1:
                    image1.Source = _listImg[0];
                    image2.Source = defaultImage;
                    image3.Source = defaultImage;
                    image4.Source = defaultImage;
                    break;
                case 2:
                    image1.Source = _listImg[1];
                    image2.Source = _listImg[0];
                    image3.Source = defaultImage;
                    image4.Source = defaultImage;
                    break;
                case 3:
                    image1.Source = _listImg[2];
                    image2.Source = _listImg[1];
                    image3.Source = _listImg[0];
                    image4.Source = defaultImage;
                    break;
                case 4:
                    image1.Source = _listImg[3];
                    image2.Source = _listImg[2];
                    image3.Source = _listImg[1];
                    image4.Source = _listImg[0];
                    break;
                default:
                    break;
            }
        }

        #region HotKey
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //Identificators
        private const int HOTKEY_ID = 9800;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //NONE

        //Visual Key:
        private const uint VK_SNAPSHOT = 0x2C;

        private IntPtr _windowHandle;
        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_SNAPSHOT); //PrintScreen
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkeya = (((int)lParam >> 16) & 0xFFFF);
                            if (vkeya == VK_SNAPSHOT)
                            {
                                WholeScreenCapture();
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        #region Creating Img

        private void WholeScreenCapture()
        {
            BitmapSource bitSource;
            using (var screenBmp = new Bitmap(
               (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight,
               System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(0, 0, 0, 0, screenBmp.Size);
                    bitSource = Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
            SetImagesX(bitSource);
        }

        #endregion Creating Img

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion HotKey
    }
}
