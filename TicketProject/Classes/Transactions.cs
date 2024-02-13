using System.ComponentModel.DataAnnotations;

namespace TicketProject.Classes
{
    public class Transactions
    {
        [Key] public int UserNameID { get; set; }

        public int UserName {  get; set; }
        public int price { get; set; }
        public string place { get; set; }
        public string artist { get; set; }
    }
}
