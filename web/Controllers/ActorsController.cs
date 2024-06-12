using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using web.Data;
using web.Models.Domain;
using web.Repositories;

namespace web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ActorsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IActorRepository _actorRepository;

        public ActorsController(DataDbContext dbContext, IActorRepository actorRepository)
        {
            _dbContext = dbContext;
            _actorRepository = actorRepository;
        }

        [HttpGet("Get-All-Actors")]
        //[Authorize(Roles = "Read, Write")]
        public IActionResult GetAllActors()
        {
            var allActors = _actorRepository.GetAllActors();
            return Ok(allActors);
        }

        [HttpGet("Get-Actor-By-Id/{id}")]
        //[Authorize(Roles = "Read, Write")]
        public IActionResult GetActorById(int id)
        {
            var actorWithId = _actorRepository.GetActorById(id);
            return Ok(actorWithId);
        }

        [HttpPost("Add-Actor")]
        //[Authorize(Roles = "Write")]
        public IActionResult AddActor([FromBody] ActorDTO actorDTO)
        {
            var actorAdd = _actorRepository.AddActor(actorDTO);
            return Ok(actorAdd);
        }

        [HttpPut("Update-Actor-By-Id/{id}")]
        //[Authorize(Roles = "Write")]
        public IActionResult UpdateActorById(int id, [FromBody] ActorDTO actorDTO)
        {
            var updateActor = _actorRepository.UpdateActorById(id, actorDTO);
            return Ok(updateActor);
        }

        [HttpDelete("Delete-Actor-By-Id/{id}")]
        //[Authorize(Roles = "Write")]
        public IActionResult DeleteActorById(int id)
        {
            var deleteActor = _actorRepository.DeleteActorById(id);
            return Ok(deleteActor);
        }
    }
}
