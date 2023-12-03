using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechincalAssessment.Models
{ 
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
