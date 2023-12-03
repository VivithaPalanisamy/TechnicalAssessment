namespace TechincalAssessment.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public PublisherDto Publisher { get; set; }
        public AuthorDto Author { get; set; }
    }
    public class PublisherDto
    {
        public string Name { get; set; }
    }

    public class AuthorDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
