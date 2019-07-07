using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechScreen.DBEntities;
using TechScreen.Models;
using TechScreen.Services;
using TechScreen.ViewModels;

namespace TechScreen.Controllers
{
    public class AdminController : Controller
    {
        private IScreeningRepository screeningRepository;
        private readonly IMapper _mapper;

        public AdminController(IScreeningRepository _screeningRepository, IMapper mapper)
        {
            this.screeningRepository = _screeningRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            var adminVm = GetAdminViewModel();

            var jsonObject = JsonConvert.SerializeObject(adminVm, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return View(adminVm);
        }

        public IActionResult OpenReviewerModal()
        {
            var reviewers = this.screeningRepository.GetReviewers();

            return PartialView("_AssignReviewer", reviewers);
        }

        private AdminVM GetAdminViewModel()
        {
            var screenings = this.screeningRepository.GetAllScreeningSummaryForAdmin();
            var adminVm = new AdminVM();
            adminVm.lstScreening = screenings;
            return adminVm;
        }

        public IActionResult GetScreeningDetailForAdmin(int id)
        {
            var screening = this.screeningRepository.GetScreeningDetailForAdmin(id);

            return Ok();
        }

        public IActionResult AssignCandidateToReviewer(int screeningId, int candidateId, int reviewerId)
        {
            var userName = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;

            var screenings = this.screeningRepository.AssignCandidateToReviewer(screeningId, candidateId, reviewerId, userName);

            return Ok(); 
        }

        public IActionResult AssignScreeningToReviewer(int screeningId, int reviewerId)
        {
            var userName = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;

            var screenings = this.screeningRepository.AssignScreeningToReviewer(screeningId, reviewerId, userName);

            return Ok();
        }
    }
}