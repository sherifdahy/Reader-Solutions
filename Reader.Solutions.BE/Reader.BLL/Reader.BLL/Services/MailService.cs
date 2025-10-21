using MailKit.Net.Imap;
using MailKit.Security;
using NOTE.Solutions.Entities.Interfaces;
using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Message;
using Reader.BLL.Errors;
using Reader.BLL.Interfaces;
using System.Text;

namespace Reader.BLL.Services
{
    public class MailService : IMailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> GetRecentMessagesFormattedAsync(string email, string senderEmail, int minutesAgo = 10, int maxCount = 16)
        {
            var client = await _unitOfWork.Clients.FindAsync(x => x.Email == email);

            if (client == null)
                return Result.Failure<string>(ClientErrors.NotFound);

            if (minutesAgo <= 0) minutesAgo = 10;
            DateTime fromDateTime = DateTime.UtcNow.AddMinutes(-minutesAgo);

            var messages = new List<MessageResponse>();

            using (var imapClient = new ImapClient())
            {
                imapClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await imapClient.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await imapClient.AuthenticateAsync(client.Email, client.AppPassword);

                var inbox = imapClient.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                // SearchQuery بدون الوقت (Gmail يدعم اليوم فقط)
                var query = MailKit.Search.SearchQuery.FromContains(senderEmail);
                var uids = (await inbox.SearchAsync(query))
                            .OrderByDescending(x => x.Id)
                            .Take(maxCount)
                            .ToList();

                foreach (var uid in uids)
                {
                    var mime = await inbox.GetMessageAsync(uid);

                    // فلترة دقيقة بالدقائق بعد استرجاع الرسائل
                    if (mime.Date.UtcDateTime < fromDateTime)
                        continue;

                    string rawBody = mime.TextBody ?? mime.HtmlBody ?? "[لا يوجد محتوى]";
                    var parsedBody = ParseBody(rawBody);

                    messages.Add(new MessageResponse
                    {
                        From = mime.From.Mailboxes.FirstOrDefault()?.Address ?? mime.From.ToString(),
                        Subject = mime.Subject,
                        Body = parsedBody,
                        DateTime = mime.Date.UtcDateTime
                    });
                }

                await imapClient.DisconnectAsync(true);
            }

            var formatted = FormatMessagesForWhatsapp(messages, client.Email);

            return Result.Success(formatted);
        }

        // ---------------------------- Helpers ----------------------------
        private MessageBodyResponse ParseBody(string rawBody)
        {
            return new MessageBodyResponse
            {
                Topic = ExtractValue(rawBody, "Topic:"),
                Language = ExtractValue(rawBody, "Language:"),
                Partner = ExtractValue(rawBody, "Your Partner:"),
                RecordingLink = ExtractLink(rawBody)
            };
        }

        private string ExtractValue(string text, string key)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var index = text.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (index == -1) return null;

            var start = index + key.Length;
            var end = text.IndexOf('\n', start);
            if (end == -1) end = text.Length;

            return text.Substring(start, end - start).Trim('*', ':', ' ');
        }

        private string ExtractLink(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var marker = "https://";
            var index = text.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (index == -1) return null;

            var end = text.IndexOfAny(new[] { ' ', '\r', '\n', '<', '>' }, index);
            if (end == -1) end = text.Length;

            return text.Substring(index, end - index).Trim();
        }

        private string FormatMessagesForWhatsapp(List<MessageResponse> messages, string clientEmail)
        {
            if (messages == null || !messages.Any())
                return "No new messages found.";

            var formatted = new StringBuilder();
            formatted.AppendLine($"📬 Number of Links: {messages.Count}");
            formatted.AppendLine(new string('═', 40));
            formatted.AppendLine();

            int counter = 1;
            foreach (var msg in messages.OrderByDescending(m => m.DateTime))
            {
                var localTime = msg.DateTime.ToLocalTime();
                string link = msg.Body?.RecordingLink ?? "[لا يوجد رابط]";

                if (!string.IsNullOrWhiteSpace(link) && !link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    link = "https://" + link;

                formatted.AppendLine($"🔢 Link #{counter++}");
                formatted.AppendLine($"📌 Topic: {msg.Body?.Topic ?? "[لا يوجد موضوع]"}");
                formatted.AppendLine($"👤 Email: {clientEmail}");
                formatted.AppendLine();
                formatted.AppendLine($"🔗 Your Recording Link:");
                formatted.AppendLine(link);
                formatted.AppendLine();
                formatted.AppendLine($"📅 Date: {localTime:yyyy-MM-dd HH:mm:ss}");
                formatted.AppendLine(new string('━', 50));
                formatted.AppendLine();
            }

            return formatted.ToString();
        }
    }
}
