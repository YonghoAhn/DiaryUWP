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
using Windows.Graphics.Imaging;
using Microsoft.ProjectOxford.Common.Contract;
using Windows.UI.Popups;

namespace Blessing
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class TakePic : Page
    {
        CameraCaptureUI cc = new CameraCaptureUI();
        StorageFile photo;
        IRandomAccessStream imageStream;
        const string APIKey = "c1535e7f2cb74b998131a4ed71ac36b5";
        EmotionServiceClient emotionserviceclient = new EmotionServiceClient(APIKey);
        Microsoft.ProjectOxford.Emotion.Contract.Emotion[] emotionresult;
        public TakePic()
        {
            this.InitializeComponent();
            cc.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            cc.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
        }

        private async void GetEmotionAsync()
        {
            try
            {
                emotionresult = await emotionserviceclient.RecognizeAsync(imageStream.AsStream());
                if (emotionresult != null)
                {
                    EmotionScores score = emotionresult[0].Scores;
                    string a = "Your Emotions are : \n" +
                        "Happiness: " + (score.Happiness) * 100 + " %" + "\n" +

                        "Sadness: " + (score.Sadness) * 100 + " %" + "\n" +

                        "Surprise: " + (score.Surprise) * 100 + " %" + "\n" +

                        "Neutral: " + (score.Neutral) * 100 + " %" + "\n" +

                        "Anger: " + (score.Anger) * 100 + " %" + "\n" +

                        "Contempt: " + (score.Contempt) * 100 + " %" + "\n" +

                        "Disgust: " + (score.Disgust) * 100 + " %" + "\n" +

                        "Fear: " + (score.Fear) * 100 + " %" + "\n";
                    MessageDialog c = new MessageDialog(a);
                    await c.ShowAsync();
                }
            }
            catch
            {
                //tblEmotion.Text = "Error Returning the Emotions from API";
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            cc.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            try
            {
                photo = await cc.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (photo == null)
                {
                    return;
                }
                else
                {
                    imageStream = await photo.OpenAsync(FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);
                    SoftwareBitmap softwarebitmap = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap softwarebitmapBGRB = SoftwareBitmap.Convert(softwarebitmap,
                    BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    SoftwareBitmapSource bitmapsource = new SoftwareBitmapSource();
                    await bitmapsource.SetBitmapAsync(softwarebitmapBGRB);
                    image.Source = bitmapsource;
                    GetEmotionAsync();
                }
            }
            catch
            {

            }
        }
    }
}
