using DemoTTCSCN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DemoTTCSCN.DAO
{
    public class ClassDAO
    {
        private ClassDAO() { }
        private static volatile ClassDAO instance;
        static object key = new object();
        public static ClassDAO Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new ClassDAO();
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        public async Task<List<Class>> GetList()
        {
            var list = new List<Class>();
            DataTable data = await DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Class");
            foreach (DataRow item in data.Rows)
            {
                Class obj = new Class(item);
                list.Add(obj);
            }
            return list;
        }

        //public Class GetClassByID(string ClassId)
        //{
        //    string query = $"SELECT * FROM dbo.Class sv WHERE IDClass = '{ClassId}'";
        //    DataTable data = DataProvider.Instance.ExecuteQuery(query);
        //    if (data != null)
        //    {
        //        return new Class(data.Rows[0]);
        //    }
        //    else
        //        return null;
        //}
        //Update: Class 
        //public int Update(string idClass, string hoTen, string quequan, string maClass, string gioiTinh, string diaChiHT, DateTime ngaySinh)
        //{
        //    string dateOfBirth = ngaySinh.ToString("yyyy-MM-dd");
        //    try
        //    {
        //        //
        //        string query = $"UPDATE dbo.Class SET IDClass='{idClass}', HoTen=N'{hoTen}', QueQuan=N'{quequan}', IDClass='{maClass}', GioiTinh=N'{gioiTinh}', DiaChiHT=N'{diaChiHT}', NgaySinh='{dateOfBirth} WHERE IDClass='{idClass}'";
        //        DataProvider.Instance.ExecuteNonQuery(query);
        //        return 1;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        ////Create: Class 
        //public int Create(string idClass, string hoTen, string quequan, string maClass, string gioiTinh, string diaChiHT, DateTime ngaySinh)
        //{
        //    string dateOfBirth = ngaySinh.ToString("yyyy-MM-dd");
        //    try
        //    {
        //        //
        //        string query = $"INSERT INTO dbo.Class(IDClass, HoTen, QueQuan, IDClass, GioiTinh, DiaChiHT, NgaySinh) VALUES('{idClass}', N'{hoTen}', N'{quequan}', '{maClass}', N'{gioiTinh}', N'{diaChiHT}', '{dateOfBirth}')";
        //        DataProvider.Instance.ExecuteNonQuery(query);
        //        return 1;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        ////Delete
        //public int Delete(string idClass)
        //{
        //    try
        //    {
        //        string query = $"DELETE dbo.Class WHERE IDClass='{idClass}'";
        //        DataProvider.Instance.ExecuteNonQuery(query);
        //        return 1;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        ////Search
        //public List<Class> Search(string search)
        //{
        //    var lstResults = new List<Class>();
        //    string query = $"SELECT * FROM dbo.Class WHERE IDClass LIKE '%{search}%' OR HoTen LIKE '%{search}%' OR QueQuan LIKE '%{search}%' OR DiaChiHT LIKE '%{search}%' OR GioiTinh LIKE '%{search}%'  OR IDClass LIKE '%{search}%'";
        //    DataTable data = DataProvider.Instance.ExecuteQuery(query);
        //    foreach (DataRow item in data.Rows)
        //    {
        //        Class obj = new Class(item);
        //        lstResults.Add(obj);
        //    }
        //    return lstResults;
        //}
    }
}