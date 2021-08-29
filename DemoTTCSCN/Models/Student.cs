using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.Models
{
    public class Student
    {
        public string IDSinhVien { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string QueQuan { get; set; }
        public string DiaChiHT { get; set; }
        public string KhoaDKi { get; set; }
        public int SoTCDaDat { get; set; }
        public int SoTCDaDki { get; set; }
        public float DiemTichLuy { get; set; }
        public string IDLop { get; set; }
        public Student(DataRow row)
        {
            this.IDSinhVien = row["IDSinhVien"].ToString();
            this.HoTen = row["HoTen"].ToString();
            this.QueQuan = row["Quequan"].ToString();
            this.IDLop = row["IDLop"].ToString();
            this.GioiTinh = row["GioiTinh"].ToString();
            this.DiaChiHT = row["DiaChiHT"].ToString();
            this.KhoaDKi = row["KhoaDKi"].ToString();
            if (string.IsNullOrEmpty(row["SoTCDaDat"].ToString()))
            {
                this.SoTCDaDat = 0;
            }
            else
                this.SoTCDaDat = int.Parse(row["SoTCDaDat"].ToString());
            if (row["SoTCDaDki"].ToString().Equals(""))
            {
                this.SoTCDaDki = 0;
            }
            else
                this.SoTCDaDki = int.Parse(row["SoTCDaDki"].ToString());
            if (row["DiemTichLuy"].ToString().Equals(""))
            {
                this.DiemTichLuy = 0;
            }
            else
                this.DiemTichLuy = float.Parse(row["DiemTichLuy"].ToString());
            this.NgaySinh = DateTime.Parse(row["NgaySinh"].ToString());

        }

        public Student(string idSinhVien, string hoten, string queQuan, string idLop, string gioiTinh, string diaChiHT, int soTCDaDat, int soTCDaDki, float diemTichLuy)
        {
            this.IDSinhVien = idSinhVien;
            this.HoTen = hoten;
            this.QueQuan = queQuan;
            this.IDLop = idLop;
            this.GioiTinh = gioiTinh;
            this.DiaChiHT = diaChiHT;
            this.SoTCDaDat = soTCDaDat;
            this.SoTCDaDki = soTCDaDki;
            this.DiemTichLuy = diemTichLuy;
        }

        public Student() { }
    }
}