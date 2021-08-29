using DemoTTCSCN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.DAO
{
    public class StudentDAO
    {
        private StudentDAO() { }
        private static volatile StudentDAO instance;
        static object key = new object();
        public static StudentDAO Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new StudentDAO();
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        public List<Student> GetList()
        {
            var list = new List<Student>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Student");
            foreach (DataRow item in data.Rows)
            {
                Student obj = new Student(item);
                list.Add(obj);
            }
            return list;
        }

        public Student GetStudentByID(string StudentId)
        {
            Object[] obj = { StudentId };
            string query = $"SELECT * FROM dbo.SinhVien WHERE IDSinhVien = @studentId";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, obj);
            if (data != null)
            {
                if (data.Rows.Count != 0)
                {
                    return new Student(data.Rows[0]);
                }
                else
                    return null;
            }
            else
                return null;
        }
        //Update: Student 
        public int Update(string idStudent, string hoTen, string quequan, string malop, string gioiTinh, string diaChiHT, DateTime ngaySinh)
        {
            string dateOfBirth = ngaySinh.ToString("yyyy-MM-dd");
            try
            {
                //
                string query = $"UPDATE dbo.Student SET IDStudent='{idStudent}', HoTen=N'{hoTen}', QueQuan=N'{quequan}', IDLop='{malop}', GioiTinh=N'{gioiTinh}', DiaChiHT=N'{diaChiHT}', NgaySinh='{dateOfBirth}' WHERE IDStudent='{idStudent}'";
                DataProvider.Instance.ExecuteNonQuery(query);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Create: Student 
        public int Create(string idStudent, string hoTen, string quequan, string malop, string gioiTinh, string diaChiHT, DateTime ngaySinh)
        {
            string dateOfBirth = ngaySinh.ToString("yyyy-MM-dd");
            try
            {
                //
                string query = $"INSERT INTO dbo.Student(IDStudent, HoTen, QueQuan, IDLop, GioiTinh, DiaChiHT, NgaySinh) VALUES('{idStudent}', N'{hoTen}', N'{quequan}', '{malop}', N'{gioiTinh}', N'{diaChiHT}', '{dateOfBirth}')";
                DataProvider.Instance.ExecuteNonQuery(query);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Delete
        public int Delete(string idStudent)
        {
            try
            {
                string query = $"DELETE dbo.Student WHERE IDStudent='{idStudent}'";
                DataProvider.Instance.ExecuteNonQuery(query);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Search
        public List<Student> Search(string search)
        {
            var lstResults = new List<Student>();
            string query = $"SELECT * FROM dbo.Student WHERE IDStudent LIKE '%{search}%' OR HoTen LIKE '%{search}%' OR QueQuan LIKE '%{search}%' OR DiaChiHT LIKE '%{search}%' OR GioiTinh LIKE '%{search}%'  OR IDLop LIKE '%{search}%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Student obj = new Student(item);
                lstResults.Add(obj);
            }
            return lstResults;
        }
    }
}