using ChatApp.Conversations.Controllers.Dtos.Requests;
using ChatApp.Conversations.Controllers.Dtos.Responses;
using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Conversations.Controllers;

[Route("")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConversationsController : ControllerBase
{
    private readonly ConversationsDbContext _dbContext;

    public ConversationsController(ConversationsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserConversations(CancellationToken cancellationToken)
    {
        return Ok(
            await _dbContext.Conversations.AsNoTracking()
                .Where(p => p.Members.Any(p => p.UserId == User.GetUserId()))
                .Select(p => new GetConversationDto(p.Id, p.Name)).ToListAsync(cancellationToken)
            );
    }

    [HttpPost]
    public async Task<IActionResult> CreateConversationAsync([FromBody] CreateConversationDto dto, CancellationToken cancellationToken)
    {
        var conversation = new Conversation
        {
            Name = dto.Name,
            Members =
            [
                new ConversationMember
                {
                    UserId = User.GetUserId(),
                    IsAdmin = true
                }
            ]
        };

        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Created(conversation.Id.ToString(), new ConversationCreatedDto(conversation.Id));
    }
}
