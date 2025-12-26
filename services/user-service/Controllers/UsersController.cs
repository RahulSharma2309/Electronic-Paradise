using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // POST api/users/{id}/wallet/debit
        [HttpPost("{id:guid}/wallet/debit")]
        public async Task<IActionResult> DebitWallet(Guid id, [FromBody] WalletOperationDto dto)
        {
            if (dto.Amount <= 0) return BadRequest(new { error = "Amount must be > 0" });
            try
            {
                var balance = await _service.DebitWalletAsync(id, dto.Amount);
                return Ok(new { id, balance });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // POST api/users/{id}/wallet/credit
        [HttpPost("{id:guid}/wallet/credit")]
        public async Task<IActionResult> CreditWallet(Guid id, [FromBody] WalletOperationDto dto)
        {
            if (dto.Amount <= 0) return BadRequest(new { error = "Amount must be > 0" });
            try
            {
                var balance = await _service.CreditWalletAsync(id, dto.Amount);
                return Ok(new { id, balance });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // GET api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        // GET api/users/by-userid/{userId}
        [HttpGet("by-userid/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var res = await _service.GetByUserIdAsync(userId);
            if (res == null) return NotFound();
            return Ok(res);
        }

        // GET api/users/phone-exists/{phoneNumber}
        [HttpGet("phone-exists/{phoneNumber}")]
        public async Task<IActionResult> PhoneNumberExists(string phoneNumber)
        {
            var exists = await _service.PhoneNumberExistsAsync(phoneNumber);
            return Ok(new { exists });
        }

        // POST api/users/add-balance - Add balance using userId (Guid)
        [HttpPost("add-balance")]
        public async Task<IActionResult> AddBalance([FromBody] AddBalanceDto dto)
        {
            if (dto.UserId == Guid.Empty)
                return BadRequest(new { error = "UserId is required" });
            
            if (dto.Amount <= 0)
                return BadRequest(new { error = "Amount must be greater than 0" });

            try
            {
                var profile = await _service.GetByUserIdAsync(dto.UserId);
                if (profile == null)
                    return NotFound(new { error = "User profile not found" });

                var balance = await _service.CreditWalletAsync(profile.Id, dto.Amount);
                return Ok(new { 
                    userId = dto.UserId, 
                    balance,
                    message = $"Successfully added ₹{dto.Amount} to wallet. New balance: ₹{balance}" 
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "User profile not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding balance for userId {UserId}", dto.UserId);
                return StatusCode(500, new { error = "Failed to add balance. Please try again." });
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (dto.UserId == Guid.Empty) return BadRequest("UserId is required to create a profile.");
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CreateUserDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

    }
}
