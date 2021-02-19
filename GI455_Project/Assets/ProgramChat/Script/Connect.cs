using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System;

public class Connect : MonoBehaviour
{

    public struct SocketEvent
    {
        public string eventName;
        public string data;

        public SocketEvent(string eventName, string data)
        {
            this.eventName = eventName;
            this.data = data;
        }
    }

    /*class MessageData
    {
        public string server;
        public string message;
    }*/

    public InputField ip;
    public InputField sendmessage;
    public Text sendText;
    public Text receiveText;
    public Text roomNameUi;
    public GameObject conui;
    public GameObject chatui;
    public GameObject menu;
    public GameObject create;
    public GameObject join;
    public GameObject popupCreate;
    public InputField roomnameCreate;
    public InputField roomnameJoin;

    
    
    private string tempMessageString;
    private WebSocket ws;

    public delegate void DelegateHandle(SocketEvent result);
    public DelegateHandle OnCreateRoom;
    public DelegateHandle OnJoinRoom;
    public DelegateHandle OnLeaveRoom;

    private void Start()
    {

        

        conui.SetActive(true);
        chatui.SetActive(false);
        menu.SetActive(false);
        create.SetActive(false);
        join.SetActive(false);
        popupCreate.SetActive(false);
    }

    private void Update()
    {
        //if (string.IsNullOrEmpty(tempMessageString) == false)
        //{
        // MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);
        //if (receiveMessageData.server == ip.text)
        //{
        //sendText.text +=  receiveMessageData.message + "\n";
        //receiveText.text += "\n";
        //}
        //else
        //{
        //receiveText.text += receiveMessageData.message + "\n";
        //receiveText.text += "\n";
        //}

        //tempMessageString = "";
        //}
        UpdateNotifyMessage();
        

    }

    public void CreateRoom(string roomName )
    {
        
        roomName = roomnameCreate.text; ;
        SocketEvent socketEvent = new SocketEvent("CreateRoom",roomName);      
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);

        roomNameUi.text = roomName;
        /*  if (roomNameUi.text== roomName)
              {
                  Bcreateroom2();
              }
              roomNameUi.text = roomName;
              if (roomNameUi.text== roomName)
              {
                  fail();
              }*/

    }


    public void JoinRoom(string joinroomName)
    {
        
        joinroomName = roomnameJoin.text;
        SocketEvent socketEvent = new SocketEvent("JoinRoom",joinroomName);
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);

        roomNameUi.text = joinroomName;

        /*  if (socketEvent.data == roomNameUi.text)
            {
                Bjoinroom2();
            }

            if(roomNameUi.text != joinroomName)
            {
                fail();
            }
            roomNameUi.text = joinroomName;*/
    }

    public void LeaveRoom()
    {
        SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");      
        string toJsonStr = JsonUtility.ToJson(socketEvent);      
        ws.Send(toJsonStr);       
        Bleave();     
    }

    public void Con()
    {
        ws= new WebSocket("ws://"+ip.text+":30000/");

        ws.OnMessage += OnMessage;
        

        ws.Connect();

        conui.SetActive(false);
        chatui.SetActive(false);
        menu.SetActive(true);
        create.SetActive(false);
        join.SetActive(false);
    }

    
    //public void SendMassage()
    //{

        //if (sendmessage.text == "" || ws.ReadyState != WebSocketState.Open)
            //return;

        //MessageData messageData = new MessageData();        
        //messageData.message = sendmessage.text;

        //string toJsonStr = JsonUtility.ToJson(messageData);

        //ws.Send(toJsonStr);
        //sendmessage.text = "";
    //}

    public void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }       
    }

    private void UpdateNotifyMessage()
    {
        if (string.IsNullOrEmpty(tempMessageString) == false)
        {
            SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

            if (receiveMessageData.eventName == "CreateRoom")
            {
                if (OnCreateRoom != null)
                    OnCreateRoom(receiveMessageData);
                if(receiveMessageData.data == "fail")
                {
                    fail();
                }
                else
                {
                    Bcreateroom2();
                }
                                                                                    
            }
            else if (receiveMessageData.eventName == "JoinRoom")
            {
                if (OnJoinRoom != null)
                    OnJoinRoom(receiveMessageData);
                if(receiveMessageData.data == "fail")
                {
                    fail();
                }
                else
                {
                    Bjoinroom2();
                }
            }
            else if (receiveMessageData.eventName == "LeaveRoom")
            {
                if (OnLeaveRoom != null)
                    OnLeaveRoom(receiveMessageData);
            }

            tempMessageString = "";
        }
    }


    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        
        tempMessageString = messageEventArgs.Data;
        Debug.Log(messageEventArgs.Data);
            
    }

    public void Bcreateroom()
    {
        menu.SetActive(false);
        create.SetActive(true);
    }
    public void Bjoinroom()
    {
        menu.SetActive(false);
        join.SetActive(true);
    }
    public void Bleave()
    {     
        chatui.SetActive(false);
        menu.SetActive(true);      
    }
    public void Bcreateroom2()
    {
        create.SetActive(false);
        chatui.SetActive(true);
    }
    public void Bjoinroom2()
    {
        join.SetActive(false);
        chatui.SetActive(true);
    }
    public void fail()
    {
        popupCreate.SetActive(true);
    }
    public void ok()
    {
        popupCreate.SetActive(false);
    }
}
