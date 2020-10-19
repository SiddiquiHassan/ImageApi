using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgImpExp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int PhoneNo { get; set; }
        public byte[] Image { get; set; }

    }
}