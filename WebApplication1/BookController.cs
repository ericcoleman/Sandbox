using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class BookController : Controller
    {
        [Route("/book/{genre}")]
        public IActionResult AddBook(string genre, [FromForm] Book book)
        {
            return Content($"General genre: {book.Genre}, "
                           + $"name: {book.Name}, page count: {book.PageCount}, "
                           + $"book genre: {book.Genre}");
        }
    }
    
    public class Book
    {
        public string Genre { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
    }
}