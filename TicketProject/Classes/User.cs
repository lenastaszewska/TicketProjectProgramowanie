using System.ComponentModel.DataAnnotations;

namespace TicketProject.Classes
{
    public class User
    {
        [Key] 
        public int UserName { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
    }
}