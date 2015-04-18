using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeFest.NinjaFamily.FamilyTreeApp.Models
{
    public class Relationship
    {
        public int ID { get; set; }
        public int Parrent { get; set; }
        public int Child { get; set; }
    }
}