using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenDemo.Models
{
    public class User
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the User Name!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter the Email!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the Mobile Number!")]
        public long Mobile { get; set; }

        [Required(ErrorMessage = "Enter the Password")]
        public string Password { get; set; }
    }
}
