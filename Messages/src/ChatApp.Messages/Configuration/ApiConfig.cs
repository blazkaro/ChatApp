namespace ChatApp.Messages.Configuration;

public class CommunicationPolicy
{
    public int RetryCount { get; set; }
    public TimeSpan MaxTimeout { get; set; }
    public int AllowedExceptionsCountBeforeBreak { get; set; }
    public TimeSpan BreakDuration { get; set; }
}

public class ConversationsApi
{
    public Uri BaseUrl { get; set; }
    public PathString ConversationAccessCheckPath { get; set; }
}

public class ApiConfig
{
    public ConversationsApi ConversationsApi { get; set; }
    public CommunicationPolicy CommunicationPolicy { get; set; }
}
