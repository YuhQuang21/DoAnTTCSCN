using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.DAO
{
    public class DataProvider
    {
        private static volatile DataProvider instance;
        private DataProvider() { }
        private string strConnection = @"Data Source=QUANGHUY; Initial Catalog = S4_N10_QLDiemSV; User Id=sa; Password=Huytuyen2211@; Integrated Security=True";
        static object key = new object();
        public static DataProvider Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new DataProvider();
                    }
                }
                return instance;
            }
        }

        // Thuc thi cau lenh va tra ve ket qua la danh sach du lieu (select.....)
        public DataTable ExecuteQuery(string query, Object[] parameter = null)
        {
            DataTable data = new DataTable("data");
            using (SqlConnection connection = new SqlConnection(strConnection))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int n = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.Add(new SqlParameter(item, parameter[n].ToString()));
                            n++;
                        }
                    }

                }
                // trung gian qua mot cai SqlAdapter
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(data);
                connection.Close();

            }
            return data;
        }

        // Thuc hien truy van insert, update, delete 
        public int ExecuteNonQuery(string query, Object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int n = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameter[n]);
                            n++;
                        }
                    }
                }

                // trung gian qua mot cai SqlAdapter
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, Object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int n = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameter[n]);
                            n++;
                        }
                    }
                }

                // trung gian qua mot cai SqlAdapter
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}