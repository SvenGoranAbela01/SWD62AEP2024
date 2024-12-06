using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CustomUser:IdentityUser
    {
        public string first_name {  get; set; }
        public string last_name { get; set; }
    }
}
