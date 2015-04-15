using CounterWebSite.Models;
using Mvc.Mailer;
using System;
using System.Net.Mail;

namespace CounterWebSite.Mailers
{
    public interface ICountifyMailer
    {
        MvcMailMessage SubscriptionEmail(String ViewName, EmailModel model, MailAddress recipient);
        //void SendAsyncMessage(MvcMailMessage Message);
    }
}