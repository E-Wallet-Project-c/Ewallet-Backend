using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Enums
{
    public enum BeneficiaryPurpose
    {

        Personal = 1,      // مستفيد شخصي (مثل صديق أو قريب)
        Business = 2,      // مستفيد تجاري (شركة، تاجر)
        Donation = 3,      // تبرعات
        Savings = 4,       // حساب ادخار
        BillsPayment = 5   // دفع فواتير
    }
}
