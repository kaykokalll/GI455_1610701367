using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

public class Connect : MonoBehaviour
{

    private WebSocket websocket;
    public InputField ip;
    public InputField sendmessage;
    public Text showmessage;
    public GameObject conui;
    public GameObject chatui;

    private void Start()
    {
        
        conui.SetActive(true);
        chatui.SetActive(false);
    }

    public void Con()
    {
        websocket = new WebSocket("ws://"+ip.text+":30000/");

        websocket.OnMessage += OnMessage;
        

        websocket.Connect();

        conui.SetActive(false);
        chatui.SetActive(true);
    }

    
    public void SendMassage()
    {
        
        websocket.Send(sendmessage.text);         
    }

    public void OnDestroy()
    {
        if (websocket != null)
        {
            websocket.Close();
        }
        
    }
    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {               
         showmessage.text +="\n"+ messageEventArgs.Data;                     
    }
    

}
