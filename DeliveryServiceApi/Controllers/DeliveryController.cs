using Microsoft.AspNetCore.Mvc;
using DeliveryServiceApi.Models;
using DeliveryServiceApi.Services;
using DeliveryServiceApi.Repositories;



namespace DeliveryServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly AddressRepository _addressRepository;
        private readonly DistanceService _distanceService;

        public DeliveryController(AddressRepository addressRepository, DistanceService distanceService)
        {
            _addressRepository = addressRepository;
            _distanceService = distanceService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<DeliveryCharge>> CalculateDeliveryCharge([FromBody] (Address shopAddress, Address deliveryAddress) addresses)
        {
            var shopAddressId = await _addressRepository.AddAddressAsync(addresses.shopAddress);
            var deliveryAddressId = await _addressRepository.AddAddressAsync(addresses.deliveryAddress);

            var shopAddress = await _addressRepository.GetAddressByIdAsync(shopAddressId);
            var deliveryAddress = await _addressRepository.GetAddressByIdAsync(deliveryAddressId);

            var distance = _distanceService.CalculateDistance(
                shopAddress.Latitude,
                shopAddress.Longitude,
                deliveryAddress.Latitude,
                deliveryAddress.Longitude
            );

            var charge = _distanceService.CalculateDeliveryCharge(distance);

            return new DeliveryCharge
            {
                Distance = distance,
                Charge = charge
            };
        }

        [HttpPost("addresses")]
        public async Task<ActionResult<Address>> AddAddress([FromBody] Address address)
        {
            var id = await _addressRepository.AddAddressAsync(address);
            address.Id = id;
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpGet("addresses/{id}")]
        public async Task<ActionResult<Address>> GetAddressById(int id)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return address;
        }
    }
}


