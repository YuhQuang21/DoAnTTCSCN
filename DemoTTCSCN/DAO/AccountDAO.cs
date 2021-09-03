using DemoTTCSCN.Models;
using gudusoft.gsqlparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DemoTTCSCN.DAO
{
    public class AccountDAO
    {
        
        private AccountDAO() { }
        private static volatile AccountDAO instance;
        static object key = new object();
        public static AccountDAO Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        public async Task<UserLogin> Login(string username, string password)
        {
            string[] parameter = { username, password };
            var query = $"SELECT * FROM dbo.Account WHERE UserName = '{username}' and Password = '{password}'";
            var data = await DataProvider.Instance.ExecuteQuery(query, parameter);
            if (data != null)
            {
                if (data.Rows.Count != 0)
                {
                    var row = data.Select().FirstOrDefault();
                    var userLogin = new UserLogin
                    {
                        Username = row["UserName"].ToString(),
                        Password = row["Password"].ToString(),
                        IdStudent = row["IdSinhVien"].ToString(),
                        Type = row["Type"] != null ? Convert.ToInt32(row["Type"]) : 1
                        
                    };
                    return userLogin;
                }
                else
                    return null;
            }
            else
                return null;

        }
    }
}