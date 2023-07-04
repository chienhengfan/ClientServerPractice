using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Linq;
using System.Net;

namespace ChatRoom
{
    public class ChatClient
    {
        UdpClient m_theClient = null;
        IPEndPoint m_theEndPoint = null;
        public ChatClient()
        {
        }

        public bool Connect(string sAddress, int iPort)
        {
            m_theClient = new UdpClient();

            try
            {
                IPHostEntry host = Dns.GetHostEntry(sAddress);
                var address = (from h in host.AddressList where h.AddressFamily == AddressFamily.InterNetwork select h).First();

                m_theEndPoint = new IPEndPoint(address, iPort);
                //m_theClient.Connect( address.ToString(), iPort ); <-- UdpClient also provides connect function as a default end point setting
                //Console.WriteLine( "Connected to Chat Server: " + sAddress + ":" + iPort + "\n" );

                return true;
            }
            catch (Exception e)
            {
                Debug.Log("Exception happened: " + e.ToString());
                return false;
            }
        }

        public void SendName(string sName)
        {
            string sRequest = "LOGINNAME:" + sName;
            byte[] aRequestBuffer = System.Text.Encoding.UTF8.GetBytes(sRequest);

            m_theClient.Send(aRequestBuffer, aRequestBuffer.Length, m_theEndPoint);
        }
        public void SendBroadcast(string sMessage)
        {
            string sRequest = "BROADCAST:" + sMessage;
            byte[] aRequestBuffer = System.Text.Encoding.UTF8.GetBytes(sRequest);

            m_theClient.Send(aRequestBuffer, aRequestBuffer.Length, m_theEndPoint);
        }

        public void Run(ref string[] reciever)
        {
            if (m_theClient.Available > 0)
            {
                _HandleReceiveMessages(m_theClient,ref reciever);
            }
        }

        public void _HandleReceiveMessages(UdpClient theClient, ref string[] reciever)
        {
            IPEndPoint ep = null;
            byte[] aBuffer = theClient.Receive(ref ep);

            string sRequest = System.Text.Encoding.ASCII.GetString(aBuffer);

            if (sRequest.StartsWith("MESSAGE:", StringComparison.OrdinalIgnoreCase))
            {
                string[] aTokens = sRequest.Split(':');
                string sName = aTokens[1];
                string sMessage = aTokens[2];
                Debug.Log(sName + " said: " + sMessage);
                Debug.Log(aTokens);
                Debug.Log(reciever);
                //aTokens.CopyTo(reciever, 0);
            }
        }
    }
}
