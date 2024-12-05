namespace SeminarHub.Data.Common
{
    public static class DataConstants
    {
        // Seminar constants:
        public const int TopicMinLength = 3;
        public const int TopicMaxLength = 100;

        public const int LecturerMinLength = 5;
        public const int LecturerMaxLength = 60;

        public const int DetailsMinLength = 10;
        public const int DetailsMaxLength = 500;

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string DateTimeRegex = @"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}$";
        public const string DateTimeFormatErrorMsg = "Invalid date and time format.";
        public const string DateTimeNotValidMsg = "Invalid date and time.";

        public const int DurationMin = 30;
        public const int DurationMax = 180;

        // Category constants:
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 50;

        public const string CategoryNotValidMsg = "This category does not exist.";

        // Names of actions and controllers:
        public const string AllAction = "All";
        public const string SeminarContr = "Seminar";
    }
}
