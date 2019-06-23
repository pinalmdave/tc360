using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechScreen.DBEntities;
using TechScreen.Models;
using TechScreen.Services;

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
            var screening = GetScreenings();

            return View(screening);
        }

        public IActionResult OpenReviewerModal()
        {
            var reviewers = this.screeningRepository.GetReviewers();

            return PartialView("_AssignReviewer", reviewers);
        }

        private List<ScreeningModel> GetScreenings()
        {
            var email = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;

            var screenings = this.screeningRepository.GetUserScreening(email);

            return this._mapper.Map<List<ScreeningModel>>(screenings);
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