using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using WebApiIntegrationTesting.DataAccess.Entities;
using Xunit;

namespace WebApiIntegrationTesting.IntegrationTests.ControllerTests
{
    public class BookReviewsControllerTests : BaseTest
    {
        [Fact]
        public async Task Post_WithValidData_SavesReview()
        {
            var newReview = new BookReview { Title = "NewTitle to testing DB", Rating = 4 };

            //_factory.ReviewRepositoryMock.Setup(r => r.Create(It.Is<BookReview>(b => b.Title == "NewTitle" && b.Rating == 4))).Verifiable();
            //_factory.ReviewRepositoryMock.Setup(r => r.SaveChanges()).Verifiable();

            var response = await _client.PostAsync("/BookReviews", JsonContent.Create(newReview));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //_factory.ReviewRepositoryMock.VerifyAll();

            //_dbContext();
        }

        [Fact]
        public async Task It_Always_ReturnsAllBooks()
        {
            //var mockReviews = new BookReview[]
            //{
            //    new(){ Id = 1, Title="A", Rating = 2 },
            //    new(){ Id = 2, Title="B", Rating = 3 }
            //}.AsQueryable();

            _dbContext.BookReviews.Add(new BookReview { Title = "A", Rating = 2 });
            _dbContext.BookReviews.Add(new BookReview { Title = "B", Rating = 3 });
            _dbContext.SaveChanges();
            //_factory.ReviewRepositoryMock.Setup(r => r.AllReviews).Returns(mockReviews);

            var response = await _client.GetAsync("/BookReviews");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var data = JsonConvert.DeserializeObject<IEnumerable<BookReview>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(2, data.Count());
        }
    }
}
