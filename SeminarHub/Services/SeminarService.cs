using Microsoft.EntityFrameworkCore;
using SeminarHub.Contracts;
using SeminarHub.Data;
using SeminarHub.Data.DataModels;
using SeminarHub.Models;
using System.Globalization;
using static SeminarHub.Data.Common.DataConstants;

namespace SeminarHub.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly SeminarHubDbContext context;

        public SeminarService(SeminarHubDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<DeleteSeminarViewModel> CreateDeleteModelAsync(int id)
        {
            var seminarToDelete = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new DeleteSeminarViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    OrganizerId = s.OrganizerId,
                })
                .FirstAsync();

            return seminarToDelete;
        }

        public async Task<DetailsSeminarViewModel> CreateDetailedSeminar(int id)
        {
            var seminarModel = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new DetailsSeminarViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Details = s.Details,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Category = s.Category.Name,
                    Organizer = s.Organizer.UserName,
                    Duration = s.Duration
                })
                .FirstAsync();

            return seminarModel;
        }

        public async Task<AddOrEditSeminarFormModel> CreateEditModelAsync(int id)
        {
            var seminarModel = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new AddOrEditSeminarFormModel()
                {
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Details = s.Details,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Duration = s.Duration,
                    CategoryId = s.CategoryId,
                })
                .FirstAsync();

            return seminarModel;
        }

        public async Task CreateSeminarEntityAsync(string userId, AddOrEditSeminarFormModel model)
        {
            var entityToAdd = new Seminar()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                OrganizerId = userId,
                DateAndTime = DateTime.ParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture),
                Duration = model.Duration,
                CategoryId = model.CategoryId
            };

            await context.Seminars.AddAsync(entityToAdd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSeminarAsync(int id)
        {
            var seminarToDelete = await context.Seminars
                .FirstAsync(s => s.Id == id);

            context.Seminars.Remove(seminarToDelete);
            await context.SaveChangesAsync();
        }

        public async Task EditEntityAsync(int id, AddOrEditSeminarFormModel model)
        {
            var entityToEdit = await context.Seminars
                .FirstAsync(s => s.Id == id);

            entityToEdit.Topic = model.Topic;
            entityToEdit.Lecturer = model.Lecturer;
            entityToEdit.Details = model.Details;
            entityToEdit.Duration = model.Duration;
            entityToEdit.CategoryId = model.CategoryId;
            entityToEdit.DateAndTime = DateTime.ParseExact(model.DateAndTime, DateTimeFormat, CultureInfo.InvariantCulture);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllSeminarViewModel>> GetAllSeminarsAsync()
        {
            var seminars = await context.Seminars
                .AsNoTracking()
                .Select(s => new AllSeminarViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime.ToString(DateTimeFormat),
                    Organizer = s.Organizer.UserName
                })
                .ToListAsync();

            return seminars;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<JoinedSeminarViewModel>> GetJoinedSeminarsAsync(string userId)
        {
            var joinedSeminars = await context.SeminarsParticipants
                .AsNoTracking()
                .Where(sp => sp.ParticipantId == userId)
                .Select(sp => new JoinedSeminarViewModel()
                {
                    Id = sp.Seminar.Id,
                    Topic = sp.Seminar.Topic,
                    Lecturer = sp.Seminar.Lecturer,
                    DateAndTime = sp.Seminar.DateAndTime.ToString(DateTimeFormat),
                    Organizer = sp.Seminar.Organizer.UserName
                })
                .ToListAsync();

            return joinedSeminars;
        }

        public async Task<bool> IsSeminarExistingAsync(int id)
        {
            var seminar = await context.Seminars
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seminar == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsUserAuthorizedAsync(int id, string userId)
        {
            var seminar = await context.Seminars
                .AsNoTracking()
                .FirstAsync(s => s.Id == id);

            if (userId != seminar.OrganizerId)
            {
                return false;
            }

            return true;
        }

        public async Task JoinSeminarAsync(int id, string userId)
        {
            if (!context.SeminarsParticipants.Any(sp => sp.SeminarId == id && sp.ParticipantId == userId))
            {
                var seminarToJoin = await context.Seminars
                .AsNoTracking()
                .FirstAsync(s => s.Id == id);

                await context.SeminarsParticipants.AddAsync(new SeminarParticipant()
                {
                    SeminarId = seminarToJoin.Id,
                    ParticipantId = userId
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task LeaveSeminarAsync(string userId, int id)
        {
            if (context.SeminarsParticipants.Any(sp => sp.ParticipantId == userId && sp.SeminarId == id))
            {
                var seminarToLeave = await context.SeminarsParticipants
                    .FirstAsync(sp => sp.ParticipantId == userId && sp.SeminarId == id);

                context.SeminarsParticipants.Remove(seminarToLeave);
                await context.SaveChangesAsync();
            }
        }
    }
}
