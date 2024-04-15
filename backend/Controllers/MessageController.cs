using Backend.Managers;
using Backend.Models.Groups;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageManager _messageManager;

    public MessageController(MessageManager messageManager)
    {
        _messageManager = messageManager;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMessage([FromBody] Message message)
    {
        var success = await _messageManager.AddMessage(message.GroupID, message.UserID, message.Content);
        if (success)
            return Ok();
        else
            return BadRequest("Unable to add message.");
    }

    [HttpGet("get/{groupId}")]
    public async Task<IActionResult> GetMessages(int groupId)
    {
        var messages = await _messageManager.GetMessages(groupId);
        if (messages != null)
            return Ok(messages);
        else
            return NotFound("No messages found.");
    }

    [HttpDelete("delete/{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId, int userId)
    {
        var success = await _messageManager.DeleteMessage(messageId, userId);
        if (success)
            return Ok();
        else
            return BadRequest("Unable to delete message or permission denied.");
    }
}
