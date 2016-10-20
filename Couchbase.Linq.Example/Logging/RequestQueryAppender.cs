using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Appender;
using log4net.Core;

namespace Couchbase.Linq.Example.Logging
{
    public class RequestQueryAppender : IAppender
    {
        public string Name { get; set; }

        public void Close()
        {
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            HttpContext.Current.Items[typeof(RequestQueryAppender)] = loggingEvent;
        }

        public static string GetLastQuery(HttpContextBase context)
        {
            var ev = context.Items[typeof(RequestQueryAppender)] as LoggingEvent;

            return ev?.RenderedMessage?.Replace("Generated query: ", "");
        }
    }
}