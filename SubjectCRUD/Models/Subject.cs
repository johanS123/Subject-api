using System.ComponentModel.DataAnnotations;

namespace SubjectCRUD.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        [Required]
        public string NameSubject { get; set; }
        [Required]
        [EnumDataType(typeof(Modality))]
        public Modality Modality {  get; set; }
        public int Credits {  get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public ICollection<Registration>? Registrations { get; set; }

    }

    public enum Modality
    {
        Virtual,
        Presencial
    }
}
