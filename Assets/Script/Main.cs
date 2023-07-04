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
    public TMPro.TextMeshProUGUI dialogue = null;
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
            string[] token = new string[3];
            client.Run(ref token);


            string sName = token[1];
            //Debug.Log(sName);
            string sMessage = token[2];
            if (sName != null && sMessage != null)
            {
                dialogue.text = sName + " said: " + sMessage;
            }
            Debug.Log(dialogue.text);

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
