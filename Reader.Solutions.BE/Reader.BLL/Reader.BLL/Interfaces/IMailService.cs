using Reader.BLL.Abstractions;

namespace Reader.BLL.Interfaces
{
    public interface IMailService
    {
        Task<Result<string>> GetRecentMessagesFormattedAsync(string email, string senderEmail, int minutesAgo = 10, int maxCount = 16);
    }
}