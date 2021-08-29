using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdSinhVien { get; set; }
    }
}