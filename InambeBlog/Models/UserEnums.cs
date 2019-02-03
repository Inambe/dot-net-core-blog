using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InambeBlog.Models
{
    public enum SignInViewState
    {
        Failed,
        NotActive,
        AccountCreated,
        ActivationSuccess,
        ExpiredToken,
        InvalidToken
    }
}
