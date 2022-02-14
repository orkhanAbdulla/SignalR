using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SinggnalR.DAL;
using SinggnalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinggnalR.Hubs
{
    public class ChatHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly Context _context;
        public ChatHub( UserManager<AppUser> userManager, DAL.Context context)
        {
            _userManager = userManager;
            _context = context;

        }
        public async Task SendMessage(string userValue,string message)
        {
            await Clients.All.SendAsync("RecieveMessage", userValue,message);
        }

        public override Task OnConnectedAsync()
        {

            AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            user.ConnectionId = Context.ConnectionId;
            _context.SaveChanges();
             Clients.All.SendAsync("Connected", user.Id);
            return  base.OnConnectedAsync();
        }
    }
}
