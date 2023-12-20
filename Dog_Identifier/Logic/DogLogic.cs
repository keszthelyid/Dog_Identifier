using Dog_Identifier.Models;
using Dog_Identifier.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace Dog_Identifier.Logic
{
    public class DogLogic : IDogLogic
    {
        private IDogRepository Repository;
        public DogLogic(IDogRepository DogRepo)
        {
            this.Repository = DogRepo;
        }

        public IEnumerable<Dog> GetDogs()
        {
            return Repository.GetAllDogs();
        }

        public async Task<DogViewModel> GetDogType(DogViewModel model)
        {
            string id = Guid.NewGuid().ToString();
            File.WriteAllBytes(id + ".jpg", model.PhotoData);

            string res = await ProcessImage(id + ".jpg", false);
            string[] DogNames = res.Split(';');
            
            DogViewModel ToReturn = new DogViewModel()
            {
                Dogs = new Dog[DogNames.Length]
            };

            if (res == "None" || res == "ERROR")
            {
                ToReturn.Dogs[0] = new Dog() { Name = "None" };
            }
            else
            {
                for (int i = 0; i < DogNames.Length; i++)
                {
                    Dog Dog = Repository.GetDogByName(DogNames[i]).GetCopy();
                    
                    ToReturn.Dogs[i] = Dog;
                    ToReturn.Dogs[i].PhotoData = File.ReadAllBytes(Path.Combine("Dog_Photos", Dog.Name + ".jpg"));
                }

                ToReturn.PhotoData = File.ReadAllBytes(id + "_output.png");
            }         

            File.Delete(id + "_output.png");
            File.Delete(id + ".jpg");
            
            return ToReturn;
        }

        public async Task<MixedDogPredictionViewModel> GetMixedDogType(DogViewModel model)
        {
            string id = Guid.NewGuid().ToString();
            File.WriteAllBytes(id + ".jpg", model.PhotoData);

            string res = await ProcessImage(id + ".jpg", true); 
            string[] Dogs = res.Split(';');
            
            MixedDogPredictionViewModel ToReturn = new MixedDogPredictionViewModel()
            {
                Predictions = new Dog[Dogs.Length][]
            };
            
            if (res == "None" || res == "ERROR")
            {
                ToReturn.Predictions[0] = new Dog[1];
                ToReturn.Predictions[0][0] = new Dog() { Name = "None" };
            }
            else 
            {
                for (int i = 0; i < Dogs.Length; i++)
                {
                    ToReturn.Predictions[i] = new Dog[Dogs[i].Split('*').Length];

                    int j = 0;
                    foreach (string pred in Dogs[i].Split('*'))
                    {
                        string dogname = pred.Split(':')[0];

                        Dog dog = Repository.GetDogByName(dogname).GetCopy();

                        ToReturn.Predictions[i][j] = dog;
                        ToReturn.Predictions[i][j].PhotoData = File.ReadAllBytes(Path.Combine("Dog_Photos", dog.Name + ".jpg"));
                        ToReturn.Predictions[i][j].Percentage = double.Parse(pred.Split(':')[1].Replace('.', ','));

                        j++;
                    }
                }

                ToReturn.PhotoData = File.ReadAllBytes(id + "_output.png");
            }

            File.Delete(id + "_output.png");
            File.Delete(id + ".jpg");

            return ToReturn;
        }

        private Task<string> ProcessImage(string path, bool enableMixedPrediction)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = enableMixedPrediction ?  "mixdog.exe" : "dog.exe";
                psi.Arguments = $"\"{path}\"";
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                string errors = "";
                string results = "";

                Process? process = Process.Start(psi);
                
                using (process)
                {
                    errors = process.StandardError.ReadToEnd();
                    results = process.StandardOutput.ReadToEnd();
                }

                if (errors == "")
                {
                    return results;
                }
                else
                {
                    return "ERROR";
                }
            });
        }
    }
}
