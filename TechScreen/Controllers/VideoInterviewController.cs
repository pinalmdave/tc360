using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TechScreen.Services;
using TechScreen.ViewModels;
using Newtonsoft.Json;
using TechScreen.DBEntities;

namespace TechScreen.Controllers
{
    public class VideoInterviewController : Controller
    {
        CloudStorageAccount storageAccount = null;
        CloudBlobContainer cloudBlobContainer = null;
        CloudBlobClient cloudBlobClient = null;

       // public static Queue<DBEntities.ScreeningQuestions> staticQueScreeningQuestions;

        private IScreeningRepository screeningRepository;


        public VideoInterviewController(IScreeningRepository _screeningRepository)
        {
            this.screeningRepository = _screeningRepository;
        }


        public IActionResult Index()
        {
            var candidateCodeViewModel = new CandidateCodeViewModel();
            return View(candidateCodeViewModel);
        }

        [HttpPost]
        public IActionResult VerifyCandidateCode(CandidateCodeViewModel candidateCodeViewModel)
        {
            if(string.IsNullOrEmpty(candidateCodeViewModel.CandidateCode)) return View("Index"); //Return invalid screeningcode error message

            var lstScreeningQuestions = screeningRepository.GetScreeningQuestions(candidateCodeViewModel.CandidateCode);

            if (lstScreeningQuestions == null) return View("Index"); //Return invalid screeningcode error message

            //Queue<ScreeningQuestions> queScreeningQuestions = new Queue<ScreeningQuestions>();

            //foreach (var item in lstScreeningQuestions)
            //{
            //    queScreeningQuestions.Enqueue(item);
            //}

            //staticQueScreeningQuestions = queScreeningQuestions;
            
            //var json1 = JsonConvert.SerializeObject(lstScreeningQuestions);



            //var qText1 = queScreeningQuestions.Dequeue().QuestionText;
            //var qText2 = queScreeningQuestions.Dequeue().QuestionText;
            //var qText3 = queScreeningQuestions.Dequeue().QuestionText;
            //var qText4 = queScreeningQuestions.Dequeue().QuestionText;
            //var qText5 = queScreeningQuestions.Dequeue().QuestionText;

            return View("Interview", lstScreeningQuestions);
        }

        //public IActionResult GetQuestion()
        //{
        //    var question = staticQueScreeningQuestions.Dequeue();
            
        //    var screenQuestion = new ScreeningQuestions();
        //    screenQuestion.QuestionId = question.QuestionId;
        //    screenQuestion.QuestionText = question.QuestionText;

        //    return Json(JsonConvert.SerializeObject(screenQuestion));
        //}

        private void ConnectToAzureStorage()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    cloudBlobContainer = cloudBlobClient.GetContainerReference("screening-videos");

                }
                catch (StorageException ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named 'storageconnectionstring' with your storage " +
                    "connection string as a value.");
            }
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> PostRecordedAudioVideo()
        {
            var file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                var memoryStream = new MemoryStream();

                await file.CopyToAsync(memoryStream); //this.Request.Body.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                await UploadAsync(memoryStream);

            }
            return Json("Success or failure response");
        }

        [RequestSizeLimit(100_000_000)]
        private async Task UploadAsync(MemoryStream stream)
        {
            ConnectToAzureStorage();
            try
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("3");

                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
            catch (StorageException ex)
            {
                Console.WriteLine("Error returned from the service: {0}", ex.Message);
            }
        }

        private async Task<CloudBlockBlob> DownloadAsync()
        {
            ConnectToAzureStorage();
            try
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("TestBlobName2");
                var stream = new MemoryStream();
                await cloudBlockBlob.DownloadToStreamAsync(stream);
                return cloudBlockBlob;
            }
            catch (StorageException ex)
            {
                Console.WriteLine("Error returned from the service: {0}", ex.Message);
                return null;
            }
        }

        public IActionResult Review()
        {
            return View();
        }

        public async Task<CloudBlockBlob> GetBlob()
        {
            try
            {
                var blob = await DownloadAsync();

                return blob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}