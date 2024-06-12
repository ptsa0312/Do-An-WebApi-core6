using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using web.Models.Domain;
using web.Data;
using web.Repositories;

namespace web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
        public class FilmsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IFilmRepository _filmRepository;
        private readonly ILogger<FilmsController> _logger;

        public FilmsController(DataDbContext dbContext, IFilmRepository filmRepository, ILogger<FilmsController> logger)
        {
            _dbContext = dbContext;
            _filmRepository = filmRepository;
            _logger = logger;
        }

        [HttpGet("Get-All-Films")]
       // [Authorize(Roles = "Read, Write")]
        public IActionResult GetAllFilms([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            _logger.LogInformation("GetAll Film Action method was invoked");
            _logger.LogWarning("This is a warning log");
            _logger.LogError("This is a error log");

            var allFilms = _filmRepository.GetAllFilms(filterOn, filterQuery, sortBy, isAscending);

            _logger.LogInformation($"Finished GetAllFilm request with data{JsonSerializer.Serialize(allFilms)}");
            return Ok(allFilms);
        }

        [HttpGet]
        [Route("Get-Film-By-Id/{id}")]
       // [Authorize(Roles = "Read, Write")]
        public IActionResult GetFilmById([FromRoute] int id)
        {
            var filmWithId = _filmRepository.GetFilmById(id);
            return Ok(filmWithId);
        }

        [HttpPost("Add-Film")]
       // [Authorize(Roles = "Write")]
        public IActionResult AddFilm([FromBody] FilmDTO filmDTO)
        {
            if (!ValidateAddFilm(filmDTO))
                return BadRequest(ModelState);
            if (ModelState.IsValid)
            {
                var filmAdd = _filmRepository.AddFilm(filmDTO);
                return Ok(filmAdd);
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("Update-Film-By-Id/{id}")]
       // [Authorize(Roles = "Write")]
        public IActionResult UpdateFilmById(int id, [FromBody] FilmDTO filmDTO)
        {
            var updateFilm = _filmRepository.UpdateFilmById(id, filmDTO);
            return Ok(updateFilm);
        }

        [HttpDelete("Delete-Film-By-Id/{id}")]
      //  [Authorize(Roles = "Write")]
        public IActionResult DeleteFilmById(int id)
        {
            var deleteFilm = _filmRepository.DeleteFilmById(id);
            return Ok(deleteFilm);
        }

        #region Private methods

        private bool ValidateAddFilm(FilmDTO filmDTO)
        {
            if (filmDTO == null)
            {
                ModelState.AddModelError(nameof(filmDTO), $"Please add film data");
                return false;
            }
            if (string.IsNullOrEmpty(filmDTO.Description))
                ModelState.AddModelError(nameof(filmDTO.Description), $"{nameof(filmDTO.Description)} cannot be null");
            if (filmDTO.Rate < 0 || filmDTO.Rate > 5)
                ModelState.AddModelError(nameof(filmDTO.Rate), $"{nameof(filmDTO.Rate)} cannot be less than 0 and more than 5");
            if (ModelState.ErrorCount > 0)
                return false;
            return true;
        }

        #endregion Private methods
    }
}
