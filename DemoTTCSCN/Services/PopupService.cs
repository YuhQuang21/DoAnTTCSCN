using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DemoTTCSCN.Services
{
    public class PopupService
    {
        private static volatile PopupService instance;
        private PopupService() { }
        static object key = new object();
        public static PopupService Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new PopupService();
                    }
                }
                return instance;
            }
        }
        public string MsgBox(String noti)
        {
            return "<SCRIPT language='javascript'>alert('" + noti.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
            //Type cstype = obj.GetType();
            //ClientScriptManager cs = pg.ClientScript;
            //cs.RegisterClientScriptBlock(cstype, s, s.ToString());
        }
    }
}