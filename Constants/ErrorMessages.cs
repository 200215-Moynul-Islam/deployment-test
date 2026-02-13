namespace ELTBackend.Constants
{
    public static class ErrorMessages
    {
        public static class User
        {
            public const string NameRequired = "Name is required.";
            public const string NameMaxLengthExceeded = "Name cannot exceed 100 characters.";
            public const string InvalidNameFormat =
                "Name must start with a capital letter for each word and contain only lowercase letters after.";
            public const string EmailRequired = "Email is required.";
            public const string EmailMaxLengthExceeded = "Email cannot exceed 254 characters.";
            public const string InvalidEmailFormat = "Email format is invalid.";
            public const string PasswordRequired = "Password is required.";
            public const string PasswordLengthOutOfRange =
                "Password must be between 8 and 64 characters long.";
            public const string InvalidPasswordFormat =
                "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        }

        public static class Leave
        {
            public const string StartDateRequired = "Start date is required.";
            public const string EndDateRequired = "End date is required.";
            public const string EndDateMustBeOnOrAfterStartDate =
                "End date must be on or after start date";
            public const string UserIdRequired = "User id is required for a leave.";
        }
    }
}
