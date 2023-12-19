using Castle.Core.Resource;
using Dog_Identifier.Logic;
using Dog_Identifier.Models;
using Dog_Identifier.Repository;
using Moq;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class LogicTests
    {
        private DogLogic Logic;

        [SetUp]
        public void Setup()
        {
            Mock<IDogRepository> mockedDogRepo = new Mock<IDogRepository>();

            mockedDogRepo.Setup(x => x.GetAllDogs()).Returns(this.FakeDogObjects());
            mockedDogRepo.Setup(x => x.GetDogByName("Wire Fox Terrier")).Returns(this.FakeDogObjects().First(x => x.Name == "Wire Fox Terrier"));
            mockedDogRepo.Setup(x => x.GetDogByName("Siberian Husky")).Returns(this.FakeDogObjects().First(x => x.Name == "Siberian Husky"));
            mockedDogRepo.Setup(x => x.GetDogByName("German Shepherd")).Returns(this.FakeDogObjects().First(x => x.Name == "German Shepherd"));
            mockedDogRepo.Setup(x => x.GetDogByName("Golden Retiver")).Returns(this.FakeDogObjects().First(x => x.Name == "Golden Retiver"));
            mockedDogRepo.Setup(x => x.GetDogByName("Rottweiler")).Returns(this.FakeDogObjects().First(x => x.Name == "Rottweiler"));
            mockedDogRepo.Setup(x => x.GetDogByName("Doberman")).Returns(this.FakeDogObjects().First(x => x.Name == "Doberman"));

            Logic = new DogLogic(mockedDogRepo.Object);
        }


        [Test]
        public void GetAllDogsTest()
        {
            List<Dog> Dogs = Logic.GetDogs().ToList();

            Assert.That(Dogs.Count == 35);
            Assert.That(Dogs[0].Name == "Afgan Hound");
        }


        [Test]
        public async Task SimpleScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "1.jpg"))
            };

            DogViewModel Answer = await Logic.GetDogType(ToSend);

            Assert.That(Answer.Dogs[0].Name, Is.EqualTo("Wire Fox Terrier"));

        }

        [Test]
        public async Task ThreeSameBreedsOnOnePhotoScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "3.jpg"))
            };

            DogViewModel Answer = await Logic.GetDogType(ToSend);

            Assert.That(Answer.Dogs[0].Name, Is.EqualTo("Golden Retiver"));
            Assert.That(Answer.Dogs[1].Name, Is.EqualTo("Golden Retiver"));
            Assert.That(Answer.Dogs[2].Name, Is.EqualTo("Golden Retiver"));
            Assert.That(Answer.Dogs.Length, Is.EqualTo(3));
        }

        [Test]
        public async Task TwoDogsOnOnePhotoScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "4.jpg"))
            };

            DogViewModel Answer = await Logic.GetDogType(ToSend);

            if ((Answer.Dogs.Length == 2) && (Answer.Dogs[0].Name == "German Shepherd" && Answer.Dogs[1].Name == "Siberian Husky") || (Answer.Dogs[1].Name == "German Shepherd" && Answer.Dogs[0].Name == "Siberian Husky"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task MixedBreedDogScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "2.png"))
            };

            MixedDogPredictionViewModel Answer = await Logic.GetMixedDogType(ToSend);

            if ((Answer.Predictions.Length == 1) && (Answer.Predictions[0][0].Name == "German Shepherd" && Answer.Predictions[0][1].Name == "Siberian Husky") || (Answer.Predictions[0][1].Name == "German Shepherd" && Answer.Predictions[0][0].Name == "Siberian Husky"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }            
        }

        [Test]
        public async Task MixedBreedTestOnPureBreedDog()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "1.jpg"))
            };

            MixedDogPredictionViewModel Answer = await Logic.GetMixedDogType(ToSend);

            Assert.That(Answer.Predictions.Length, Is.EqualTo(1));
            Assert.That(Answer.Predictions[0][0].Name, Is.EqualTo("Wire Fox Terrier"));
        }

        [Test]
        public async Task TwoDogsOnOnePhotoMixedBreedScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = File.ReadAllBytes(Path.Combine("Test Photos", "5.jpg"))
            };

            MixedDogPredictionViewModel Answer = await Logic.GetMixedDogType(ToSend);

            if ((Answer.Predictions.Length == 2) && (Answer.Predictions[0][0].Name == "Doberman" && Answer.Predictions[1][0].Name == "Rottweiler") || (Answer.Predictions[1][0].Name == "Doberman" && Answer.Predictions[0][0].Name == "Rottweiler"))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task RequestWithoutPhotoScanTest()
        {
            DogViewModel ToSend = new DogViewModel()
            {
                PhotoData = new byte[1]
            };

            DogViewModel Answer = await Logic.GetDogType(ToSend);

            Assert.That(Answer.Dogs.Length, Is.EqualTo(1));
            Assert.That(Answer.Dogs[0].Name, Is.EqualTo("None"));
        }

        private IQueryable<Dog> FakeDogObjects()
        {
            List<Dog> Dogs = new List<Dog>();

            #region Entities
            Dog d0 = new Dog() { Name = "Afgan Hound", Link = "https://www.petfinder.com/dog-breeds/afghan-hound/", ApiId = "dd9362cc-52e0-462d-b856-fccdcf24b140" };
            Dog d1 = new Dog() { Name = "Australian Terrier", Link = "https://www.petfinder.com/dog-breeds/australian-terrier/", ApiId = "4b3278eb-612a-4e87-b2ab-73174d039514" };
            Dog d2 = new Dog() { Name = "Border Collie", Link = "https://www.petfinder.com/dog-breeds/border-collie/", ApiId = "20b1d8be-ae44-4a70-8526-0612904bc9b2" };
            Dog d3 = new Dog() { Name = "Bouvier des Flandres", Link = "https://www.petfinder.com/dog-breeds/bouvier-des-flandres/", ApiId = "4ddbe251-72af-495e-8e9d-869217e1d92a" };
            Dog d4 = new Dog() { Name = "Clumber Spaniel", Link = "https://www.petfinder.com/dog-breeds/clumber-spaniel/", ApiId = "3c7ca8f4-175f-4d55-bedb-5c53907340f9" };
            Dog d5 = new Dog() { Name = "Collie", Link = "https://www.petfinder.com/dog-breeds/collie/", ApiId = "6b57d7c1-553d-4f46-b33b-c9c5a3a67c96" };
            Dog d6 = new Dog() { Name = "Dalmatian", Link = "https://www.petfinder.com/dog-breeds/dalmatian/", ApiId = "9ca1f843-4cad-45b3-847f-bc7975864b1d" };
            Dog d7 = new Dog() { Name = "Doberman", Link = "https://www.petfinder.com/dog-breeds/doberman-pinscher/", ApiId = "29fac412-58f8-44c4-8a47-77527e55123b" };
            Dog d8 = new Dog() { Name = "English Springer", Link = "https://www.petfinder.com/dog-breeds/english-springer-spaniel/", ApiId = "3b7aa8d3-4e4c-424b-bf4d-11453c26bb00" };
            Dog d9 = new Dog() { Name = "Wire Fox Terrier", Link = "https://www.petfinder.com/dog-breeds/wire-fox-terrier/", ApiId = "5b631664-c346-4e2c-89c5-ee0b82cafa6f" };
            Dog d10 = new Dog() { Name = "French Bulldog", Link = "https://www.petfinder.com/dog-breeds/french-bulldog/", ApiId = "1fa29da4-8cc9-4f82-9c4d-cd93ee6dd6be" };
            Dog d11 = new Dog() { Name = "German Shepherd", Link = "https://www.petfinder.com/dog-breeds/german-shepherd/", ApiId = "74a2d74a-48a3-4281-a473-abfcd59a0d60" };
            Dog d12 = new Dog() { Name = "Giant Schnauzer", Link = "https://www.petfinder.com/dog-breeds/giant-schnauzer/", ApiId = "4033b262-984d-458a-9a0d-aa8e9cdda8e4" };
            Dog d13 = new Dog() { Name = "Golden Retiver", Link = "https://www.petfinder.com/dog-breeds/golden-retriever/", ApiId = "fee91641-2a2e-4c4f-b557-cff67c5803bc" };
            Dog d14 = new Dog() { Name = "Irish Setter", Link = "https://www.petfinder.com/dog-breeds/irish-setter/", ApiId = "89e06189-4e3c-43a3-9494-16073e888fc0" };
            Dog d15 = new Dog() { Name = "Komondor", Link = "https://www.petfinder.com/dog-breeds/komondor/", ApiId = "63ada456-e0c7-4b52-9759-25efec4dc540" };
            Dog d16 = new Dog() { Name = "Kuvasz", Link = "https://www.petfinder.com/dog-breeds/kuvasz/", ApiId = "9cb2e6d5-bacb-4b14-a534-7b1b243045b0" };
            Dog d17 = new Dog() { Name = "Maltese Dog", Link = "https://www.petfinder.com/dog-breeds/maltese/", ApiId = "521d8c02-32b9-4b50-8f0d-f791b4bfe697" };
            Dog d18 = new Dog() { Name = "Old English Sheepdog", Link = "https://www.petfinder.com/dog-breeds/old-english-sheepdog/", ApiId = "169f3d91-4430-452e-8dac-8399286444d8" };
            Dog d19 = new Dog() { Name = "Papillon", Link = "https://www.petfinder.com/dog-breeds/papillon/", ApiId = "88591552-b3bd-45fd-931c-5c8b88aad4f2" };
            Dog d20 = new Dog() { Name = "Pomeranian", Link = "https://www.petfinder.com/dog-breeds/pomeranian/", ApiId = "b7211a59-787d-4b34-b390-77b9b0dc5b9d" };
            Dog d21 = new Dog() { Name = "Pug", Link = "https://www.petfinder.com/dog-breeds/pug/", ApiId = "a6ea38ed-f692-478e-af29-378d0e2cc270" };
            Dog d22 = new Dog() { Name = "Rottweiler", Link = "https://www.petfinder.com/dog-breeds/rottweiler/", ApiId = "11f90c78-8f4c-43f7-bc47-fab733a33c6b" };
            Dog d23 = new Dog() { Name = "Saluki", Link = "https://www.petfinder.com/dog-breeds/saluki/", ApiId = "58f9eb88-7288-42a5-b34a-5b5192eaac17" };
            Dog d24 = new Dog() { Name = "Schipperke", Link = "https://www.petfinder.com/dog-breeds/schipperke/", ApiId = "88025f0c-987f-42e9-98e0-ccb8b53516e9" };
            Dog d25 = new Dog() { Name = "Scottish Deerhund", Link = "https://www.petfinder.com/dog-breeds/scottish-deerhound/", ApiId = "861977df-b35a-49c4-b155-e9d29bbd2f0d" };
            Dog d26 = new Dog() { Name = "Siberian Husky", Link = "https://www.petfinder.com/dog-breeds/siberian-husky/", ApiId = "e0561c26-6d8b-42ae-88df-3047873e929a" };
            Dog d27 = new Dog() { Name = "Soft Coated Wheaten Terrier", Link = "https://www.petfinder.com/dog-breeds/soft-coated-wheaten-terrier/", ApiId = "faa43ad0-3e19-4506-9a31-0bd1d821f5aa" };
            Dog d28 = new Dog() { Name = "Standard Poodle", Link = "https://www.petfinder.com/dog-breeds/poodle/", ApiId = "710ba3fe-c4ee-4d7c-b5cc-1cb19a4815fc" };
            Dog d29 = new Dog() { Name = "Tibetan Terrier", Link = "https://www.petfinder.com/dog-breeds/tibetan-terrier/", ApiId = "84cb99f1-4735-4dc9-a3b3-25a5f0fb0eb5" };
            Dog d30 = new Dog() { Name = "Tibetan Mastiff", Link = "https://www.petfinder.com/dog-breeds/tibetan-mastiff/", ApiId = "1ce29779-f1c3-47f4-894c-c45173f35336" };
            Dog d31 = new Dog() { Name = "Vizsla", Link = "https://www.petfinder.com/dog-breeds/vizsla/", ApiId = "2d7be3ac-c72b-4d2f-af62-93a1f1d944f9" };
            Dog d32 = new Dog() { Name = "Welsh Springer Spaniel", Link = "https://www.petfinder.com/dog-breeds/welsh-springer-spaniel/", ApiId = "90dac44c-f0d9-43bb-9560-cd47010a72ec" };
            Dog d33 = new Dog() { Name = "Whippet", Link = "https://www.petfinder.com/dog-breeds/whippet/", ApiId = "32534831-9241-4be3-a594-70c45e1a9bbe" };
            Dog d34 = new Dog() { Name = "Yorkshire Terrier", Link = "https://www.petfinder.com/dog-breeds/yorkshire-terrier/", ApiId = "ce40589c-295a-4259-9e83-711854db8129" };
            #endregion

            Dogs.AddRange(new[] { d0, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, d17, d18, d19, d20, d21, d22, d23, d24, d25, d26, d27, d28, d29, d30, d31, d32, d33, d34 });

            return Dogs.AsQueryable();
        }
    }
}