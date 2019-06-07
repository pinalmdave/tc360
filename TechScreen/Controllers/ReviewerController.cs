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
    public class ReviewerController : Controller
    {
        private IScreeningRepository screeningRepository;
        private readonly IMapper _mapper;


        public ReviewerController(IScreeningRepository _screeningRepository, IMapper mapper)
        {
            this.screeningRepository = _screeningRepository;
            this._mapper = mapper;
        }


        public IActionResult Index()
        {
            var reviewerScreenings = this.screeningRepository.GetReviewerScreenings(3);

            return View();
        }
    }
}