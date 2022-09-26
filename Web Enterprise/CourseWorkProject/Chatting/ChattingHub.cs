using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CourseWorkProject.Chatting
{
    public class ChattingHub : Hub
    {
        public void Chatting(string someText)
        {
            Clients.All.chatForFun(someText);
        }
    }
}