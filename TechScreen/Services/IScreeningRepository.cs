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

    }
}