using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SubjectCRUD.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        [Required]
        public string NameSubject { get; set; }
        public int Credits {  get; set; }
        public int TeacherId { get; set; }
        [JsonIgnore]
        public Teacher? Teacher { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
    }
}
