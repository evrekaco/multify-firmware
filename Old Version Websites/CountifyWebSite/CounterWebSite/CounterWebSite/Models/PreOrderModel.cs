using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CounterWebSite.Models
{
    public class PreOrderModel
    {
        [Required(ErrorMessageResourceName = "PreOrderModel_BusinessNameRequired", ErrorMessageResourceType = typeof(OrderStrings))]
        [Display(Name = "PreOrderModel_BusinessName", ResourceType=typeof(OrderStrings))]
        public string BusinessName { get; set; }

        [Required(ErrorMessageResourceName = "PreOrderModel_NameRequired", ErrorMessageResourceType = typeof(OrderStrings))]
        [Display(Name = "PreOrderModel_Name", ResourceType = typeof(OrderStrings))]
        public string Name { get; set; }
        
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "PreOrderModel_EmailInvalid", ErrorMessageResourceType = typeof(OrderStrings))]
        [Display(Name = "PreOrderModel_Email", ResourceType = typeof(OrderStrings))]
        public string EmailAddress { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PreOrderModel_Phone", ResourceType = typeof(OrderStrings))]
        public string PhoneNumber { get; set; }

        [Display(Name = "PreOrderModel_Address", ResourceType = typeof(OrderStrings))]
        public string Address { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "PreOrderModel_Message", ResourceType = typeof(OrderStrings))]
        public string Message { get; set; }
    }
}