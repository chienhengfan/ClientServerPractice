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
    ChatClient client = new ChatClient();
    public bool IsSuccess;
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

    }

    public void ClickButton()
    {
        string address = adressInput.text;
        string name = nameInput.text;;

        IsSuccess = client.Connect(address, port);
        client.SendName(name);
        if (IsSuccess)
        {
            Debug.Log("connect server success!");
            Debug.Log($"your name is : {name}");
        }
        
    }

    public void ClickSendMessage()
    {
        if (IsSuccess)
        {
            string message = messageInput.text;
            client.SendBroadcast(message);
        }
        
    }
}
