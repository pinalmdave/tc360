using System.Collections.Generic;
using TechScreen.DBEntities;
using TechScreen.Models;

namespace TechScreen.Services
{
    public interface IScreeningRepository
    {
        void AddNewScreening(Screening screening);

        int AddNewClient(Client client);

        int AddNewUser(User user);

        int GetUserId(string userEmail);

        bool IsNewClient(string email);

        void CancelScreening(int screeningId, string userId);

        List<Screening> GetUserScreening(string userEmail);

        List<ReviewerModel> GetReviewers();

        List<ScreeningQuestions> GetScreeningQuestions(string candidateCode);

        bool AssignCandidateToReviewer(int screeningId, int candidateId, int reviewerId, string userName);

        List<ScreeningModel> GetReviewerScreenings(int reviewerId);

        bool AssignScreeningToReviewer(int screeningId, int reviewerId, string userName);

        ScreeningModel GetScreeningDetailToCreateQuestions(int screeningId);

        bool CreateScreeningQuestions(int screeningId, string userEmail, List<ScreeningQuestionsModel> lstScreeningQuestionsModel);

        ScreeningModel GetScreeningDetailForUpdate(int screeningId);

        TransactionModel GetTransaction(int transactionId);

        bool UpdateTransaction(int transactionId, string status);
    }
}