using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using Xamarin.Forms;

using Plugin.Media;
using System.IO;
using System.Threading.Tasks;

namespace TestProject
{
    public partial class PicCamPage : ContentPage
    {

        // Setup, Keys and Paths
        private const string apiKey = "6a80f9c7b80b4c449dc011de3aefec76";
        private string localImagePath = "";
        private ComputerVisionClient computerVision;
        // Specify the features to return
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };
        private static string picDescription = "";




        public PicCamPage()
        {
            InitializeComponent();
            computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(apiKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://westcentralus.api.cognitive.microsoft.com";
        }


        private static async Task AnalyzeLocalAsync(
            ComputerVisionClient computerVision, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                DisplayResults(analysis, imagePath);
            }
        }

        // Display the most relevant caption for the image
        private static void DisplayResults(ImageAnalysis analysis, string imageUri)
        {
            if (analysis.Description.Captions.Count != 0)
            {
                picDescription = analysis.Description.Captions[0].Text; 
            }
            else
            {
                Console.WriteLine("No description generated.");
            }
        }




        async void OnCamButtonClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        async void OnPicButtonClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;
               

            var path = file.Path;
            localImagePath = path;
            image.Source = ImageSource.FromFile(path);



        }

        async void onCVButtonClicked(object sender, EventArgs e)
        {
            await AnalyzeLocalAsync(computerVision, localImagePath);
            resultLabel.Text = picDescription;
        }




    }
}
