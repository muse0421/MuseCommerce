using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace MuseCommerce.Web.SignalR
{
    public class NoticeMessage
    {        
        public string Sender { set; get; }
        public string Content { set; get; }
        public string SendTime { set; get; }
    }

    public class NoticeMessageSubSystem
    {
        public static void SendMessage(string Content)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NoticeMessageHub>();

            NoticeMessage sendData = new NoticeMessage()
            {
                Sender = "系统消息",
                Content = Content,
                SendTime = DateTime.Now.ToString("MMdd HHmm")
            };
            context.Clients.All.ReviceNotice(sendData);
        }
    }

   
    [HubName("NoticeMessageHub")]
    public class NoticeMessageHub : Hub
    { 
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

       

        public void SendMessage()
        {
            NoticeMessage sendData = new NoticeMessage()
            {
                Sender = "消息",
                Content = "系統登錄消息",
                SendTime = DateTime.Now.ToString("yyyyMMdd HH:mm")
            };
            Clients.All.ReviceNotice(sendData);
        }

        public void SendChartMessage(string connecionid,string message)
        {
            NoticeMessage sendData = new NoticeMessage()
            {
                Sender = connecionid,
                Content = message,
                SendTime = DateTime.Now.ToString("MMdd HHmm")
            };

            Clients.Others.ReviceChartNotice(sendData);
            //Clients.All.ReviceChartNotice(sendData);
        }
                

        public void CallHello()
        {
            Clients.Caller.hello();
        }
    }
}