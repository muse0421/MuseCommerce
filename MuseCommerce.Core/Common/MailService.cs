using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Common
{
    public class MailService
    {
        public static void Send(string recieve, string subject, string mailbody)
        {
            string send = "xpy.liu@kingmaker-footwear.com";
            string host = "192.168.101.174";
            string uname = "xpy.liu@kingmaker-footwear.com";
            string pwd = "0000";
            string strFileName = "";

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = host;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(uname, pwd);
            
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(recieve);

            message.From = new System.Net.Mail.MailAddress(send, uname, System.Text.Encoding.UTF8);
            message.Subject = subject;
            message.Body = mailbody;
            //定义邮件正文，主题的编码方式   
            message.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            message.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //获取或设置一个值，该值指示电子邮件正文是否为   HTML。   
            message.IsBodyHtml = false;
            //指定邮件优先级   
            message.Priority = System.Net.Mail.MailPriority.High;

            //添加附件   
            //System.Net.Mail.Attachment data = new Attachment(@"E:\9527\tubu\PA260445.JPG", System.Net.Mime.MediaTypeNames.Application.Octet);   
            if (strFileName != "" && strFileName != null)
            {
                Attachment data = new Attachment(strFileName);
                message.Attachments.Add(data);
            }
            //发送
            client.Send(message);
        }
    }
}
