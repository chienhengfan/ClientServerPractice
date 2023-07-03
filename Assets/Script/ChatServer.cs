using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;

namespace ChatRoom
{
    public class ChatServer
    {

        UdpClient m_theSocket = null;
        Dictionary<IPEndPoint, string> m_dicUDPClientNames = new Dictionary<IPEndPoint, string>();
        public ChatServer()
        {
        }

        public void Bind(int iPort)
        {
            m_theSocket = new UdpClient(iPort);
            Debug.Log("Chat Server port " + iPort + " binded...");
        }

        public void Run()
        {
            while (true)
            {

                int iNumBytes = m_theSocket.Available;
                if (iNumBytes > 0)
                {
                    _HandleReceiveMessages();
                }
            }
        }

        void _HandleReceiveMessages()
        {
            IPEndPoint thePeerEP = null;
            byte[] aBuffer = m_theSocket.Receive(ref thePeerEP);

            string sRequest = System.Text.Encoding.ASCII.GetString(aBuffer);

            if (sRequest.StartsWith("LOGINNAME:", StringComparison.OrdinalIgnoreCase))
            {
                string[] aTokens = sRequest.Split(':');
                m_dicUDPClientNames.Add(thePeerEP, aTokens[1]);
                Debug.Log("...and the client name is: " + aTokens[1]);
            }
            else if (sRequest.StartsWith("BROADCAST:", StringComparison.OrdinalIgnoreCase))
            {
                string[] aTokens = sRequest.Split(':');
                string sMessage = aTokens[1];
                foreach (IPEndPoint ep in m_dicUDPClientNames.Keys)
                {

                    if (thePeerEP.ToString() != ep.ToString())
                    {
                        string sMsgRequest = "MESSAGE:" + m_dicUDPClientNames[thePeerEP] + ":" + sMessage;
                        byte[] aRequestBuffer = System.Text.Encoding.ASCII.GetBytes(sMsgRequest);

                        m_theSocket.Send(aRequestBuffer, aRequestBuffer.Length, ep);
                    }
                }
            }
        }
    }
}
