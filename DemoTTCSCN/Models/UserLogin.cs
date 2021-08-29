using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.Models
{
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IdStudent { get; set; }
        public int Type { get; set; }
        public UserLogin(DataRow row )
        {
            this.Username = row["UserName"].ToString();
            this.Password = row["Password"].ToString();
            this.IdStudent = row["IdStudent"].ToString();
            this.Type = Convert.ToInt32(row["Type"]);
        }
        public UserLogin()
        {

        }
    }
}