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
    public class JobApplicantController : Controller
    {
        CloudStorageAccount storageAccount = null;
        CloudBlobContainer cloudBlobContainer = null;
        CloudBlobClient cloudBlobClient = null;

        static int questionCounter = 0;
        static string candidateSignInCode = "";

        private IScreeningRepository screeningRepository;

        public JobApplicantController(IScreeningRepository _screeningRepository)
        {
            this.screeningRepository = _screeningRepository;
        }

        public IActionResult Index()
        {
            var candidateCloudStorageInfoVM = new CandidateScreeningInfoVM();
            return View(candidateCloudStorageInfoVM);
        }

        [HttpPost]
        public IActionResult VerifyCandidateCode(CandidateScreeningInfoVM candidateStorageInfoVM)
        {
            if (string.IsNullOrEmpty(candidateStorageInfoVM.CandidateSignInCode))
            {
                ViewBag.InvalidCandidateCode = "Invalid SignIn Code";
                return View("Index"); //Return invalid screeningcode error message
            }
            var candidateStorageInfo = screeningRepository.GetCandidateInfoForSignUp(candidateStorageInfoVM.CandidateSignInCode, "Y");

            if(candidateStorageInfo == null)
            {
                ViewBag.InvalidCandidateCode = "Invalid SignIn Code";
                return View("Index");
            }
            var lstScreeningQuestions = candidateStorageInfo.ScreeningQuestions;

            if (lstScreeningQuestions == null)
            {
                ViewBag.InvalidCandidateCode = "Invalid SignIn Code";
                return View("Index"); //Return invalid screeningcode error message
            }
            ViewBag.InvalidCandidateCode = "";
            candidateSignInCode = candidateStorageInfoVM.CandidateSignInCode;

            ViewBag.CandidateCode = candidateStorageInfoVM.CandidateSignInCode;

            return View("Interview", lstScreeningQuestions);
        }

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
            questionCounter = questionCounter + 1;

            var candidateStorageInfo = screeningRepository.GetCandidateInfoForSignUp(candidateSignInCode, "N");
            //ScreeningId/CandidateId/QuestionNo
            var blobName = candidateStorageInfo.ScreeningId + "/" + candidateStorageInfo.CandidateId + "/" + questionCounter; //candidateSignInCode + "-" + questionCounter.ToString();

            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var memoryStream = new MemoryStream();

                    await file.CopyToAsync(memoryStream); //this.Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await UploadAsync(memoryStream, blobName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json("Success or failure response");
        }

        [RequestSizeLimit(100_000_000)]
        private async Task UploadAsync(MemoryStream stream, string blobName)
        {
            ConnectToAzureStorage();
            try
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
            catch (StorageException ex)
            {
                Console.WriteLine("Error returned from the service: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                throw;
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

        public IActionResult ScreeningCompleted(string candidateCode)
        {
            this.screeningRepository.CandiateScreeningCompleted(candidateCode);
            return Ok();
        }
    }
}