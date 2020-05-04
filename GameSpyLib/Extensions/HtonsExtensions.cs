﻿using System;
using System.Net;

namespace GameSpyLib.Extensions
{
    public class HtonsExtensions
    {
        public static byte[] PortToHtonUshortBytes(ushort port)
        {
            byte[] buffer = BitConverter.GetBytes(port);
            Array.Reverse(buffer);
            return buffer;
        }

        public static byte[] PortToHtonUshortBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return PortToHtonUshortBytes(port);
        }

        public static byte[] PortToUshortBytes(string portStr)
        {
            ushort port = ushort.Parse(portStr);
            return PortToUshortBytes(port);
        }
        public static byte[] PortToUshortBytes(ushort port)
        {
            return BitConverter.GetBytes(port);
        }

        public static string EndPointToIP(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.ToString();
        }
        public static string EndPointToPort(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Port.ToString();
        }

        public static string BytesToIPString(byte[] ip)
        {
            return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }

        public static byte[] IPStringToBytes(string ip)
        {
            return IPAddress.Parse(ip).GetAddressBytes();
        }

        public static byte[] IPToBytes(int ip)
        {
            return BitConverter.GetBytes(ip);
        }

        public static ushort HtonBytesToUshortPort(byte[] buffer)
        {
            Array.Reverse(buffer);
            return BytesToUshortPort(buffer);
        }

        public static ushort BytesToUshortPort(byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer);
        }

        public static byte[] PortToIntBytes(string port)
        {
            return PortToIntBytes(int.Parse(port));
        }

        public static byte[] PortToIntBytes(int port)
        {
            return BitConverter.GetBytes(port);
        }
    }
}
