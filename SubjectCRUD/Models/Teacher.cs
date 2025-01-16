namespace SubjectCRUD.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string NameTeacher { get; set; }
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
