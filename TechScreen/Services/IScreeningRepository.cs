using System.Collections.Generic;
using TechScreen.DBEntities;
using TechScreen.Models;
using TechScreen.ViewModels;

namespace TechScreen.Services
{
    public interface IScreeningRepository
    {
        void AddNewScreening(Screening screening);

        int AddNewClient(Client client);

        int AddNewUser(User user);

        int GetUserId(string userEmail);

        CandidateScreeningInfoVM GetCandidateInfoForSignUp(string candidateSignInCode, string validateSignInCode);

        bool IsNewClient(string email);

        void CancelScreening(int screeningId, string userId);

        List<Screening> GetAllScreeningSummaryForAdmin();

        List<Screening> GetUserScreening(string email);

        List<ReviewerModel> GetReviewers();

        bool AssignCandidateToReviewer(int screeningId, int candidateId, int reviewerId, string userName);

        List<ScreeningModel> GetReviewerScreenings(int reviewerId);

        bool AssignScreeningToReviewer(int screeningId, int reviewerId, string userName);

        ScreeningModel GetScreeningDetailToCreateQuestions(int screeningId);

        bool CreateScreeningQuestions(int screeningId, string userEmail, List<ScreeningQuestionsModel> lstScreeningQuestionsModel);

        ScreeningModel GetScreeningDetailForUpdate(int screeningId);

        TransactionModel GetTransaction(int transactionId);

        bool UpdateTransaction(int transactionId, string status);

        bool CandiateScreeningCompleted(string candidateCode);

        List<ScreeningQuestions> GetScreeningQuestions(int candidateId, int screeningId);

        void SubmitScreeningFeedback(ScreeningCandidate screeningCandidate, List<DetailedCandidateScreening> detailedCandidateScreening);

        ScoreCardVM GetCandidateScoreCard(int candidateId);

        Screening GetScreeningDetailForAdmin(int screeningId);
    }
}