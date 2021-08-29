using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.Models
{
    public class Class
    {
        public string IDLop { get; set; }
        public string TenLop { get; set; }
        public string NienKhoa { get; set; }


        public Class(string idLop, string tenLop, string nienKhoa)
        {
            this.IDLop = idLop;
            this.TenLop = tenLop;
            this.NienKhoa = nienKhoa;
        }

        public Class(DataRow row)
        {
            this.IDLop = row["IDLop"].ToString();
            this.TenLop = row["TenLop"].ToString();
            this.NienKhoa = row["NienKhoa"].ToString();
        }

        public Class() { }
    }
}