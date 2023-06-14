using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Globalization;
using SchnapsNet.ConstEnum;
using System.Runtime.Serialization;
using System.Web.SessionState;

namespace SchnapsNet.Models
{
    [Serializable]
    public class SchnapsException : ApplicationException
    {

        #region ctor
        /// <summary>
        /// Standard empty default constructor
        /// </summary>
        public SchnapsException() : base() { }

        /// <summary>
        /// constructor for SchnapsException
        /// </summary>
        /// <param name="message">exception message</param>
        public SchnapsException(string message) : base(message) { }

        /// <summary>
        /// constructor for SchnapsException
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception beyond</param>
        public SchnapsException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// constructor for SchnapsException
        /// </summary>
        /// <param name="info"><see cref="System.Runtime.Serialization.SerializationInfo">System.Runtime.Serialization.SerializationInfo</see></param>
        /// <param name="context"><see cref="System.Runtime.Serialization.StreamingContext">System.Runtime.Serialization.StreamingContext</see></param>
        public SchnapsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion ctor

        #region members

        public virtual ApplicationException GetApplicationException()
        {
            return (!string.IsNullOrWhiteSpace(this.Message) || this.InnerException != null) ?
                new ApplicationException(this.Message, this.InnerException) : (ApplicationException)this;
        }

        public virtual Exception GetException()
        {
            return (!string.IsNullOrWhiteSpace(this.Message) || this.InnerException != null) ?
                new Exception(this.Message, this.InnerException) : null;
        }

        #endregion members
    
    }

    [Serializable]
    public class InvalidSchnapsStateException : SchnapsException
    {

        #region ctor
        /// <summary>
        /// Standard empty default constructor
        /// </summary>
        public InvalidSchnapsStateException() : base() { }

        /// <summary>
        /// constructor for InvalidSchnapsStateException
        /// </summary>
        /// <param name="message">exception message</param>
        public InvalidSchnapsStateException(string message) : base(message) { }

        /// <summary>
        /// constructor for SchnapsException
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception beyond</param>
        public InvalidSchnapsStateException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// constructor for SchnapsException
        /// </summary>
        /// <param name="info"><see cref="System.Runtime.Serialization.SerializationInfo">System.Runtime.Serialization.SerializationInfo</see></param>
        /// <param name="context"><see cref="System.Runtime.Serialization.StreamingContext">System.Runtime.Serialization.StreamingContext</see></param>
        public InvalidSchnapsStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion ctor

        // members virtual inherited from SchnapsException
    }

}