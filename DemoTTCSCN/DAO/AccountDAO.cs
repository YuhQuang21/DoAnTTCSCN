using AntiSqlInjection;
using DemoTTCSCN.Models;
using gudusoft.gsqlparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.DAO
{
    public class AccountDAO
    {
        private Sanitize _sanitize;
        private AccountDAO(Sanitize sanitize) 
        {
            this._sanitize = sanitize;
        }
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
                        instance = new AccountDAO(new Sanitize());
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        public UserLogin Login(string username, string password)
        {
            var query = $"SELECT * FROM dbo.Account WHERE UserName = '{username}' and Password = '{password}'";
            var isException =_sanitize.SanitizeQuery(query);
            if (isException != null)
            {
                throw isException;
            }
            var data = DataProvider.Instance.ExecuteQuery(query);
            if (data != null)
            {
                if (data.Rows.Count != 0)
                {
                    var row = data.Select().FirstOrDefault();
                    var userLogin = new UserLogin
                    {
                        Username = row["UserName"].ToString(),
                        Password = row["Password"].ToString(),
                        IdStudent = row["IdSinhVien"].ToString()
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