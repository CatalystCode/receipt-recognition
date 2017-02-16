using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.ComponentModel;
using System.Diagnostics;

using static System.Diagnostics.Debug;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ExpenseIt;

namespace InvoiceIt
{
    public class InvoicesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Invoice> Invoices { get; } = new ObservableCollection<Invoice>();

        public string Message { get; set; } = "Hello World!";

        Command addInvoiceCommand = null;
        public Command AddInvoiceCommand =>
                    addInvoiceCommand ?? (addInvoiceCommand = new Command(async () => await ExecuteAddInvoiceCommandAsync()));


        async Task ExecuteAddInvoiceCommandAsync()
        {
            try
            {
                IsBusy = true;
                // 1. Add camera logic.
                await CrossMedia.Current.Initialize();

                MediaFile photo;
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Receipts",
                        Name = "Receipt"
                    });
                }
                else
                {
                    photo = await CrossMedia.Current.PickPhotoAsync();
                }

                if (photo == null)
                {
                    PrintStatus("Photo was null :(");
                    return;
                }


                byte[] data = ReadFully(photo.GetStream());

                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {

                        var fileContent = new ByteArrayContent(data);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") // attachment?
                        {
                            Name = "file", // required?
                            FileName = "test.jpg"
                        };
                        content.Add(fileContent);
                        //IsBusy = false;

                        var requestUri = "http://at-dev-vm-zxpapi-01.westus.cloudapp.azure.com/v1.0/groups/1/receipts";
                        //var requestUri = "http://requestb.in/11y6k4z1";
                        var response = await client.PostAsync(requestUri, content);
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(result);
                   
                            using (var clientML = new HttpClient())
                            {
                                string temp = apiResponse.Receipt_Url.Replace("https", "http");

                                var scoreRequest = new
                                {

                                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                                                        {
                                                            "input1",
                                                            new List<Dictionary<string, string>>() {
                                                            new Dictionary<string, string>() {
                                                            //{ "Category", "" },
                                                            { "Url", temp},
                                                         }
                                                     }
                                    },
                                    },
                                    GlobalParameters = new Dictionary<string, string>()
                                    {
                                    }
                                };

                                var jsonToSend = JsonConvert.SerializeObject(scoreRequest);
                                var body = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

                                const string apiKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"; // Replace this with the API key for the web service
                                clientML.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                                clientML.BaseAddress = new Uri("https://xxx.services.azureml.net/subscriptions/xxxxxxxxx/services/xxxxxxx/execute?api-version=2.0&format=swagger");

                                var responseML = await clientML.PostAsync("", body);

                                if (responseML.IsSuccessStatusCode)
                                {
                                    string resultML = await responseML.Content.ReadAsStringAsync();
                                    // Convert JSON to object
                                    MLResponse obj = JsonConvert.DeserializeObject<MLResponse>(resultML);

                                    var outPuts = new List<MLOutputItem>(6);

                                    outPuts.Add(new MLOutputItem { Category = "Clothes and Accessories", Probability = (float)obj.Results.Output[0].ClothesAndAccessoriesProbability });
                                    outPuts.Add(new MLOutputItem { Category = "Daily Snacks", Probability = (float)obj.Results.Output[0].DailySnacksProbability });
                                    outPuts.Add(new MLOutputItem { Category = "Dining Out", Probability = (float)obj.Results.Output[0].DiningOutProbability });
                                    outPuts.Add(new MLOutputItem { Category = "Entertainment", Probability = (float)obj.Results.Output[0].EntertainmentProbability });
                                    outPuts.Add(new MLOutputItem { Category = "Fuel", Probability = (float)obj.Results.Output[0].FuelProbability });
                                    outPuts.Add(new MLOutputItem { Category = "Groceries", Probability = (float)obj.Results.Output[0].GroceriesProbability });

                                    outPuts = outPuts.OrderByDescending(x => x.Probability).ToList();
                              
                                    Invoices.Add(new Invoice
                                    {
                                        ScoredLabel1 = outPuts[0].Category + " : " + outPuts[0].Probability ,
                                        ScoredLabel2 = outPuts[1].Category + " : " + outPuts[1].Probability ,
                                        ScoredLabel3 = outPuts[2].Category + " : " + outPuts[2].Probability ,
                                        Photo = photo.Path,
                                        TimeStamp = DateTime.Now
                                    });

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }

        }


        public void PrintStatus(string helloWorld)
        {
            if (helloWorld == null)
                throw new ArgumentNullException(nameof(helloWorld));

            WriteLine(helloWorld);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                if (busy == value)
                    return;

                busy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Message));
            }
        }


    }
}