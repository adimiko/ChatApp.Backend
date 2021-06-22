using ChatApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Infrastructure.Hubs
{
    public interface IChatHubContext : IHubContext<ChatHub, IChatClient>
    { }
}