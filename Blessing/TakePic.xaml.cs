using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace Blessing
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class TakePic : Page
    {
        public TakePic()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI cc = new CameraCaptureUI();
            cc.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            cc.PhotoSettings.CroppedAspectRatio = new Size(3, 4);
            cc.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            StorageFile sf = await cc.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (sf != null)
            {
                BitmapImage bmp = new BitmapImage();
                IRandomAccessStream rs = await sf.OpenAsync(FileAccessMode.Read);
                bmp.SetSource(rs);
                image.Source = bmp;
            }
        }
    }
}
