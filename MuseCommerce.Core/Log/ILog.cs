using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Log
{
    public interface ILog
    {
        void Critical(string Message);

        void Error(string Message);

        void Warning(string Message);

        void Information(string Message);

        void Verbose(string Message);
    }

    public class EntLibLog : ILog
    {
        protected void Writer(TraceEventType Severity, string Message)
        {
            LogEntry logEntry = new LogEntry();

            logEntry.Priority = 1;
            logEntry.Severity = Severity;
            logEntry.Message = Message;

            Logger.Writer.Write(logEntry, "General");
        }

        public void Critical(string Message)
        {
            Writer(TraceEventType.Critical, Message);
        }

        public void Error(string Message)
        {
            Writer(TraceEventType.Error, Message);
        }

        public void Warning(string Message)
        {
            Writer(TraceEventType.Warning, Message);
        }

        public void Information(string Message)
        {
            Writer(TraceEventType.Information, Message);
        }

        public void Verbose(string Message)
        {
            Writer(TraceEventType.Verbose, Message);
        }

    }
}
