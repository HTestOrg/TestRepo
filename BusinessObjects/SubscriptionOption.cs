using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class SubscriptionOption
    {
        [Required]
        public int ID { get; set; }


        public string PlanName { get; set; }


        public double Amount { get; set; }

        public string Caption { get; set; }

        public string Save { get; set; }

        public double AmountPerMonth { get; set; }
        public string BillingCycle { get; set; }
        public string BillSavingAmount { get; set; }
        public long RoleId { get; set; }

    }

    public class SubscriptionOptionsList
    {
        public SubscriptionOptionsList()
        {
            Subscriptions = new List<SubscriptionOption>();
        }
        public List<SubscriptionOption> Subscriptions { get; set; }
    }
}
