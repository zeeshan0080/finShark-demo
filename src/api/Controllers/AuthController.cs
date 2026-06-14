using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using finShark_demo.Application.DTOs.Auth;
using finShark_demo.Application.DTOs.User;
using finShark_demo.Application.Services.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace finShark_demo.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        // private readonly ILogger _logger;

        public AuthController(IUserService userService)
        {
            _userService = userService;
            // _logger = logger;
        }

        [HttpPost("auth/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            var response = await _userService.RegisterAsync(registerDto);
            return StatusCode(response.StatusCode, response);
            // return CreatedAtAction(nameof(GetProfile), null, response);
        }

        [HttpPost("auth/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userService.LoginAsync(loginDto);
            return StatusCode(response.StatusCode, response);
            // return Ok(response);
        }

        [Authorize]
        [HttpGet("auth/profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = await _userService.GetUserProfileAsync(userId);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("auth/profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserDto updateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var response = await _userService.UpdateUserAsync(userId, updateDto);
            return StatusCode(response.StatusCode, response);
            // return Ok(user);
        }

        [Authorize]
        [HttpPost("auth/change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var response = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            return StatusCode(response.StatusCode, response);
            // return Ok(new { message = "Password changed successfully" });
        }

        [HttpGet("auth/verify-email/{gid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> VerifyEmail(Guid gid)
        {
            var response = await _userService.VerifyEmailAsync(gid);
            return StatusCode(response.StatusCode, response);
            // return Ok(new { message = "Email verified successfully" });
        }

        [AllowAnonymous]
        [HttpPost("auth/refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            // var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            //     return Unauthorized();
            

            var response = await _userService.RefreshTokenAsync(refreshTokenDto);
            return StatusCode(response.StatusCode, response);
            // return Ok(response);
        }
    }
}