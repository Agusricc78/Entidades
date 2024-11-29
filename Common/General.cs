using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class General
    {
        public static string GetIp()
        {
            var retvalue = string.Empty;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
                retvalue = ip.ToString();

            if (string.IsNullOrEmpty(retvalue))
            {
                var ip = host.AddressList.FirstOrDefault();
                retvalue = ip == null ? "" : ip.ToString();
            }
            return retvalue;
        }



    }
}
