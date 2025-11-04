using ExampleWebService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExampleWebService.Controllers;

/// <summary>
/// Controller for managing a collection of words.
/// Provides endpoints to retrieve all words and add a new word.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Words")]
public class WordsController(IWordRepository wordRepository) : ControllerBase
{
    /// <summary>
    /// Retrieves the list of all stored words. DOCKER
    /// </summary>
    /// <returns>
    /// A JSON array containing all words.
    /// </returns>
    /// <response code="200">Returns the list of words.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [Produces("application/json")]
    [ActionName("GetAllWords")]
    public IActionResult GetAllWords()
    {
        return Ok(wordRepository.GetAll());
    }

    /// <summary>
    /// Adds a new word to the collection.
    /// </summary>
    /// <param name="value">The word to add. Must not be null or whitespace.</param>
    /// <returns>
    /// A message indicating success and the total number of words after addition.
    /// </returns>
    /// <response code="200">Returns a success message with the total count of words.</response>
    /// <response code="400">If the 'value' parameter is null, empty, or contains only whitespace.</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [ActionName("AddWord")]
    public IActionResult AddWord([FromQuery] string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return BadRequest("Query parameter 'value' is required and cannot be empty");

        var trimmedValue = value.Trim();
        wordRepository.Add(trimmedValue);
        return Ok($"Word '{trimmedValue}' added. Total words: {wordRepository.GetAll().Count()}");
    }
}
