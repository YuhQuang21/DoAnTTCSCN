using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoTTCSCN.Services
{
    public class ExceptionService
    {
        private ExceptionService() { }
        private static volatile ExceptionService instance;
        static object key = new object();
        public static ExceptionService Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new ExceptionService();
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        private String _exception { get; set; }
        public string getException()
        {
            return _exception;
        }
        public void setException(String exception)
        {
            _exception = exception;
        }
    }
}