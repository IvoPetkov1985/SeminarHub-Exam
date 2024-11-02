using SeminarHub.Data.DataModels;
using SeminarHub.Models;

namespace SeminarHub.Contracts
{
    public interface ISeminarService
    {
        Task<DeleteSeminarViewModel> CreateDeleteModelAsync(int id);

        Task<DetailsSeminarViewModel> CreateDetailedSeminar(int id);

        Task<AddOrEditSeminarFormModel> CreateEditModelAsync(int id);

        Task CreateSeminarEntityAsync(string userId, AddOrEditSeminarFormModel model);

        Task DeleteSeminarAsync(int id);

        Task EditEntityAsync(int id, AddOrEditSeminarFormModel model);

        Task<IEnumerable<AllSeminarViewModel>> GetAllSeminarsAsync();

        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<IEnumerable<JoinedSeminarViewModel>> GetJoinedSeminarsAsync(string userId);

        Task<bool> IsSeminarExistingAsync(int id);

        Task<bool> IsUserAuthorizedAsync(int id, string userId);

        Task JoinSeminarAsync(int id, string userId);

        Task LeaveSeminarAsync(string userId, int id);
    }
}
