using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Enums
{
    public enum TransferStatus
    {
        Pending,    // تم إنشاء التحويل ولم يعالج بعد
        Processing, // جاري المعالجة في الـ ledger
        Completed,  // تم التحويل بنجاح
        Failed,     // فشل التحويل
        Cancelled   // تم إلغاء التحويل
    }
}
