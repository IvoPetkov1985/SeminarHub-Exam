using Microsoft.EntityFrameworkCore;
using SeminarHub.Contracts;
using SeminarHub.Data;
using SeminarHub.Data.DataModels;
using SeminarHub.Models;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly SeminarHubDbContext context;

        public SeminarService(SeminarHubDbContext _context)
        {
            context = _context;
        }

        public async Task<SeminarDeleteViewModel> CreateDeleteModelAsync(int id)
        {
            SeminarDeleteViewModel deleteModel = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SeminarDeleteViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat)
                })
                .SingleAsync();

            return deleteModel;
        }

        public async Task<SeminarDetailsViewModel> CreateDetailsModelAsync(int id)
        {
            SeminarDetailsViewModel detailsModel = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SeminarDetailsViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Details = s.Details,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Duration = s.Duration,
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName
                })
                .SingleAsync();

            return detailsModel;
        }

        public async Task<SeminarFormModel> CreateEditModelAsync(int id)
        {
            SeminarFormModel editModel = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SeminarFormModel()
                {
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Details = s.Details,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Duration = s.Duration,
                    CategoryId = s.CategoryId
                })
                .SingleAsync();

            return editModel;
        }

        public async Task CreateNewSeminarAsync(SeminarFormModel model, string userId)
        {
            Seminar seminar = new()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                DateAndTime = DateTime.Parse(model.DateAndTime),
                Duration = model.Duration,
                CategoryId = model.CategoryId,
                OrganizerId = userId
            };

            await context.Seminars.AddAsync(seminar);

            await context.SaveChangesAsync();
        }

        public async Task EditSeminarAsync(SeminarFormModel model, int id)
        {
            Seminar seminarToEdit = await context.Seminars
                .SingleAsync(s => s.Id == id);

            seminarToEdit.Topic = model.Topic;
            seminarToEdit.Lecturer = model.Lecturer;
            seminarToEdit.Details = model.Details;
            seminarToEdit.DateAndTime = DateTime.Parse(model.DateAndTime);
            seminarToEdit.Duration = model.Duration;
            seminarToEdit.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryViewModel> allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return allCategories;
        }

        public async Task<IEnumerable<SeminarAllViewModel>> GetAllJoinedSeminarsAsync(string userId)
        {
            IEnumerable<SeminarAllViewModel> joinedSeminars = await context.SeminarsParticipants
                .AsNoTracking()
                .Where(sp => sp.ParticipantId == userId)
                .Select(sp => new SeminarAllViewModel()
                {
                    Id = sp.Seminar.Id,
                    Topic = sp.Seminar.Topic,
                    Lecturer = sp.Seminar.Lecturer,
                    DateAndTime = sp.Seminar.DateAndTime.ToString(DateTimeFormat),
                    Category = sp.Seminar.Category.Name,
                    Organizer = sp.Seminar.Organizer.UserName
                })
                .ToListAsync();

            return joinedSeminars;
        }

        public async Task<IEnumerable<SeminarAllViewModel>> GetAllSeminarsAsync()
        {
            IEnumerable<SeminarAllViewModel> allSeminars = await context.Seminars
                .AsNoTracking()
                .Select(s => new SeminarAllViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName
                })
                .ToListAsync();

            return allSeminars;
        }

        public async Task<bool> IsSeminarAlreadyJoinedAsync(string userId, int id)
        {
            SeminarParticipant entryToCheck = new()
            {
                SeminarId = id,
                ParticipantId = userId
            };

            return await context.SeminarsParticipants.ContainsAsync(entryToCheck);
        }

        public async Task<bool> IsSeminarExistingAsync(int id)
        {
            Seminar? seminar = await context.Seminars
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id);

            return seminar != null;
        }

        public async Task<bool> IsUserAuthorizedAsync(string userId, int id)
        {
            Seminar seminar = await context.Seminars
                .AsNoTracking()
                .SingleAsync(s => s.Id == id);

            return seminar.OrganizerId == userId;
        }

        public async Task JoinSeminarAsync(string userId, int id)
        {
            SeminarParticipant entry = new()
            {
                SeminarId = id,
                ParticipantId = userId
            };

            if (await context.SeminarsParticipants.ContainsAsync(entry) == false)
            {
                await context.SeminarsParticipants.AddAsync(entry);

                await context.SaveChangesAsync();
            }
        }

        public async Task LeaveSeminarAsync(string userId, int id)
        {
            SeminarParticipant entry = new()
            {
                SeminarId = id,
                ParticipantId = userId
            };

            if (await context.SeminarsParticipants.ContainsAsync(entry))
            {
                context.SeminarsParticipants.Remove(entry);

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveSeminarAsync(int id)
        {
            Seminar seminarToRemove = await context.Seminars
                .SingleAsync(s => s.Id == id);

            context.Seminars.Remove(seminarToRemove);

            await context.SaveChangesAsync();
        }
    }
}
