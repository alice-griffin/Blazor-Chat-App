using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Pages
{
    public partial class Clock : IDisposable
    {
        public void Dispose()
        {
            if (!IsFirstRender)
            {
                Timer?.Dispose();
            }
        }
    }
}
