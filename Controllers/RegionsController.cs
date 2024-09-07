using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
namespace NZWalks.API;

// https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsController: ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    public RegionsController(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAllRegions(){
        var regions = dbContext.Regions.ToList();
        return Ok(regions);
    }
}
