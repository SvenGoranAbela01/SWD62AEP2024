using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Student
    {
        [Key]
        public string IdCard { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //This is the foreign key so this contains a single value
        public string GroupFK { get; set; }
        //This is a navigational property
        // the navigational property will allow me to explore and navigate the
        // group properties right through an eventual student instance
        //adv: i can get data related to the Group pertaining to this student without having to write additional sql/linq statements
        [ForeignKey("GroupFK")]
        public Group Group { get; set; }
    }
}
