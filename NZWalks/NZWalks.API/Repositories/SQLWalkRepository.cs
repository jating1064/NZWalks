using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext dBContext;

        public SQLWalkRepository(NZWalksDBContext dBContext) {
            this.dBContext = dBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dBContext.Walks.AddAsync(walk);
            await dBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk= await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null) 
            {
                return null;            
            }
            dBContext.Walks.Remove(existingWalk);
            await dBContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null
            , string? sortBy = null, bool isAscending = true, int pageNumber = 1
            , int pageSize = 1000)
        {
            //retreive the walks as queryable instead of list so we can use it to implement sorting
            //and fitering 
            var walks= dBContext.Walks.Include("Difficulty").Include("Region")
                .AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false &&
                string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false){
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) :
                        walks.OrderByDescending(x => x.Name);

                }
                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthinKM):
                        walks.OrderByDescending(x => x.LengthinKM);
                }
            }

            //Pagination

            // skip 1st
            var skipResults = (pageNumber - 1) * pageSize;
            //fetch result now

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();


           // return await walks.ToListAsync();
            //return await dBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            //return await dBContext.Walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dBContext.Walks.Include("Difficulty")
                .Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk= await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingWalk == null)
            {
                return null;
            }

            existingWalk.Name=walk.Name;
            //existingWalk.Id=id;
            existingWalk.WalkImageURL=walk.WalkImageURL;
            existingWalk.Description=walk.Description;
            existingWalk.LengthinKM=walk.LengthinKM;
            existingWalk.DifficultyId=walk.DifficultyId;
            existingWalk.RegionId=walk.RegionId;

            await dBContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
