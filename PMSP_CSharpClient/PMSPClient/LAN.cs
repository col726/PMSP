/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace PMSPClient
{
    /// <summary>
    /// This class interacts with the local area network in order to obtain IP information.
    /// </summary>
    public static class LAN
    {
        /// <summary>
        /// Gets all valid LAN IPv4 addresses into list.
        /// </summary>
        public static List<string> GetActiveIp4Addresses()
        {
            //Get IPv4 Base from local host.
            string ipv4Base = GetIPv4Base();

            //Declare list variable to store all active IPv4 addresses.
            List<string> activeIPv4Addresses = new List<string>();

            //Ping all possible addresses
            for (int i = 1; i < 255; i++)
            {
                //Set server address.
                string address = ipv4Base + i.ToString();

                //Write new line.
                Utilities.WriteNewLine();

                //Inform user of ping.
                Console.Write("Pinging " + address + "...");

                //Instantiate new ping object.
                Ping ping = new Ping();

                //Send ping request with a 10 second timeout to current IPv4 address.
                PingReply pingReply = ping.Send(address, 100);

                //If we pinged the current IPv4 address, successfully, add to list.
                if (pingReply.Status == IPStatus.Success)
                {
                    //Success.
                    Console.Write("Success!");

                    //Add current IPv4 address to list.
                    activeIPv4Addresses.Add(ipv4Base + i.ToString());
                }
                //Inform user of failed ping.
                else
                {
                    //Success.
                    Console.Write("Failed.");
                }
            }

            //Return list of active IPv4 addresses.
            return activeIPv4Addresses;
        }

        /// <summary>
        /// This function obtains the first 3 octets of the LANs IPv4 address schema.
        /// </summary>
        /// <returns>IP Base string.</returns>
        private static string GetIPv4Base()
        {
            //Declare variable for the first local IPv4 address we encounter.
            string localIPv4 = "";

            //Get local host name.
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            //Loop through address list provided by local host and break when we find the first local IPv4 address.
            foreach (IPAddress ip in host.AddressList)
            {
                //Only look for IPv4.
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    //Set local IP.
                    localIPv4 = ip.ToString();
                    break;
                }
            }

            //Split local IP into octet array.
            string[] ipParts = localIPv4.Split('.');

            //Return first 3 octets as the base address.
            return ipParts[0] + "." + ipParts[1] + "." + ipParts[2] + ".";
        }
    }
}