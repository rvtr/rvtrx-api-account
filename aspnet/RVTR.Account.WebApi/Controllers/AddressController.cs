using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVTR.Account.DataContext.Repositories;
using RVTR.Account.ObjectModel.Models;
using RVTR.Account.WebApi.ResponseObjects;

namespace RVTR.Account.WebApi.Controllers
{
  /// <summary>
  /// Represents the _Address Component_ class
  /// </summary>
  [ApiController]
  [ApiVersion("0.0")]
  [EnableCors("Public")]
  [Route("rest/account/{version:apiVersion}/[controller]")]
  public class AddressController : ControllerBase
  {
    private readonly ILogger<AddressController> _logger;
    private readonly UnitOfWork _unitOfWork;

    /// <summary>
    /// The _Address Component_ constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public AddressController(ILogger<AddressController> logger, UnitOfWork unitOfWork)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Delete a user's address
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        if (_logger != null)
        {
          _logger.LogDebug("Deleting an address by its ID number...");
        }
        await _unitOfWork.Address.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
        if (_logger != null)
        {
          _logger.LogInformation($"Deleted the address.");

        }
        return Ok(MessageObject.Success);
      }
      catch
      {
        if (_logger != null)
        {
          _logger.LogWarning($"Address with ID number {id} does not exist.");
        }
        return NotFound(new ErrorObject($"Address with ID number {id} does not exist."));
      }


    }


    /// <summary>
    /// Get all addresses
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AddressModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {

      if (_logger != null)
      {
        _logger.LogInformation($"Retrieved the addresses.");
      }
      return Ok(await _unitOfWork.Address.SelectAsync());

    }


    /// <summary>
    /// Get a user's address with address ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
      AddressModel addressModel;

      if (_logger != null)
      {
        _logger.LogDebug("Getting an address by its ID number...");
      }
      addressModel = await _unitOfWork.Address.SelectAsync(id);


      if (addressModel is AddressModel theAddress)
      {
        if (_logger != null)
        {
          _logger.LogInformation($"Retrieved the address with ID: {id}.");
        }
        return Ok(theAddress);
      }
      if (_logger != null)
      {
        _logger.LogWarning($"Address with ID number {id} does not exist.");
      }
      return NotFound(new ErrorObject($"Address with ID number {id} does not exist."));
    }


    /// <summary>
    /// Add an address to an account
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(AddressModel address)
    {
      if (_logger != null)
      {
        _logger.LogDebug("Adding an address...");
      }
      await _unitOfWork.Address.InsertAsync(address);
      await _unitOfWork.CommitAsync();

      if (_logger != null)
      {
        _logger.LogInformation($"Successfully added the address {address}.");
      }
      return Ok(MessageObject.Success);
    }

    /// <summary>
    /// Update a user's address
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(AddressModel address)
    {
      try
      {
        if (_logger != null)
        {
          _logger.LogDebug("Updating an address...");
        }
        _unitOfWork.Address.Update(address);
        await _unitOfWork.CommitAsync();

        if (_logger != null)
        {
          _logger.LogInformation($"Successfully updated the address {address}.");
        }
        return Ok(MessageObject.Success);

      }
      catch
      {
        {
          if (_logger != null)
          {
            _logger.LogWarning($"This address does not exist.");
          }
          return NotFound(new ErrorObject($"Address with ID number {address.Id} does not exist."));
        }
      }

    }

  }
}