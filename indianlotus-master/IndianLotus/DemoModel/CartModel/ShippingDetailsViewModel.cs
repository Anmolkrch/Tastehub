using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoModel.ViewModel
{
   public class ShippingDetailsViewModel
    {
        public int ShippingDetailId { get; set; }

        public int MemberId { get; set; }

        public int AddressId { get; set; }

        public Nullable<decimal> AmountPaid { get; set; }

        public string PaymentType { get; set; }

    }
}
