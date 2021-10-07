using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasSite.HttpContext
{
    public interface IUserContext
    {
        public bool IsAuthenticated { get; }
        public string GetUserEmail();
    }
}
