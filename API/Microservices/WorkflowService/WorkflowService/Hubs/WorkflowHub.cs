using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Hubs
{
    public class WorkflowHub : Hub
    {
        public async Task Send(string boardId)
        {
            await Clients.Group(boardId).SendAsync(boardId);
        }

        public async Task JoinGroup(string boardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);
        }
    }
}
