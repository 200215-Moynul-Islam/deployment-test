namespace ELTBackend.Constants
{
    public static class BusinessErrorMessages
    {
        public const string UserEmailAlreadyExists =
            "A user with the provided email already exists.";
        public const string UserInvalidEmail = "The provided email is incorrect.";
        public const string UserInvalidPassword = "The provided password is incorrect.";
        public const string UserNotFound = "User does not exist.";
        public const string InternalServerError =
            "An unexpected error occurred. Please try again later.";
        public const string PendingLeaveNotFound = "Either Leave does not exists or not pending.";
    }
}
