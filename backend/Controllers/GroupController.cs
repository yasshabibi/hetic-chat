using Backend.Managers;
using Backend.Models.Groups;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly GroupManager _groupManager;


        public class GroupCreationRequest
        {
            [Required]
            [StringLength(255, MinimumLength = 3)]
            public string Name { get; set; }

            [Required]
            public int CreatedBy { get; set; }

            [Required]
            public List<int> MemberUserIds { get; set; } = new List<int>();
        }

        public GroupController(GroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationRequest request)
        {
            var success = await _groupManager.CreateGroup(request.Name, request.MemberUserIds, request.CreatedBy);
            if (success)
                return Ok();
            else
                return BadRequest("Unable to create group.");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGroupByUserId(int userId)
        {
            var groups = await _groupManager.GetGroupByUserId(userId);
            return Ok(groups);
        }

        [HttpDelete("delete/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId, int userId)
        {
            var success = await _groupManager.DeleteGroupById(groupId, userId);
            if (success)
                return Ok();
            else
                return BadRequest("Unable to delete group or permission denied.");
        }

        [HttpPut("modify/{groupID}")]
        public async Task<IActionResult> ModifyGroup(int groupID, string name)
        {
            var success = await _groupManager.ModifyGroup(groupID, name);
            if (success)
                return Ok();
            else
                return BadRequest("Unable to modify group or permission denied.");
        }
    }

}
