using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Application.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace finShark_demo.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var response = await _userService.GetAllUsersAsync();
            return StatusCode(response.StatusCode, response);
            // return Ok(users);
        }

        [HttpGet("users/by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("users/{gid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetByGId(Guid gid)
        {
            var response = await _userService.GetUserByGIdAsync(gid);
            return StatusCode(response.StatusCode, response);
            // return Ok(user);
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
            // return NoContent();
        }
    }
}