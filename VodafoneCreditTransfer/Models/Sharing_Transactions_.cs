using System;

namespace VodafoneCreditTransfer.Models
{
    public class Sharing_Transactions_
    {
        public int Transaction_No_ { get; set; }
        public string Transaction_Type { get; set; }
        public int From_Number_ { get; set; }
        public int To_Number_ { get; set; }
        public decimal Credit_Transfered { get; set; }
        public int Minutes_Transfered { get; set; }
        public int Data_Transferred_MB { get; set; }
        public DateTime Date_and_Time { get; set; }
        public bool Requested { get; set; }
        public bool Request_accepted { get; set; }
    }
}
