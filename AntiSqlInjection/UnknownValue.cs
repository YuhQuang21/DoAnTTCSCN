using System;
using System.Collections.Generic;
using System.Text;

namespace AntiSQLInjection
{
    class UnknownValue
    {
        public String toString()
        {
            return "unknown value";
        }

        public String AsText
        {
            get { return this.toString(); }
        }
    }
}
