using System;
using System.Net.NetworkInformation;

public static class Connection
{
    private static readonly System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
    private static readonly string host = "google.com";
    private static readonly byte[] buffer = new byte[32];
    private static readonly int timeout = 1000;
    private static readonly PingOptions pingOptions = new PingOptions();

    public static bool IsDeviceConnected()
    {
        return CheckConnection();
    }

    private static bool CheckConnection()
    {
        try
        {
            PingReply reply = ping.Send(host, timeout, buffer, pingOptions);
            return reply.Status == IPStatus.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
