<%@ Page Language="VB" AutoEventWireup="false" CodeFile="admin.aspx.vb" Inherits="ws_www_p_livechat_admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{
            font-family:Calibri 
        }
        #ConvList{
            float : left ;
            width:400px;
            height:100%;
         border:1px solid ;
         margin-top:10px;
          cursor :pointer 
        }
      #ConvList  td {
            border-bottom: 1px solid;
            padding : 5px;
            width:auto;
           
        }
        .time{
            font-size:12px;
            color:lightgray 
        }
        .msg{
            font-size:16px;
            color:gray

        }
        .inp{
            width:700px;
            height:100%;
            border: 0;
            border-bottom: 1px solid lightblue;
                 border-top: 1px solid lightblue
        }
    #messages{

        border: 1px solid lightblue;
        border-radius: 25px;
        padding: 10px;
       
        
    }
    .inbox{

        background: #F0FFFF;
          font-family:Calibri ;
        font-size:18px;
       

    }
    #msgview{
    display: block;
    height: 300px;
    overflow-y: scroll;
    width : 100%;
    padding : 1px 10px
    }
    </style>
</head>
<body>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
    <script type="text/javascript"  >
        var ConvID = ""
        var lastMsgDate = "2012-06-04 23:02:33.460"

        var begin = "0"

        function getConv(event) {

            ConvID = event.id
            
            $('#msgview >tr').remove()
            
            $.ajax({
                type: "POST",
                url: "admin.aspx/getMesages",
                data: '{id: "' + ConvID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    for (var j = 0; j < data.d.length ; j++) {

                        var msg = data[Object.keys(data)[0]][j].msg

                        var msglist = document.getElementById("msgview")

                        lastMsgDate = data[Object.keys(data)[0]][j].time

                        var tr = document.createElement("tr")

                        var td = document.createElement("td")

                        var txt = document.createTextNode(msg)

                        var a =  document.createElement("label")

                        a.className = "inbox"

                        a.appendChild(txt)
                      
                        if (data[Object.keys(data)[0]][j].dist == "admin") {

                            td.setAttribute("align", "right")
                        } else {
                            td.setAttribute("align", "left")
                        }
                        td.width = "500px"
                        td.appendChild(a)
                        tr.appendChild(td)
                        msglist.appendChild(tr)
                        begin = "1"

                    }

                }
            })

        };


        setInterval(function getMessages() {
            if (begin != "0") {
               
                $.ajax({
                    type: "POST",
                    url: "admin.aspx/getAll",
                    data: '{dateNow : "' + lastMsgDate + '",convID : "' + ConvID + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                      
                        for (var j = 0; j <= data.d.length ; j++) {

                            var msg = data[Object.keys(data)[0]][j].msg

                            var msglist = document.getElementById("msgview")

                            lastMsgDate = data[Object.keys(data)[0]][j].time

                            var tr = document.createElement("tr")

                            var td = document.createElement("td")

                            var txt = document.createTextNode(msg)

                            var a = document.createElement("label")

                            a.className = "inbox"

                            a.appendChild(txt)

                            if (data[Object.keys(data)[0]][j].dist == "admin") {

                                td.setAttribute("align", "right")
                            } else {
                                td.setAttribute("align", "left")
                            }
                            td.width = "500px"
                            td.appendChild(a)
                            tr.appendChild(td)
                            msglist.appendChild(tr)
                            msglist.scrollTop = msglist.scrollHeight;

                        }
                    },
                    failure: function (status) { alert(status) }
                })
            }
           
        }, 1000);
        $(document).ready(function () {

  
        $("#btn").click(function () {

            var text = $("#inp").val()

           
            $.ajax({
               
                type: "POST",
                url: "admin.aspx/sendMesage",
                data: '{id: "' + ConvID + '",msg: "' + text + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                  
                    document.getElementById("inp").value = ''

                 getMessages()
                },
                failure: function (status) { alert(status) }
            })
        
        })
        })
        
    </script>

    
 <table id="ConvList" runat="server" cellpadding="0" cellspacing="0" >
     <tr>
         <td><b>Client</b></td>
         <td><b>Last Message</b></td>
         <td><b>Time</b></td>
     </tr>
 </table>

 <table id="messages"  align="center"  width="700" height="500" cellpadding="0" cellspacing="0">
            <tr>
                <td  width="300px" >
                    <table id="msgview" width="300px">

                    </table>

                </td>
            </tr>
            <tr>
                <td height="100" ><textarea   id="inp" class="inp"></textarea> </td>
            </tr>
            <tr>
                <td height="50" align="center">
                  <button id="btn">Send</button>
                </td>
            </tr>
        </table>
    
</body>
</html>
