using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalksDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            //Fetch data from the database - Domain walks
            var walksdifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            //convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.WalksDifficulty>>(walksdifficultyDomain);

            //return reponse
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            //Fetch data from the database - Domain walks
            var walksdifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            if (walksdifficultyDomain == null)
            {
                return NotFound();
            }
            //convert domain walks object to DTO walks
            var walksdifficultyDTO = mapper.Map<Models.DTO.WalksDifficulty>(walksdifficultyDomain);

            //return reponse
            return Ok(walksdifficultyDomain);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            //pass domain object  to repository to persist this
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //convert this domain object back to DTO
            var WalkDifficultyDTO = new Models.DTO.WalksDifficulty()
            {
                Id= walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code
            };

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = WalkDifficultyDTO.Id }, WalkDifficultyDTO);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateWalkDifficultyAsync([FromRoute] Guid id,
           [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //pass details to repository -Get domain object in response(or null)
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            //handle null(not found)
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //convet back domain to DTO
            var WalkDifficultyDTO = new Models.DTO.WalksDifficulty()
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code
            };

            //return response
            return Ok(WalkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultysAsync(Guid id)
        {
            //call repository to delete walk
            var domainWalkdifficulty = await walkDifficultyRepository.DeleteAsync(id);
            if (domainWalkdifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalksDifficulty>(domainWalkdifficulty);
            return Ok(walkDifficultyDTO);
        }
    }
}
