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
        public Sanitize()
        {

        }
        public string SanitizeQuery(string query)
        {
            TAntiSQLInjection anti = new TAntiSQLInjection(TDbVendor.DbVMssql);
            String msg = "";
            if (anti.isInjected(query))
            {
                msg = "This request contains invalid SQL statement \n\t$Detail:";
                for (int i = 0; i < anti.getSqlInjections().Count; i++)
                {
                    msg = msg + Environment.NewLine + ("\t$-type: " + anti.getSqlInjections()[i].getType() + ", description: " + anti.getSqlInjections()[i].getDescription());
                }
                return msg;
            }
            else
            {
                return null;
            }
        }
    }
}
