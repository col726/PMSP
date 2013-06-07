using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSPClient
{
    public enum Status
    {
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        InvalidStateTransition = 442,
        InternalServerError = 500,
        NotImplemented = 501
    }
}