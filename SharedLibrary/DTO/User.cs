namespace SharedLibrary.DTO
{
    public class User
    {
#pragma warning disable
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizeName { get; set; }
		public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
		public string? PhoneNumber { get; set; }
        public DateOnly? CreateDate { get; set; }
        public bool IsLock { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        User, Admin
    }
}

