using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Services
{
    public interface IConfirmationService
    {
        bool Confirm(string message, string caption);
    }
}
