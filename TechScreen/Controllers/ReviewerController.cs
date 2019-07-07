using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechScreen.DBEntities;
using TechScreen.Models;
using TechScreen.Services;
using TechScreen.ViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace TechScreen.Controllers
{
    public class ReviewerController : Controller
    {
        private readonly IMapper _mapper;

        private IScreeningRepository screeningRepository;

        CloudStorageAccount storageAccount = null;
        CloudBlobContainer cloudBlobContainer = null;
        CloudBlobClient cloudBlobClient = null;


        public ReviewerController(IScreeningRepository _screeningRepository, IMapper mapper)
        {
            _mapper = mapper;
            this.screeningRepository = _screeningRepository;
        }

        public IActionResult Index()
        {
            var reviewerScreenings = this.screeningRepository.GetReviewerScreenings(1);
            return View(reviewerScreenings);
        }

        public IActionResult CreateScreeningQuestions(int id)
        {
            var screeningViewModel = new ScreeningViewModel();

            var screening = this.screeningRepository.GetScreeningDetailToCreateQuestions(id);
            screeningViewModel.Screening = screening;

            TempData["ScreeningId"] = id;

            return View(screeningViewModel);
        }

        [HttpPost]
        public IActionResult CreateScreeningQuestions()
        {
            var userEmail = User.Claims.First(x => x.Type == "emails").Value;
            var screeningId = (int)TempData["ScreeningId"];
            var formdata_screeningQuestions = Request.Form["questions"];
            var lstscreeningQuestionsModel = JsonConvert.DeserializeObject<List<ScreeningQuestionsModel>>(formdata_screeningQuestions);
            
            if (ModelState.IsValid)
            {
                try
                {   
                   this.screeningRepository.CreateScreeningQuestions(screeningId, userEmail, lstscreeningQuestionsModel);
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Ok();
            }
            return View("CreateJobRequirement");
        }

        public IActionResult ReviewInterview(int candidateId, int screeningId)
        {
            var questions = this.screeningRepository.GetScreeningQuestions(candidateId, screeningId);

            var vm = new CandidateScreeningInfoVM();

            vm.CandidateId = candidateId.ToString();
            vm.ScreeningId = screeningId.ToString();

            vm.ScreeningQuestions = questions;

            return View(vm);
        }

        [HttpPost]
        public IActionResult SubmitScreeningFeedback()
        {
            var candidateTechSkillDetails = Request.Form["techSkillScore"];
            var candidateScoreSummary = Request.Form["scoreSummary"];

            var userEmail = User.Claims.First(x => x.Type == "emails").Value;

            candidateScoreSummary = candidateScoreSummary.ToString().Replace("ScreeningCandidateModel.", "");
            string responseString = candidateScoreSummary;
            var dict = HttpUtility.ParseQueryString(responseString);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
            var screeningCandidateModel = JsonConvert.DeserializeObject<ScreeningCandidateModel>(json);

            var screeningCandidate = this._mapper.Map(screeningCandidateModel, new ScreeningCandidate());


            var detailedCandidateScreening = JsonConvert.DeserializeObject<List<DetailedCandidateScreening>>(candidateTechSkillDetails);
            foreach (var item in detailedCandidateScreening)
            {
                item.CreatedOn = DateTime.Now;
                item.CreatedBy = userEmail;
                item.ScreeningId = screeningCandidateModel.ScreeningId;
                item.CandidateId = screeningCandidateModel.CandidateId;
                //item.ReviewerId = 1;
            }

            this.screeningRepository.SubmitScreeningFeedback(screeningCandidate, detailedCandidateScreening);

            return Json(Url.Action("Index", "Reviewer"));
        }

        public async Task<CloudBlockBlob> GetBlob(string screeningId, string candidateId, string questionNo)
        {
            try
            {
                var blob = await DownloadAsync(screeningId, candidateId, questionNo);

                return blob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private async Task<CloudBlockBlob> DownloadAsync(string screeningId, string candidateId, string questionNo)
        {
            ConnectToAzureStorage();
            try
            {
                var questionpath = screeningId + "/" + candidateId + "/" + questionNo;
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(questionpath);
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

        public IActionResult ScoreCard(int id)
        {
            var result = this.screeningRepository.GetCandidateScoreCard(id);
            return View(result);
        }
    }
}