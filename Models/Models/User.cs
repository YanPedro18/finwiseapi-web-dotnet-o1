using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Password { get; set; }
    }


}