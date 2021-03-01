using System;
using System.Net.NetworkInformation;
using UnityEngine;

public static class Connection
{
    public static bool IsDeviceConnected()
    {
        return CheckConnection();
    }

    private static bool CheckConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
