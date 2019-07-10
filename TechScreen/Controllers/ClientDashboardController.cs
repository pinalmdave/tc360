using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using TechScreen.DBEntities;
using TechScreen.Models;
using TechScreen.Services;
using TechScreen.Utilities;
using TechScreen.ViewModels;

namespace TechScreen.Controllers
{
    public class ClientDashboardController : Controller
    {
        private IScreeningRepository screeningRepository;
        private readonly IMapper _mapper; 

        public ClientDashboardController(IScreeningRepository _screeningRepository, IMapper mapper)
        {
            this.screeningRepository = _screeningRepository;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {  
            var email = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;

            var isNewClient = this.screeningRepository.IsNewClient(email);

            if (isNewClient)
            {
                AddNewClient();
            }

            var screening = GetScreenings();

            return View(screening);
            //return View("Dashboard");
        }


        private List<ScreeningModel> GetScreenings()
        {  
            var email = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;

            var screenings = this.screeningRepository.GetUserScreening(email);

            return this._mapper.Map<List<ScreeningModel>>(screenings);
        }
            

        private void AddNewClient()
        {
            var email = User.Claims.First(x => x.Type == "emails").Value;
            var jobTitle = User.Claims.First(x => x.Type == "jobTitle").Value;
            var firstName = User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
            var lastName = User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            var contactNo = User.Claims.First(x => x.Type == "extension_ContactNo").Value;
            var companyName = User.Claims.First(x => x.Type == "extension_CompanyName").Value;

            var client = new Client();
            client.Email = email;
            client.CreatedOn = DateTime.Now;
            client.CreatedBy = email;
            client.Phone = contactNo;
            client.CompanyName = companyName;

            var clientId = screeningRepository.AddNewClient(client);

            var user = new User();
            user.ClientId = clientId;
            user.CreatedBy = client.Email;
            user.CreatedOn = DateTime.Now;
            user.Designation = jobTitle;
            user.IsAdmin = "Y";
            user.UserEmail = email;
            user.UserFirstName = firstName;
            user.UserLastName = lastName;
            user.UserPhone = contactNo;
            screeningRepository.AddNewUser(user);

        }

        [HttpPost]
        public IActionResult NewScreening(Screening screening)
        {
            var userId = User.Claims.Where(x => x.Type == "emails").FirstOrDefault().Value;
            screening.UserId = 1;//userId;
            screeningRepository.AddNewScreening(screening);
            return View();
        }

        public IActionResult NewScreening()
        {  
            return RedirectToAction("Create", "Screening");
        }

        public IActionResult OpenCandidateModal()
        {
            var screeningCandidateModel = new ScreeningCandidateModel();

            return PartialView("_AddCandidate", screeningCandidateModel);
        }
    }
}