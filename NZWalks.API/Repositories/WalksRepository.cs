using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext context;

        public WalksRepository(NZWalksDbContext context)
        {
            this.context = context;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //assign the new id as we are not taking the id from the client
            walk.Id = Guid.NewGuid();
            await context.Walks.AddAsync(walk);
            await context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            //delete the region from the DB
            else
            {
                context.Walks.Remove(existingWalk);
                await context.SaveChangesAsync();
            }
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
          return await context.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }
        public async Task<Walk> GetAsync(Guid id)
        {
            return await context.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            context.Walks.Update(existingWalk);
            await context.SaveChangesAsync();
            return existingWalk;
        }
    }
}
