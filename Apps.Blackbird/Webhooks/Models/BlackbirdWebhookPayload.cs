namespace Apps.Blackbird.Webhooks.Models;

public class BlackbirdWebhookPayload<T>
{
    public string NestId { get; set; }
    public T Entity { get; set; }
}