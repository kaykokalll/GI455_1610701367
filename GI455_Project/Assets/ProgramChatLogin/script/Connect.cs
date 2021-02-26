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

     class MessageData
     {
         public string server;
         public string message;
     }

    
    public InputField sendmessage;
    public Text sendText;
    public Text receiveText;
    public Text roomNameUi;
    public Text nameUser;
    public GameObject conui;
    public GameObject chatui;
    public GameObject menu;
    public GameObject create;
    public GameObject join;
    public GameObject popupCreate;
    public GameObject login;
    public GameObject register;
    public GameObject loginFail;
    public GameObject regisFail;
    public InputField roomnameCreate;
    public InputField roomnameJoin;
    public InputField id;
    public InputField password;
    public InputField idRegis;
    public InputField passwordRegis;
    public InputField rePassword;
    public InputField userName;
    

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
        login.SetActive(false);
        register.SetActive(false);
        loginFail.SetActive(false);
        regisFail.SetActive(false);

    }

    private void Update()
    {
        UpdateNotifyMessage();
        if (string.IsNullOrEmpty(tempMessageString) == false)
        {
         MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);
            if (receiveMessageData.server =="127.0.0.1")
        {
        sendText.text +=  receiveMessageData.message + "\n";
        receiveText.text += "\n";
        }
        else
        {
        receiveText.text += receiveMessageData.message + "\n";
        receiveText.text += "\n";
        }
        Debug.Log(receiveMessageData.server);
        tempMessageString = "";
        }
    
        UpdateNotifyMessage();
        
    }
    public void Login(string idPass)
    {

        idPass = id.text + "#" + password.text;
        SocketEvent socketEvent = new SocketEvent("Login", idPass);
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);
      
    }
    public void Register(string regisData)
    {
    
        regisData = idRegis.text + "#" + passwordRegis.text+ "#" +rePassword.text+ "#" +userName.text;
        SocketEvent socketEvent = new SocketEvent("Register", regisData);
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);
        

    }
    public void CreateRoom(string roomName )
    {
        
        roomName = roomnameCreate.text; ;
        SocketEvent socketEvent = new SocketEvent("CreateRoom",roomName);      
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);

        roomNameUi.text = roomName;      
    }

    public void JoinRoom(string joinroomName)
    {
        joinroomName = roomnameJoin.text;
        SocketEvent socketEvent = new SocketEvent("JoinRoom",joinroomName);
        string toJsonStr = JsonUtility.ToJson(socketEvent);
        ws.Send(toJsonStr);

        roomNameUi.text = joinroomName;
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

        ws = new WebSocket("ws://127.0.0.1:30000/");

        ws.OnMessage += OnMessage;


        ws.Connect();

        conui.SetActive(false);
        chatui.SetActive(false);
        menu.SetActive(false);
        create.SetActive(false);
        join.SetActive(false);
        login.SetActive(true);
        register.SetActive(false);
        loginFail.SetActive(false);
        regisFail.SetActive(false);
    }
 
    public void SendMassage()
    {

        if (sendmessage.text == "" || ws.ReadyState != WebSocketState.Open)
            return;

        MessageData messageData = new MessageData();        
        messageData.message = sendmessage.text;

        string toJsonStr = JsonUtility.ToJson(messageData);

        ws.Send(toJsonStr);
        sendmessage.text = "";
    }

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
            else if(receiveMessageData.eventName == "Login")
            {
                if(receiveMessageData.data == "fail")
                {
                    failLogin();
                    
                }
                else
                {
                    nameUser.text = receiveMessageData.data;
                    toMenu();
                }
            }
            else if (receiveMessageData.eventName == "Register")
            {
                if (receiveMessageData.data == "fail")
                {
                    failRegister();
                }
                else
                {
                    toLogin();
                }
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
    public void failLogin()
    {
        loginFail.SetActive(true);
    }
    public void failRegister()
    {
        regisFail.SetActive(true);
    }
    public void toMenu()
    {
        menu.SetActive(true);
        login.SetActive(false);
    }
    public void toLogin()
    {
        register.SetActive(false);
        login.SetActive(true);
        
    }
    public void toRegister()
    {
        login.SetActive(false);
        register.SetActive(true);
        
    }
    public void okLogin()
    {
        loginFail.SetActive(false);
    }
    public void okRegis()
    {
        loginFail.SetActive(false);
    }
}
