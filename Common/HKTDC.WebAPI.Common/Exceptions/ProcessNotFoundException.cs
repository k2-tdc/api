using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Exceptions
{
    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException() : base()
        {

        }

        public ProcessNotFoundException(string message) : base(message)
        {

        }
    }
}