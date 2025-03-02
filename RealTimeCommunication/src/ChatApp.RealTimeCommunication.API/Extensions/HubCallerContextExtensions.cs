using ChatApp.RealTimeCommunication.DataStructures;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;

namespace ChatApp.RealTimeCommunication.Extensions;

public static class HubCallerContextExtensions
{
    private const string CONVERSATIONS_KEY = "Conversations";

    /// <summary>
    /// Stores conversation in collection of caller's conversations
    /// </summary>
    /// <param name="ctx">The caller context</param>
    /// <param name="conversationId">The conversation id</param>
    /// <returns>true if stored successfully; otherwise, false</returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool StoreConversation(this HubCallerContext ctx, string conversationId)
    {
        if (string.IsNullOrEmpty(conversationId))
        {
            throw new ArgumentException($"'{nameof(conversationId)}' cannot be null or empty.", nameof(conversationId));
        }

        if (!ctx.Items.ContainsKey(CONVERSATIONS_KEY))
            ctx.Items.TryAdd(CONVERSATIONS_KEY, new ConcurrentHashSet<string>());

        if (ctx.Items[CONVERSATIONS_KEY] is not ConcurrentHashSet<string> conversations)
            return false;

        return conversations.TryAdd(conversationId);
    }

    /// <summary>
    /// Checks whether caller is member of conversation
    /// </summary>
    /// <param name="ctx">The caller context</param>
    /// <param name="conversationId">The conversation id</param>
    /// <returns>true if caller is member of conversation; otherwise, false</returns>
    public static bool HasConversation(this HubCallerContext ctx, [NotNullWhen(true)] string? conversationId)
    {
        if (string.IsNullOrEmpty(conversationId) || !ctx.Items.ContainsKey(CONVERSATIONS_KEY))
            return false;

        if (ctx.Items[CONVERSATIONS_KEY] is ConcurrentHashSet<string> conversations && conversations.ContainsKey(conversationId))
            return true;

        return false;
    }
}
