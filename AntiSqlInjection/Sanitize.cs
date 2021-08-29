using AntiSQLInjection;
using gudusoft.gsqlparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiSqlInjection
{
    public class Sanitize
    {
        public Exception SanitizeQuery(string query)
        {
            TAntiSQLInjection anti = new TAntiSQLInjection(TDbVendor.DbVMssql);
            String mes = "";
            if (anti.isInjected(query))
            {
                mes = "This is an SQL query contains malicious code\nDetail:";
                for (int i = 0; i < anti.getSqlInjections().Count; i++)
                {
                    mes = mes + Environment.NewLine + ("type: " + anti.getSqlInjections()[i].getType() + ", description: " + anti.getSqlInjections()[i].getDescription());
                }
                return new Exception(mes);
            }
            else
            {
                return null;
            }
        }
    }
}
