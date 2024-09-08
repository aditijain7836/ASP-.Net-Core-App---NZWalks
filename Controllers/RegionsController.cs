using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
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

    //Get all regions
    [HttpGet]
    public IActionResult GetAllRegions(){
        //Get data from database - Domain Models
        var regionsDomain = dbContext.Regions.ToList();

        //Map domain models to dto
        var regionsDto = new List<RegionDto>();
        foreach(var region in regionsDomain){
            regionsDto.Add(new RegionDto(){
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }

        return Ok(regionsDto);
    }

    //Get region by id
    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetRegionById([FromRoute] Guid id){
        var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if(region == null)
        {
            return NotFound();
        }
        return Ok(region);
    }

    //Create new region
    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto){
        //Map or Convert to Domain model
        var regionDomainModel = new Region{
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        //Use Domain model to create region in db
        dbContext.Regions.Add(regionDomainModel);
        dbContext.SaveChanges();

        //Map Domain model back to dto
        var regionDto = new RegionDto{
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
    }

    //Update region
    [HttpPut]
    [Route("{id:Guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto){
        //Check if region exists
        var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if(regionDomainModel == null)
        {
            return NotFound();
        }

        //Map DTO to Domain model
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        dbContext.SaveChanges();

        //Convert domain model to DTO
        var regionDto = new RegionDto(){
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }

    //Delete Region
    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        //Check if region exists
        var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if(regionDomainModel == null)
        {
            return NotFound();
        }

        //Delete Region
        dbContext.Regions.Remove(regionDomainModel);
        dbContext.SaveChanges();

        //retrn deleted region back
        var regionDto = new RegionDto(){
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }
}
