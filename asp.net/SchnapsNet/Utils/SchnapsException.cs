using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.Utils
{

    public class SchnapsException : ApplicationException
    {
        public static SchnapsException LastException
        {
            get => (SchnapsException)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            protected set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }

        public SchnapsException Previous { get; protected set; }

        public DateTime TimeStampException { get; set; }



        public SchnapsException(string message) : base(message)
        {
            TimeStampException = DateTime.UtcNow;
            SchnapsException lastButNotLeast = (SchnapsException)LastException;
            Previous = (lastButNotLeast != null) ? (SchnapsException)lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);

            Area23Log.LogOriginMsg("SchnapsException", message);
        }

        public SchnapsException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStampException = DateTime.UtcNow;
            SchnapsException lastButNotLeast = (SchnapsException)LastException;
            Previous = (lastButNotLeast != null) ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);

            Area23Log.LogOriginMsgEx("SchnapsException", message, innerException);
        }

        public static void SetLastException(Exception exc)
        {
            SchnapsException cqrLastEx = (exc != null && exc is SchnapsException) ? (SchnapsException)exc :
                ((exc != null && exc.InnerException != null) ? new SchnapsException(exc.Message, exc.InnerException) :
                    ((exc != null && exc.Message != null) ? new SchnapsException(exc.Message) : null));

            cqrLastEx.Source = exc.Source;
            cqrLastEx.HelpLink = exc.HelpLink;
            cqrLastEx.HResult = exc.HResult;
            cqrLastEx.Previous = (SchnapsException)LastException;

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, cqrLastEx);

            Area23Log.LogOriginMsgEx("SchnapsException", cqrLastEx.Message, cqrLastEx.InnerException ?? cqrLastEx);
        }
    }

}