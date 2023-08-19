using System.ComponentModel.DataAnnotations;

namespace WebApiIntegrationTesting.DataAccess.Entities
{
    public class BookReview
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public double Rating { get; set; }
    }
}
