using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Dog_Identifier_WpfClient.Models;
using System.Diagnostics;

namespace Dog_Identifier_WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new HttpClient();
            // http://localhost:5194
            // http://192.168.1.200:81
            // http://dogidentifier.duckdns.org:81
            client.BaseAddress = new Uri("http://localhost:5194");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void bt1_Click(object sender, RoutedEventArgs e)
        {
            bt1.IsEnabled = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.png";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {   
                
                this.result.Text = "Processing image...";

                Stopwatch sw = new Stopwatch();
                sw.Start();


                DogViewModel vm = new DogViewModel();
                vm.PhotoData = File.ReadAllBytes(ofd.FileName);
 
                try
                {
                    DogViewModel result = await GetDogType(vm);

                    this.result.Text = "Detected dog type(s):\n";
                    foreach (Dog dog in result.Dogs.Distinct<Dog>())
                    {
                        this.result.Text += dog.Name + "\n";
                    }

                    picture.Source = ToImage(result.PhotoData);
                    sw.Stop();

                    this.result.Text += $"\n It took {Math.Round(sw.Elapsed.TotalSeconds, 2)} seconds to process your image.";
                }
                catch (Exception err)
                {
                    sw.Stop ();
                    System.Windows.MessageBox.Show(err.Message,"An error has occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.result.Text = "";
                }           
            }

            bt1.IsEnabled = true;
        }

        private async Task<DogViewModel> GetDogType(DogViewModel model)
        {
            var response = await client.PostAsJsonAsync("/dog/getdogtype", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<DogViewModel>();
            }
            throw new Exception("Can't connect to the server.");
        }

        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        // To make mixed breed predictions

        //private async Task<MixedDogPredictionViewModel> GetMixedDogType(DogViewModel model)
        //{
        //    var response = await client.PostAsJsonAsync("/dog/getmixeddogtype", model);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return await response.Content.ReadAsAsync<MixedDogPredictionViewModel>();
        //    }
        //    throw new Exception("Can't communicate with the server.");
        //}

    }
}
