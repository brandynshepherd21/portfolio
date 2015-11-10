using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using ALOHASystem.Controllers;
using System.Collections;

namespace ALOHASystem.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            CustomerID = new List<int>();
            lstItemOrderViewModel = new List<ItemOrderViewModel>();
            lstOrderCartViewModel = new List<OrderCartViewModel>();
            
        }

        public List<int> CustomerID { get; set; }
        public List<ItemOrderViewModel> lstItemOrderViewModel { get; set; }
        public List<OrderCartViewModel> lstOrderCartViewModel { get; set; }
        
    }
    public class ItemOrderViewModel
    {
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
    }
    public class OrderCartViewModel
    {
        public OrderCartViewModel()
        {
            lstOrder = new List<OrderInfo>();
            lstOrderedInfo = new List<FindOrderInfo>();
            
        }
        public List<OrderInfo> lstOrder { get; set; }
        public List<FindOrderInfo> lstOrderedInfo { get; set; }
        public decimal total { get; set; }
    }
    public class OrderInfo
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        

    }
    public class NewCustomerModel
    {
        public string first { get; set; }
        public string last { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
    public class NewInventoryViewModel
    {
        public NewInventoryViewModel()
        {
            id = new List<int>();
            name = new List<string>();
            price = new List<decimal>();
            quantity = new List<int>();
            lstItem = new List<FindItemInfo>();
        }
        public List<int> id {get; set;}
        public List<string> name { get; set; }
        public List<decimal> price { get; set; }
        public List<int> quantity { get; set; }
        public List<FindItemInfo> lstItem { get; set; }
    }
    public class FindItemInfo
    {
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
    }
    public class FindOrderInfo
    {
        public int orderID { get; set; }
        public string itemName { get; set; }
        public decimal itemPrice { get; set; }
        public int itemQuantity { get; set; }
        public string date { get; set; }
        public string status { get; set; }
    }
    public class UpdateCustomerModel
    {
        public UpdateCustomerModel()
        {
            CustomerID = new List<int>();
            lstCustomer = new List<findCustomerInfo>();
            
        }
        public List<int> CustomerID { get; set; }
        public List<findCustomerInfo> lstCustomer { get; set; }
    }
    public class findCustomerInfo
    {
        public int id { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
    public class LstCustomerInfo
    {
        public LstCustomerInfo()
        {
            Fname = new List<string>();
            Lname = new List<string>();
            Phone = new List<string>();
            Email = new List<string>();
            Address = new List<string>();
            ID = new List<int>();

        }
        public List<string> Fname { get; set; }
        public List<string> Lname { get; set; }
        public List<string> Phone { get; set; }
        public List<string> Email { get; set; }
        public List<string> Address { get; set; }
        public List<int> ID { get; set; }
    }
    public class finances
    {
        public decimal net { get; set; }
        public decimal amazon { get; set; }
        public decimal gross { get; set; }
    }
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}