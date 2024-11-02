namespace SeminarHub.Data.Common
{
    public static class DataConstants
    {
        // Seminar constants
        public const int SeminarTopicMinLength = 3;
        public const int SeminarTopicMaxLength = 100;

        public const int SeminarLecturerMinLength = 5;
        public const int SeminarLecturerMaxLength = 60;

        public const int SeminarDetailsMinLength = 10;
        public const int SeminarDetailsMaxLength = 500;

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string InvalidDateAndTime = "Invalid date and time.";
        public const string NotMatchedDateAndTime = "Date and time must be in format dd/MM/yyyy HH:mm";
        public const string InvalidCategory = "This category does not exist.";

        public const int SeminarMinDuration = 30;
        public const int SeminarMaxDuration = 180;

        // Category constants
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;
    }
}
