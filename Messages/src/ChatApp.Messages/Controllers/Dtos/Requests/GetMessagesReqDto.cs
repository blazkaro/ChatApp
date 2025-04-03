using System.ComponentModel.DataAnnotations;

namespace ChatApp.Messages.Controllers.Dtos.Requests;

public record GetMessagesReqDto([Range(1, int.MaxValue)] int? page)
{
}
