using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walksRepository;
        private readonly IMapper mapper;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from the database - Domain walks
            var walksDomain = await walksRepository.GetAllAsync();

            //convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return reponse
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalksAsync")]
        public async Task<IActionResult> GetWalksAsync(Guid id)
        {
            //Get domain object from the DB
            var walksDomain = await walksRepository.GetAsync(id);

            if (walksDomain == null)
            {
                return NotFound();
            }
            //convert domain walks object to DTO walks
            var walksDTO = mapper.Map<Models.DTO.Walk>(walksDomain);

            //return reponse
            return Ok(walksDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalksAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            //convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //pass domain object  to repository to persist this
            walkDomain = await walksRepository.AddAsync(walkDomain);

            //convert this domain object back to DTO
            var WalkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalksAsync), new { id = WalkDTO.Id }, WalkDTO);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //pass details to repository -Get domain object in response(or null)
            walkDomain = await walksRepository.UpdateAsync(id, walkDomain);

            //handle null(not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            //convet back domain to DTO
            var WalkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //return response
            return Ok(WalkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalksAsync(Guid id)
        {
            //call repository to delete walk
            var domainWalk = await walksRepository.DeleteAsync(id);
            if (domainWalk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(domainWalk);
            return Ok(walkDTO);
        }
    }
}
