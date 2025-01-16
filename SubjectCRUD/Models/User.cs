namespace SubjectCRUD.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Fullname { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public required string Email { get; set; }
    }
}
