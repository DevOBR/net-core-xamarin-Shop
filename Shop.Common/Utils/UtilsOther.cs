namespace Shop.Common.Utils
{
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Contains differents types of methods to get or transform generic normal forms
    /// </summary>
    public class UtilsOther
    {
        /// <summary>
        /// Get ip address from your hosname
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAdress = string.Empty;

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAdress = ip.ToString();
                }
            }

            return ipAdress;
        }
    }
}
