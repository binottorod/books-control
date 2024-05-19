using BooksControl.Models;
using BooksControl.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksControl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) => _booksService = booksService;

    [HttpGet]
    public async Task<List<Book>> Get() => await _booksService.GetAsync().ConfigureAwait(true);

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _booksService.GetAsync(id).ConfigureAwait(true);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Book newBook)
    {
        if (newBook is null)
        {
            return BadRequest();
        }

        await _booksService.CreateAsync(newBook).ConfigureAwait(true);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book updatedBook)
    {
        if (updatedBook is null)
        {
            return BadRequest();
        }
        var book = await _booksService.GetAsync(id).ConfigureAwait(true);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateAsync(id, updatedBook).ConfigureAwait(true);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _booksService.GetAsync(id).ConfigureAwait(true);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.DeleteAsync(id).ConfigureAwait(true);

        return NoContent();
    }
}
