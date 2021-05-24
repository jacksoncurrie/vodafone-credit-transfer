namespace VodafoneCreditTransfer.Models
{
    public class Notifications_and_Requests
    {
        public int RQ_Number_PK { get; set; }
        public int Mobile_or_Landline_Number_ { get; set; }
        public bool Request_or_notifycation { get; set; }
        public int Requested_From { get; set; }
        public string Notification_MSG { get; set; }
        public decimal Amount_Requested { get; set; }
        public string Credit_Type_Requested { get; set; }
        public bool Notification_Recived { get; set; }
    }
}
