using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.InterceptionExtension
{
   public class TraceCallHandler : ICallHandler, IDisposable
    {
        private TraceSource source;

        public TraceCallHandler(TraceSource source)
        {
            Guard.ArgumentNotNull(source, "source");
            this.source = source;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            this.source.TraceInformation(
                "Invoking {0}",
                input.MethodBase.ToString());

            IMethodReturn methodReturn = getNext().Invoke(input, getNext);

            if (methodReturn.Exception == null)
            {
                this.source.TraceInformation(
                    "Successfully finished {0}",
                    input.MethodBase.ToString());
            }
            else
            {
                this.source.TraceInformation(
                    "Finished {0} with exception {1}: {2}",
                    input.MethodBase.ToString(),
                    methodReturn.Exception.GetType().Name,
                    methodReturn.Exception.Message);
            }

            this.source.Flush();

            return methodReturn;
        }

        private int order;
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        public void Dispose()
        {
            this.source.Close();
        }
    }

   public class TraceCallHandlerAttribute : HandlerAttribute
    {
        private string sourceName;

        public TraceCallHandlerAttribute(string sourceName)
        {
            Guard.ArgumentNotNull(sourceName, "sourceName");
            this.sourceName = sourceName;
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new TraceCallHandler(new TraceSource(this.sourceName));
        }
    }
}
