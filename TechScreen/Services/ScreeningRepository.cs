using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.DBEntities;
using TechScreen.Models;
using AutoMapper;
using TechScreen.ViewModels;
using TechScreen.Utilities;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace TechScreen.Services
{
    public class ScreeningRepository : IScreeningRepository
    {
        private TechscreenDBContext context;
        private readonly IMapper _mapper;


        public ScreeningRepository(TechscreenDBContext _context, IMapper mapper)
        {
            this.context = _context;
            this._mapper = mapper;
        }

        public bool CreateScreeningQuestions(int screeningId, string userEmail, List<ScreeningQuestionsModel> lstScreeningQuestionsModel)
        {
            try
            {
                var screening = this.context.Screening.Include(sc=> sc.ScreeningCandidate).Include(sq=> sq.ScreeningQuestions).Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                screening.LastUpdated = DateTime.Now;
                screening.LastUpdatedBy = userEmail;
                screening.ReviewerStatus= EnumScreeningStatus.Screening_Invitation_Sent.ToString();
                screening.AdminStatus = EnumScreeningStatus.Screening_Invitation_Sent.ToString();
                screening.Status = EnumScreeningStatus.AwaitingCandidateResponse.ToString();

                foreach (var candidate in screening.ScreeningCandidate)
                {
                    candidate.ScreeningStatus = EnumScreeningStatus.AwaitingCandidateResponse.ToString();
                    candidate.LastUpdated = DateTime.Now;
                    candidate.LastUpdatedBy = userEmail;
                    candidate.CandidateSignInCode = Guid.NewGuid().ToString();
                }

                screening.ScreeningQuestions = this._mapper.Map(lstScreeningQuestionsModel , new List<ScreeningQuestions>());

                this.context.Screening.Update(screening);
                Save();

                //Send  email to candidates

                foreach(var candidate in screening.ScreeningCandidate)
                {
                    var sendGridMessage = new SendGridMessage();
                    sendGridMessage.SetSubject("Video Interview Request From " + screening.HiringCompanyName);
                    sendGridMessage.AddTo(candidate.CandidateEmail);
                    sendGridMessage.SetFrom(new EmailAddress("noreply@techscreen360.com", "TechScreen360"));
                    sendGridMessage.AddContent(MimeType.Html, "<p>Dear" +candidate.CandidateFirstName + " " + candidate.CandidateLastName +"" +
                        ",</br></br>" +
                        "You have received Video Interview Request from :"+ screening.HiringCompanyName + "</br></br>" +
                        "<b>Please follow below instruction to complete your video interview:</b></b>" +
                        "1. This is a video interview so make sure to test your laptop's webcam and audio device is working fine.</br>" +
                        "2. For best results, please use <b>latest Google Chrome Browser</b>" +
                        "3. <b>Click on the link below:</br></br>" +
                        "4. https://www.techscreen360.com/jobcandidate" +
                        "5.  Your sign in Code is :" + candidate.CandidateSignInCode + "  .The sign in code will expire 24 hours after you receive this email.</br></br>" +
                        "Best of luck for your interview. If you come across any issue, please contact InterviewSupport@techscreen360.com" +
                        "</p>"
                        );

                    EmailUtility.SendGridMessage(sendGridMessage).Wait();
                }
              

                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public ScreeningModel GetScreeningDetailToCreateQuestions(int screeningId)
        {
            try
            {
                var result = context.Screening.Include(r => r.ScreeningQuestions)
                    .Include(j => j.JobCat)
                    .Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                return this._mapper.Map(result, new ScreeningModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ScreeningModel GetScreeningDetailForUpdate(int screeningId)
        {
            try
            {
                var result = context.Screening
                    .Include(r => r.ScreeningQuestions)
                    .Include(sc=>sc.ScreeningCandidate)
                    .Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                return this._mapper.Map(result, new ScreeningModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ScreeningModel> GetReviewerScreenings(int reviewerId)
        {
            try
            {
                var lstScreening = context.Screening
                                            .Include(s => s.ScreeningCandidate)
                                            .Include(j => j.JobCat)
                                            .Where(x => x.ReviewerId == reviewerId).OrderByDescending(o=> o.CreatedOn).ToList();
                                 
                return this._mapper.Map(lstScreening, new List<ScreeningModel>());
            }

            catch(Exception ex)
            {
                return null;
            }
        }

        public List<ReviewerModel> GetReviewers()
        {
            var reviewers = this.context.Reviewer
                            .Include(rt => rt.ReviewerTechnologies).ToList();

            return this._mapper.Map(reviewers, new List<ReviewerModel>());
        }

        public void AddNewScreening(Screening screening)
        {
            this.context.Screening.Add(screening);
            Save();
        }

        public List<Screening> GetAllScreeningSummaryForAdmin()
        {   
            var lstUserScreening = context.Screening
                                            .Include(sc => sc.ScreeningCandidate)
                                            .Include(j=> j.JobCat)
                                            .Include(r=>r.Reviewer)
                                            .OrderByDescending(x=> x.CreatedOn).ToList();
            return lstUserScreening;
        }
        public Screening GetScreeningDetailForAdmin(int screeningId)
        {
            var screening = context.Screening
                                            .Include(sc => sc.ScreeningCandidate)
                                            .Include(j => j.JobCat)
                                            .Where(s => s.ScreeningId == screeningId)
                                            .OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            return screening;
        }

        public List<Screening> GetUserScreening(string email)
        {
            var userId = GetUserId(email);
            var lstUserScreening = context.Screening
                                            .Include(s => s.ScreeningCandidate)
                                            .Include(j => j.JobCat)
                                            .Where(x=> x.UserId == userId)
                                            .OrderByDescending(x => x.CreatedOn).ToList();
            return lstUserScreening;
        }

        public int GetUserId(string userEmail)
        {
            var userId = this.context.User.Where(x => x.UserEmail == userEmail).FirstOrDefault().UserId;
            return userId;
        }

        public bool AssignCandidateToReviewer(int screeningId, int candidateId, int reviewerId, string userName)
        {
            try
            {
                var screeningCandidate = this.context.ScreeningCandidate.Where(x => x.ScreeningId == screeningId && x.CandidateId == candidateId).FirstOrDefault();
                screeningCandidate.ReviewerId = reviewerId;

                screeningCandidate.LastUpdated = DateTime.Now;
                screeningCandidate.LastUpdatedBy = userName;
                this.context.ScreeningCandidate.Update(screeningCandidate);
                Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool AssignScreeningToReviewer(int screeningId, int reviewerId, string userName)
        {
            try
            {
                var screening = this.context.Screening.Where(x => x.ScreeningId == screeningId).Include(sc=> sc.ScreeningCandidate).FirstOrDefault();
                screening.ReviewerId = reviewerId;
                screening.LastUpdated = DateTime.Now;
                screening.LastUpdatedBy = userName;
                screening.Status = EnumScreeningStatus.InProcess.ToString();
                screening.AdminStatus = EnumScreeningStatus.Awaiting_Reviewer_Response.ToString();
                screening.ReviewerStatus = EnumScreeningStatus.Assigned.ToString();

                var screeningCandidates = screening.ScreeningCandidate.ToList(); 

                foreach(var candidate in screeningCandidates)
                {
                    candidate.ReviewerId = reviewerId;
                    candidate.LastUpdated = DateTime.Now;
                    candidate.LastUpdatedBy = userName;
                    candidate.ScreeningStatus = EnumScreeningStatus.InProcess.ToString();
                }

                this.context.Screening.Update(screening);

                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AssignScreening(int screeningId, int reviewerId, string userName)
        {
            try
            {
                var screeningCandidate = this.context.ScreeningCandidate.Where(x => x.ScreeningId == screeningId).FirstOrDefault();
                screeningCandidate.ReviewerId = reviewerId;

                screeningCandidate.LastUpdated = DateTime.Now;
                screeningCandidate.LastUpdatedBy = userName;
                this.context.ScreeningCandidate.Update(screeningCandidate);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public TransactionModel GetTransaction(int transactionId)
        {
            var transaction = this.context.Transaction.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            var transactionModel = this._mapper.Map(transaction, new TransactionModel());
            return transactionModel;
        }

        public bool UpdateTransaction(int transactionId, string status)
        {
            var transaction = this.context.Transaction.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            transaction.PaymentStatus = status;
            transaction.LastUpdated = DateTime.Now;
            this.context.Transaction.Update(transaction);
            var isSaved = Save();
            return isSaved;
        }

        public bool CandiateScreeningCompleted(string candidateCode)
        {
            try
            {
                var screeningCandidate = this.context.ScreeningCandidate.Where(x => x.CandidateSignInCode == candidateCode).FirstOrDefault();
                screeningCandidate.ScreeningStatus = EnumScreeningStatus.CandidateResponseReceived.ToString();
                screeningCandidate.CandidateSignInCode = "";

                this.context.ScreeningCandidate.Update(screeningCandidate);
                var isSaved = Save();
                return isSaved;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void CancelScreening(int screeningId, string userId)
        {
            var screening = this.context.Screening.Where(x => x.ScreeningId == screeningId).FirstOrDefault();
            //screening.ScreeningStatus = "Cancelled";
            screening.LastUpdated = DateTime.Now;
            screening.LastUpdatedBy = userId;
            this.context.Screening.Add(screening);
            Save();
        }

        public bool IsNewClient(string email)
        {
            var client = this.context.Client.Where(x => x.Email == email).FirstOrDefault();
            if (client == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int AddNewClient(Client client)
        {
            this.context.Add(client);
            context.SaveChanges();
            var clientId = client.ClientId;
            return clientId;
        }

        public int AddNewUser(User user)
        {
            this.context.Add(user);
            context.SaveChanges();
            var userId = user.UserId;
            return userId;
        }

        public bool Save()
        {
            return (this.context.SaveChanges() >= 0);
        }

        public CandidateScreeningInfoVM GetCandidateInfoForSignUp(string candidateSignInCode, string validateSignInCode)
        {
            var screeningCandidate = context.ScreeningCandidate.Where(x => x.CandidateSignInCode == candidateSignInCode.Trim()).FirstOrDefault();

            if (screeningCandidate == null) return null;

            var vm = new CandidateScreeningInfoVM();

            if (validateSignInCode=="Y")
            {
                var lstScreeningQuestions = context.ScreeningQuestions.Where(x => x.ScreeningId == screeningCandidate.ScreeningId).ToList();
                vm.ScreeningQuestions = lstScreeningQuestions;
            }
            else
            {   
                vm.CandidateId = screeningCandidate.CandidateId.ToString();
                vm.ScreeningId = screeningCandidate.ScreeningId.ToString();
            }
            return vm;
        }

        public List<ScreeningQuestions> GetScreeningQuestions(int candidateId, int screeningId)
        {
            var questions = context.ScreeningQuestions.Where(x => x.ScreeningId == screeningId)
                .OrderBy(q=>q.QuestionId)
                .ToList();
            return questions;
        }

        public void SubmitScreeningFeedback(ScreeningCandidate screeningCandidate, List<DetailedCandidateScreening> detailedCandidateScreening)
        {
            try
            {
                var candidate = context.ScreeningCandidate.Where(x => x.CandidateId == screeningCandidate.CandidateId).FirstOrDefault();

                candidate.OverallScore = screeningCandidate.OverallScore;
                candidate.TechnicalCommunication = screeningCandidate.TechnicalCommunication;
                candidate.VerbalCommunication = screeningCandidate.VerbalCommunication;
                candidate.CandidateEnthusiasm = screeningCandidate.CandidateEnthusiasm;
                candidate.ReviewerComments = screeningCandidate.ReviewerComments;
                candidate.ScreeningStatus = EnumScreeningStatus.Completed.ToString();

                context.ScreeningCandidate.Update(candidate);
                context.SaveChanges();

                context.DetailedCandidateScreening.AddRange(detailedCandidateScreening);
                context.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }

        public ScoreCardVM GetCandidateScoreCard(int candidateId)
        {
            var candidate = this.context.ScreeningCandidate.Where(x => x.CandidateId == candidateId).FirstOrDefault();
            var lstDetailedCandidateScreening = this.context.DetailedCandidateScreening.Where(x => x.CandidateId == candidateId).ToList();

            var vm = new ScoreCardVM();
            vm.lstDetailedCandidateScreening = lstDetailedCandidateScreening;
            vm.OverallScore = candidate.OverallScore;
            vm.TechnicalCommunication = candidate.TechnicalCommunication;
            vm.VerbalCommunication = candidate.VerbalCommunication;
            vm.CandidateEnthusiasm = candidate.CandidateEnthusiasm;
            vm.ReviewerComments = candidate.ReviewerComments;
            vm.FeedbackDate = candidate.LastUpdated.ToString();

            return vm;
        }
    }
}