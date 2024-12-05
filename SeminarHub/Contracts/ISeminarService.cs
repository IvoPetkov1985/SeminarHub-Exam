using SeminarHub.Models;

namespace SeminarHub.Contracts
{
    public interface ISeminarService
    {
        Task<SeminarDeleteViewModel> CreateDeleteModelAsync(int id);

        Task<SeminarDetailsViewModel> CreateDetailsModelAsync(int id);

        Task<SeminarFormModel> CreateEditModelAsync(int id);

        Task CreateNewSeminarAsync(SeminarFormModel model, string userId);

        Task EditSeminarAsync(SeminarFormModel model, int id);

        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<SeminarAllViewModel>> GetAllJoinedSeminarsAsync(string userId);

        Task<IEnumerable<SeminarAllViewModel>> GetAllSeminarsAsync();

        Task<bool> IsSeminarAlreadyJoinedAsync(string userId, int id);

        Task<bool> IsSeminarExistingAsync(int id);

        Task<bool> IsUserAuthorizedAsync(string userId, int id);

        Task JoinSeminarAsync(string userId, int id);

        Task LeaveSeminarAsync(string userId, int id);

        Task RemoveSeminarAsync(int id);
    }
}
