namespace Apps.Blackbird.Webhooks.Models;

public class BlackbirdWebhookPayload<T>
{
    public T Entity { get; set; }
}