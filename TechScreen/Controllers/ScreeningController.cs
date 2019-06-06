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
    public class ScreeningController : Controller
    {
        private readonly TechscreenDBContext _context;
        private readonly IMapper _mapper;

        private IScreeningRepository screeningRepository;


        public ScreeningController(TechscreenDBContext context, IScreeningRepository _screeningRepository, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.screeningRepository = _screeningRepository;
        }

        // GET: Screening
        public IActionResult Index()
        {
            var userEmail = User.Claims.First(x => x.Type == "emails").Value;
            var lstScreening = this.screeningRepository.GetUserScreening(userEmail);
            return View(lstScreening);

            //var techscreenDBContext = _context.Screening.Include(s => s.User);
            //return View(await techscreenDBContext.ToListAsync());
        }

        // GET: Screening/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }


        [HttpPost]
        public IActionResult PostCandidates(ScreeningCandidateModel screeningCandidateModel)
        {  
            if (ModelState.IsValid)
            {
                var userEmail = User.Claims.First(x => x.Type == "emails").Value;

                var screeningCandidate = _mapper.Map(screeningCandidateModel, new ScreeningCandidate());

                screeningCandidate.CreatedBy = userEmail;
                screeningCandidate.CreatedOn = DateTime.Now;
                _context.Add(screeningCandidate);
                _context.SaveChangesAsync();

            }

            return RedirectToAction("Index", "ClientDashboard");
        }

        [HttpPost]
        public IActionResult PostJobRequirement(ScreeningViewModel screeningViewModel)
        {
            var userEmail = User.Claims.First(x => x.Type == "emails").Value;

            var candidateData = Request.Form["candidates"];
            var screeningData = Request.Form["jobReq"];

            var screeningCandidateModel = JsonConvert.DeserializeObject<List<ScreeningCandidateModel>>(candidateData);
            foreach(var item in screeningCandidateModel)
            {
                item.ScreeningStatus = "P"; // i.e. Pending
                item.CreatedBy = userEmail;
                item.CreatedOn = DateTime.Now;
            }

            screeningData = screeningData.ToString().Replace("Screening.", "");
            string responseString = screeningData;
            var dict = HttpUtility.ParseQueryString(responseString);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
            var screeningModel = JsonConvert.DeserializeObject<ScreeningModel>(json);
            screeningModel.ScreeningCandidate = screeningCandidateModel;

            if (ModelState.IsValid)
            {
                try
                {
                    var screening = _mapper.Map(screeningModel, new Screening());

                    screening.CreatedBy = userEmail;
                    screening.UserId = this._context.User.First(x => x.UserEmail == userEmail).UserId;
                    screening.CreatedOn = DateTime.Now;

                    _context.Add(screening);
                     _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction("Index", "ClientDashboard");
            }
            return View("CreateJobRequirement", screeningViewModel);
        }

        public IActionResult AddCandidate(int id)
        {
            var model = new ScreeningCandidateModel();
            model.ScreeningId = id;
            return View(model);
        }

        public IActionResult AddScreeningCandidate()
        {
            var model = new ScreeningCandidateModel();
            return PartialView("_ScreeningCandidate", model);
        }

        public IActionResult CreateJobRequirement()
        {
            var screeningViewModel = new ScreeningViewModel();

            screeningViewModel.lstScreeningCandidates = _mapper.Map<List<ScreeningCandidateModel>> (this._context.ScreeningCandidate.ToList());
            screeningViewModel.lstJobCategories = _mapper.Map<List<JobCategoriesModel>> (this._context.JobCategories.ToList());
            screeningViewModel.lstScreeningQuestions = _mapper.Map<List<ScreeningQuestionsModel>>(this._context.ScreeningQuestions.ToList());
            screeningViewModel.lstTechnologyStack = _mapper.Map<List<TechnologyStackModel>> (this._context.TechnologyStack.ToList());
            screeningViewModel.lstTechnologies = _mapper.Map<List<TechnologiesModel>>(this._context.Technologies.ToList());

            return View(screeningViewModel);
        }

        public IActionResult AddCandidatesForScreening()
        {
            return View();
        }

        public IActionResult ReviewDetails()
        {
            return View();
        }

        public IActionResult MakePayment()
        {
            return View();
        }


        // GET: Screening/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");

            var screeningViewModel = new ScreeningViewModel();

            screeningViewModel.lstScreeningCandidates = _mapper.Map<List<ScreeningCandidateModel>>(this._context.ScreeningCandidate.ToList());
            screeningViewModel.lstJobCategories = _mapper.Map<List<JobCategoriesModel>>(this._context.JobCategories.ToList());
            screeningViewModel.lstScreeningQuestions = _mapper.Map<List<ScreeningQuestionsModel>>(this._context.ScreeningQuestions.ToList());
            screeningViewModel.lstTechnologyStack = _mapper.Map<List<TechnologyStackModel>>(this._context.TechnologyStack.ToList());
            screeningViewModel.lstTechnologies = _mapper.Map<List<TechnologiesModel>>(this._context.Technologies.ToList());

            return View(screeningViewModel);
        }

        // POST: Screening/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScreeningId,UserId,IsJobDescOptSelected,IsCustomQuestionOptSelected,JobDesc,CandidateName,CandidateEmail,CandidatePhone,ScreeningStatus,CandidateSignInCode,CreatedOn,CreatedBy,LastUpdated,LastUpdatedBy")] Screening screening)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Claims.First(x => x.Type == "emails").Value;
                var userId = this.screeningRepository.GetUserId(userEmail);
                screening.UserId = userId;
                //screening.ScreeningStatus = "Pending";
                screening.CreatedOn = DateTime.Now;
                screening.CreatedBy = userEmail;
                //screening.CandidateSignInCode = Guid.NewGuid().ToString();
                _context.Add(screening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", screening.UserId);
            return View(screening);
        }

        // GET: Screening/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", screening.UserId);
            return View(screening);
        }

        // POST: Screening/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreeningId,UserId,IsJobDescOptSelected,IsCustomQuestionOptSelected,JobDesc,CandidateName,CandidateEmail,CandidatePhone,ScreeningStatus,CandidateSignInCode,CreatedOn,CreatedBy,LastUpdated,LastUpdatedBy")] Screening screening)
        {
            if (id != screening.ScreeningId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.ScreeningId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", screening.UserId);
            return View(screening);
        }

        // GET: Screening/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // POST: Screening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screening.FindAsync(id);
            _context.Screening.Remove(screening);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screening.Any(e => e.ScreeningId == id);
        }
    }
}
