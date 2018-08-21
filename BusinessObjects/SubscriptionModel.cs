using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    [Serializable()]
    public class SubscriptionModel
    {
        public int ID { get; set; } 
        public long UserID { get; set; } 
        public double Amount { get; set; }

        [Required(ErrorMessage = "The Card Type is required.")]
        public int CardTypeID { get; set; }

        [Required(ErrorMessage = "The Name On Card field is required.")]
        [StringLength(50, ErrorMessage =("Maximum 50 characters are allowed."))]
        [RegularExpression("[a-zA-Z ]*", ErrorMessage = "Enter valid Name.")]
        [DisplayName("Name On Card")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "The Card Number field is required.")]
        [StringLength(16, ErrorMessage = ("Enter valid Card Number."))]
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter valid Card Number.")]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        public List<ExpirationMonth> ExpirationMonthList { get; set; } 
        [Required(ErrorMessage = "Month is required.")]
        [DisplayName("Expiration Month")]
        public int ExpirationMonth { get; set; }

        public List<ExpirationYear> ExpirationYearList { get; set; } 
        [Required(ErrorMessage = "Year is required.")]
        [DisplayName("Expiration Year")]
        public int ExpirationYear { get; set; }

        [StringLength(3, ErrorMessage = ("Enter valid Security Code."))]
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter valid Security Code.")]
        [DisplayName("Security Code")]
        public string SecurityCode { get; set; }

        public long RoleID { get; set; }
        public string Token { get; set; }

        [DisplayName("Card Type")]
        public List<PaymentCardTypes> CardType { get; set; }

        public string BillingDate { get; set; }
        public string CustomerID { get; set; }
        public SubscriptionPlans SubscriptionPlans { get; set; }

        [Required(ErrorMessage = "The Billing Address field is required.")]
        [StringLength(200)]
        [DisplayName("Billing Address")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        [RegularExpression("^[a-zA-Z\\s]+", ErrorMessage = "Numbers and Special Symbols are not allowed.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The State field is required.")]
        [RegularExpression("^[a-zA-Z\\s]+", ErrorMessage = "Numbers and Special Symbols are not allowed.")]
        public string State { get; set; }

        [Required(ErrorMessage = "The Zip field is required.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Please enter valid Zip")]
        public string Zip { get; set; }

        public SubscriptionOption subscriptionOption { get; set; }

    }

    public class PaymentCardTypes
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
    [Serializable]
    public class SubscriptionPlans
    {
        public long ID { get; set; }
        public string SubscriptionPlanID { get; set; }
        public double Amount { get; set; }
    }
    public class ExpirationYear
    {
        public int ID { get; set; }
        public string Year { get; set; }
    }
    public class ExpirationMonth
    {
        public int ID { get; set; }
        public string Month { get; set; }
    }

}
