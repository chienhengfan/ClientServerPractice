using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoom;


public class Main : MonoBehaviour
{
    public TMPro.TMP_InputField adressInput = null;
    public TMPro.TMP_InputField nameInput = null;
    public TMPro.TMP_InputField messageInput = null;
    public TMPro.TMP_Text dialogue = null;
    ChatClient client = new ChatClient();
    public bool ConnectSuccess;
    public int port = 4099;

    // Start is called before the first frame update
    void Awake()
    {
        //ChatRoom.ChatServer server = new ChatServer();
        //server.Bind(port);
        //server.Run();
    }

    private void Update()
    {
        if (ConnectSuccess)
        {
            string[] token = null;
            client.Run(ref token);

            if(token != null)
            {
                string sName = token[1];
                string sMessage = token[2];
                dialogue.text = sName + " said: " + sMessage;
            }
            else
            {
                Debug.Log(token);
            }
        }

        
    }

    public void ClickButton()
    {
        string address = adressInput.text;
        string name = nameInput.text;;

        ConnectSuccess = client.Connect(address, port);
        client.SendName(name);
        if (ConnectSuccess)
        {
            Debug.Log("connect server success!");
            Debug.Log($"your name is : {name}");
        }
        
    }

    public void ClickSendMessage()
    {
        if (ConnectSuccess)
        {
            string message = messageInput.text;
            client.SendBroadcast(message);
        }
        
    }
}
