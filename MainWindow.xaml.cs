using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFMediaKit.DirectShow.Controls;

namespace WindowsHunter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] ss = MultimediaUtil.VideoInputNames;
            foreach (string s in ss)
            {
                vce.VideoCaptureSource = s;
            }
            WindowState = WindowState.Minimized;
            t.Elapsed += Elapsed;
            t.Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => {
                Capture();
            });
        }

        readonly Timer t = new Timer(20000);

        private void Capture()
        {
            var bmp = new RenderTargetBitmap((int)vce.ActualWidth, (int)vce.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(vce);
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                var dt = DateTime.Now;
                File.WriteAllBytes($"{dt.Year}{dt.Month}{dt.Day}{dt.Hour}{dt.Minute}{dt.Second}.jpg", ms.ToArray());
            }
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            Capture();
        }
    }
}
