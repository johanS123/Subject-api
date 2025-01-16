namespace SubjectCRUD.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string NameStudent { get; set; }
        public ICollection<Registration>? Registrations { get; set; }

    }
}
