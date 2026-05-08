using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            return Ok(regions);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var regions = dbContext.Regions.FirstOrDefault(x=> x.Id == id);
            return Ok(regions);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionDto region)
        {
            if (ModelState.IsValid)
            {
                
                var regionDomainModel = new Region
                {
                    code = region.code,
                    Name = region.Name,
                    RegionImageURL = region.RegionImageURL
                };

                // Save to database
                dbContext.Regions.Add(regionDomainModel);
                dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var region = dbContext.Regions.Find(id);

            if (region == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(region);
            dbContext.SaveChanges();

            return Ok(region);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] AddRegionDto updatedRegion)
        {
            var existingRegion = dbContext.Regions.Find(id);

            if (existingRegion == null)
            {
                return NotFound();
            }

            // Update fields
            existingRegion.code = updatedRegion.code;
            existingRegion.Name = updatedRegion.Name;
            existingRegion.RegionImageURL = updatedRegion.RegionImageURL;

            dbContext.SaveChanges();

            return Ok(existingRegion);
        }
    }
}
