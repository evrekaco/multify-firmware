using DatabaseContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CounterWebSite.Models
{
    public class EmailModel
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
    }

    public class PreOrderEmail : EmailModel
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }

        public PreOrderEmail() : base() { }

        public PreOrderEmail(PreOrder Order) : this()
        {
            this.Subject = String.Format("New Pre-Order From {0}", Order.BusinessName);
            this.Id = Order.Id;
            this.BusinessName = Order.BusinessName;
            this.Name = Order.Name;
            this.EmailAddress = Order.EmailAddress;
            this.PhoneNumber = Order.PhoneNumber;
            this.Address = Order.Address;
            this.Message = Order.Message;
        }
    }
}