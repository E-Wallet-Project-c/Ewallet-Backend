using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace E_wallet.Applications.Hubs
{
    [Authorize]   
    public class AppHub : Hub
    {

    }
}