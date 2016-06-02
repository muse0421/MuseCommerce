using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Exception_Handling
{
    public class ExceptionDetail
    {
        public ExceptionDetail(Exception exception, string errorMessage = null)
        {
            this.HelpLink = exception.HelpLink;
            this.Message = string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage;
            this.StackTrace = exception.StackTrace;
            this.Type = exception.GetType().ToString();
            if (exception.InnerException != null)
            {
                this.InnerException = new ExceptionDetail(exception.InnerException);
            }
        }

        public string HelpLink { get; set; }
        public ExceptionDetail InnerException { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Type { get; set; }
    }
}