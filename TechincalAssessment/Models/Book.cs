using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechincalAssessment.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("PublisherId")]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [ForeignKey("AuthorId")]
        public int? AuthorId { get; set; }
        public Author Author { get; set; }

        [NotMapped] // This property won't be stored in the database
        public string MlaCitation
        {
            get
            {
                if (Author != null && Publisher != null)
                {
                    string authorName = $"{Author.LastName},{Author.FirstName}";
                    string title = Title;
                    string publisher = Publisher.Name;
                    string citation = $"{authorName}. \"{title}\", {publisher}.";
                    return citation;
                }
                return null;
            }
        }
        [NotMapped]
        public string ChicagoCitation
        {
            get
            {
                if (Author != null && Publisher != null)
                {
                    string authorName = $"{Author.FirstName} {Author.LastName}";
                    string title = Title;
                    string publisher = Publisher.Name;
                    // Build the Chicago style citation
                    string citation = $"{authorName}, \"{title},\" {publisher}.";

                    return citation;
                }

                return null;
            }
        }
    }
}
