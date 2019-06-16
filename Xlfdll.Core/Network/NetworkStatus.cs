using System;
using System.Net.NetworkInformation;

namespace Xlfdll.Network
{
    public static class NetworkStatus
    {
        public static NetworkInterface GetCurrentActiveNetworkInterface()
        {
            NetworkInterface currentNetworkInterface = null;
            Int64 maxBytesSent = 0;

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up && item.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    IPInterfaceStatistics ipStatistics = item.GetIPStatistics();

                    if (ipStatistics.BytesSent > maxBytesSent)
                    {
                        currentNetworkInterface = item;
                        maxBytesSent = ipStatistics.BytesSent;
                    }
                }
            }

            return currentNetworkInterface;
        }
    }
}