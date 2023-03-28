using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagerClient.WS
{
    public interface IWSMessageHandler
    {
        public void onMessageResive(StompMessage message);
    }
}
