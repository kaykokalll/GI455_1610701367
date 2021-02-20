<<<<<<< HEAD
const app = require('express')();
const server = require('http').Server(app);
const websocket = require('ws');
const wss = new websocket.Server({server});

server.listen(process.env.PORT || 30000, ()=>{
    console.log("Server start at port :"+server.address().port);
});

//var wsList = [];
var roomList = [];
/*
{
    roomName: ""
    wsList: []
}
*/

wss.on("connection", (ws)=>{
    
    //Lobby
    console.log("client connected.");
    //Reception
    ws.on("message", (data)=>{
        console.log("send from client :"+ data);

        //========== Convert jsonStr into jsonObj =======

        //toJsonObj = JSON.parse(data);

        // I change to line below for prevent confusion
        var toJsonObj = { 
            roomName:"",
            data:""
        }
        toJsonObj = JSON.parse(data);
        //===============================================

        if(toJsonObj.eventName == "CreateRoom")//CreateRoom
        {
            //============= Find room with roomName from Client =========
            var isFoundRoom = false;
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    isFoundRoom = true;
                    break;
                }
            }
            //===========================================================

            if(isFoundRoom == true)// Found room
            {
                //Can't create room because roomName is exist.
                //========== Send callback message to Client ============

                //ws.send("CreateRoomFail"); 

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("client create room fail.");
            }
            else
            {
                //============ Create room and Add to roomList ==========
                var newRoom = {
                    roomName: toJsonObj.data,
                    wsList: []
                }

                newRoom.wsList.push(ws);

                roomList.push(newRoom);
                //=======================================================

                //========== Send callback message to Client ============

                //ws.send("CreateRoomSuccess");

                //I need to send roomName into client too. I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:toJsonObj.data
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
                console.log("client create room success.");
            }

            //console.log("client request CreateRoom ["+toJsonObj.data+"]");
            
        }
        else if(toJsonObj.eventName == "JoinRoom")//JoinRoom
        {
            var isFoundRoom = false;
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    isFoundRoom = true;
                    break;
                }
            }
            if(isFoundRoom == true)
            {
                
                roomList[i].wsList.push(ws);


                var callbackMsg = {
                    eventName:"JoinRoom",
                    data:"Success"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("client join room success.");
            }
            else
            {
                var callbackMsg = {
                    eventName:"JoinRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("client Join Room Fail ");
            }
            
            //================= Hint =================
            //roomList[i].wsList.push(ws);

            
        }
        else if(toJsonObj.eventName == "LeaveRoom")//LeaveRoom
        {
            //============ Find client in room for remove client out of room ================
            var isLeaveSuccess = false;//Set false to default.
            for(var i = 0; i < roomList.length; i++)//Loop in roomList
            {
                for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                {
                    if(ws == roomList[i].wsList[j])//If founded client.
                    {
                        roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                        if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                        {
                            roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                        }
                        isLeaveSuccess = true;
                        break;
                    }
                }
            }
            //===============================================================================

            if(isLeaveSuccess)
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomSuccess");

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"success"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room success");
            }
            else
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomFail");

                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room fail");
            }
        }
    });


    /*wsList.push(ws);
    
    ws.on("message", (data)=>{
        console.log("send from client :"+ data);
        Boardcast(data);
    });
    */
    ws.on("close", ()=>{
        console.log("client disconnected.");

        //============ Find client in room for remove client out of room ================
        for(var i = 0; i < roomList.length; i++)//Loop in roomList
        {
            for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
            {
                if(ws == roomList[i].wsList[j])//If founded client.
                {
                    roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                    if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                    {
                        roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                    }
                    break;
                }
            }
        }
        //===============================================================================
    });
});

function Boardcast(data)
{
    /*for(var i = 0; i < wsList.length; i++)
    {
        wsList[i].send(data);
    }*/
}

=======
var websocket = require('ws');

var websocketServer = new websocket.Server({port:30000}, ()=>{
    console.log("guy server is running");
});

var wsList = [];

websocketServer.on("connection",(ws, rq)=>{
    console.log('client connected');

    wsList.push(ws);

    ws.on("message", (data)=>{

        console.log("send from client :"+data);
        Boardcast(data);
    });

    ws.on("close",()=>{

        wsList = ArrayRemove(wsList,ws);
        console.log("client disconnected");
    });
});

function ArrayRemove(arr, value)
{
    return arr.filter((element)=>{
        return element != value;
    });
}

function Boardcast(data)
{
    for(var i =0; i < wsList.length;i++)
    {
        wsList[i].send(data);
    }
}
>>>>>>> 1670c965c2b627f12a2d60ed5598e51e82c05ab7
