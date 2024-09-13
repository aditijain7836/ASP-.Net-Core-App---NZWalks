using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
namespace NZWalks.API;

// https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsController: ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }

    //Get all regions
    [HttpGet]
    public async Task<IActionResult> GetAllRegions(){
        //Get data from database - Domain Models
        var regionsDomain = await regionRepository.GetAllAsync();

        //Map domain models to dto
        var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

        return Ok(regionsDto);
    }

    //Get region by id
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id){
        var region = await regionRepository.GetByIdAsync(id);

        if(region == null)
        {
            return NotFound();
        }
        return Ok(region);
    }

    //Create new region
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto){
        //Map or Convert to Domain model
        var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
    
        //Use Domain model to create region in db
        await regionRepository.CreateAsync(regionDomainModel);

        //Map Domain model back to dto
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
    }

    //Update region
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto){
        //Map dto to domain model
        var region = mapper.Map<Region>(updateRegionRequestDto);

        //Check if region exists
        var regionDomainModel = await regionRepository.UpdateAsync(id, region);

        if(regionDomainModel == null)
        {
            return NotFound();
        }

        //Convert domain model to DTO
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return Ok(regionDto);
    }

    //Delete Region
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        //Check if region exists and delete
        var regionDomainModel = await regionRepository.DeleteAsync(id);

        if(regionDomainModel == null)
        {
            return NotFound();
        }

        //retrn deleted region back
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return Ok(regionDto);
    }
}
