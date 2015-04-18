using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeFest.NinjaFamily.FamilyTreeApp.Models
{
    public class User
    {
        public string UserName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }
        [DataType(DataType.Date)]
        public DateTime Death { get; set; }
        public int Age
        {
            get
            {
                 return Math.Max(Death.Year - Birth.Year, DateTime.Now.Year - Birth.Year);
            }
        }

    }
}