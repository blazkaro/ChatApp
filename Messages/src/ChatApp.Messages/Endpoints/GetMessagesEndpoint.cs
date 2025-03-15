using ChatApp.Messages.Dtos.Requests;
using ChatApp.Messages.Dtos.Responses;
using ChatApp.Messages.Repositories;
using ChatApp.Messages.Services;

namespace ChatApp.Messages.Endpoints;

public static class GetMessagesEndpoint
{
    public static async Task<IResult> GetAsync(GetMessagesReqDto dto, IAccessTokenService atService,
        IConversationAccessCheckService accessCheckService, IMessageRepository messagesRepo,
        CancellationToken cancellationToken)
    {
        var at = await atService.GetCurrentAccessTokenAsync(cancellationToken);
        if (string.IsNullOrEmpty(at))
            return Results.Unauthorized();

        var authorized = await accessCheckService.HasAccesAsync(dto.ConversationId, at, cancellationToken);
        if (!authorized)
            return Results.Forbid();

        return Results.Ok(
            (await messagesRepo.GetLastMessagesUpToDateAsync(dto.ConversationId, dto.UpTo, cancellationToken))
                .Select(msg => new GetMessageResDto(msg.Id, msg.Content, msg.SenderId, msg.ConversationId, msg.SentAt))
            );
    }
}
