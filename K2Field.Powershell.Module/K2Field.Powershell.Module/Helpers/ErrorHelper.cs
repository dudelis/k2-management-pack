using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace K2Field.Powershell.Module
{
    internal class ErrorHelper : Cmdlet
    {

        internal static void Throw(Exception ex)
        {
            var error = new StringBuilder();
            error.AppendFormat("Exception.Message: {0}\n", ex.Message);
            error.AppendFormat("Exception.StackTrace: {0}\n", ex.StackTrace);

            var innerEx = ex;
            int i = 0;
            while (innerEx.InnerException != null)
            {
                error.AppendFormat("{0} InnerException.Message: {1}\n", i, innerEx.InnerException.Message);
                error.AppendFormat("{0} InnerException.StackTrace: {1}\n\n", i, innerEx.InnerException.StackTrace);
                innerEx = innerEx.InnerException;
                i++;
            }
            Console.WriteLine(error.ToString());
        }
    }
}
