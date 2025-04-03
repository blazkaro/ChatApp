namespace ChatApp.Spa.Server.Configuration;

public class CommunicationPolicy
{
    public int RetryCount { get; set; }
    public TimeSpan MaxTimeout { get; set; }
    public int AllowedExceptionsCountBeforeBreak { get; set; }
    public TimeSpan BreakDuration { get; set; }
}

public class Auth0Api
{
    public Uri BaseUrl { get; set; }
    public PathString GetUsersPath { get; set; }
}

public class ConversationsApi
{
    public Uri BaseUrl { get; set; }
    public PathString GetMembersPath { get; set; }
}

public class ApiConfig
{
    public Auth0Api Auth0Api { get; set; }
    public ConversationsApi ConversationsApi { get; set; }
    public CommunicationPolicy CommunicationPolicy { get; set; }
}