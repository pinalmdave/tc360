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


namespace TechScreen.Controllers
{
    public class ReviewerController : Controller
    {
        private readonly IMapper _mapper;

        private IScreeningRepository screeningRepository;


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
    }
}