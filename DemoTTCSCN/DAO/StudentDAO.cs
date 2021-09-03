using DemoTTCSCN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Student>> GetList()
        {
            var list = new List<Student>();
            DataTable data = await DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.SinhVien");
            foreach (DataRow item in data.Rows)
            {
                Student obj = new Student(item);
                list.Add(obj);
            }
            return list;
        }

        public async Task<Student> GetStudentByID(string StudentId)
        {
            string[] parameter = { StudentId };
            string query = $"SELECT * FROM dbo.SinhVien WHERE IDSinhVien = '{StudentId}'";
            DataTable data = await DataProvider.Instance.ExecuteQuery(query, parameter);
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
        public async Task<int> Update(Student student)
        {
            string[] parameter = { student.QueQuan, student.GioiTinh, student.DiaChiHT, student.SoTCDaDki.ToString(), student.SoTCDaDat.ToString(), student.DiemTichLuy.ToString(), student.IDSinhVien };
            try
            {
                //
                string query = $"UPDATE dbo.SinhVien SET QueQuan=N'{student.QueQuan}', GioiTinh=N'{student.GioiTinh}', DiaChiHT=N'{student.DiaChiHT}', SoTCDaDki = {student.SoTCDaDki}, SoTCDaDat = {student.SoTCDaDat}, DiemTichLuy = {student.DiemTichLuy} WHERE IDSinhVien='{student.IDSinhVien}'";
                await DataProvider.Instance.ExecuteNonQuery(query, parameter);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Create: Student 
        public async Task<int> Create(string idStudent, string hoTen, string quequan, string malop, string gioiTinh, string diaChiHT, DateTime ngaySinh)
        {
            string[] parameter = { idStudent, hoTen, quequan, malop, gioiTinh, diaChiHT, ngaySinh.ToString("yyyy-MM-dd") };
            try
            {
                //
                string query = $"INSERT INTO dbo.Student(IDStudent, HoTen, QueQuan, IDLop, GioiTinh, DiaChiHT, NgaySinh) VALUES('{idStudent}', N'{hoTen}', N'{quequan}', '{malop}', N'{gioiTinh}', N'{diaChiHT}', '{ngaySinh.ToString("yyyy-MM-dd")}')";
                await DataProvider.Instance.ExecuteNonQuery(query, parameter);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Delete
        public async Task<int> Delete(string idStudent)
        {
            try
            {
                string query = $"DELETE dbo.Student WHERE IDStudent='{idStudent}'";
                await DataProvider.Instance.ExecuteNonQuery(query, new string[] { idStudent });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Search
        public async Task<List<Student>> Search(string search)
        {
            var lstResults = new List<Student>();
            string query = $"SELECT * FROM dbo.Student WHERE IDStudent LIKE '%{search}%' OR HoTen LIKE '%{search}%' OR QueQuan LIKE '%{search}%' OR DiaChiHT LIKE '%{search}%' OR GioiTinh LIKE '%{search}%'  OR IDLop LIKE '%{search}%'";
            DataTable data = await DataProvider.Instance.ExecuteQuery(query, new string[] { search });
            foreach (DataRow item in data.Rows)
            {
                Student obj = new Student(item);
                lstResults.Add(obj);
            }
            return lstResults;
        }
    }
}