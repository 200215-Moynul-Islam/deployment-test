namespace ELTBackend.Constants
{
    public static class ValidationConstants
    {
        public static class User
        {
            public const int MaxNameLength = 100;
            public const string NameRegex = @"^[A-Z][a-z]*(?: [A-Z][a-z]*)*$";
            public const int MaxEmailLength = 254;
            public const int MaxRoleLength = 50;
            public const int MinPasswordLength = 8;
            public const int MaxPasswordLength = 64;
            public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$";
        }

        public static class Leave
        {
            public const int MaxStatusLength = 50;
        }
    }
}
