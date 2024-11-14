using System.ComponentModel.DataAnnotations;

namespace UserLoginASPCoreApp.Models
{
    public class User
    {
        public int id { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string session { get; set; }
    }

}

