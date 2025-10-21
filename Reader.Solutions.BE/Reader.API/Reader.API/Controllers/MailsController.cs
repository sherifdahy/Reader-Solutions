using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reader.BLL.Interfaces;
using Reader.Entities.Abstractions.Consts;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = DefaultRoles.Admin)]
public class MailsController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailsController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string email, string senderEmail = "content-creation@jeenie.com", int minutesAgo = 10, int maxCount = 16)
    {
        try
        {
            var result = await _mailService.GetRecentMessagesFormattedAsync(email, senderEmail, minutesAgo, maxCount);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Content(result.Value, "text/plain; charset=utf-8");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"خطأ: {ex.Message}");
        }
    }
}
