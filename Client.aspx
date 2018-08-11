<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Client.aspx.vb" Inherits="ws_www_p_livechat_users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .inp{
            width:300px;height:100px;
        }
        .inbox {
            background:#F0FFFF;
              font-family:Calibri ;
        font-size:18px;
        
        }
        #msgview{
     display: block;
  height: 300px;
  overflow-y: scroll;
  width : 100%
        }
    
    </style>
</head>
<body >
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
    <script type="text/javascript">
        var lastMsgDate = "2012-06-04 23:02:33.460"
       


        setInterval(function getMessages() {
            
            $.ajax({
                type: "POST",
                url: "Client.aspx/getAll",
                data: '{dateNow : "' + lastMsgDate + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
              
                    for (var j = 0; j <= data.d.length ; j++) {

                        var msg = data[Object.keys(data)[0]][j].msg

                        lastMsgDate = data[Object.keys(data)[0]][j].time

                       

                        var msglist = document.getElementById("msgview")

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
                        td.width = "300px"

                        td.appendChild(a)
                        tr.appendChild(td)
                        msglist.appendChild(tr)
                       
                        msglist.scrollTop = msglist.scrollHeight;

                    }
                

                },
                failure: function(status){alert(status) }
            })
            
           
         

        } , 1000);
       

        function sendMsg() {
            var txt = document.getElementById("inp").value
           
            $.ajax({
                type: "POST",
                url: "Client.aspx/SendMsg",
                data: '{msg: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    
                    document.getElementById("inp").value = ''
                 
                },
                failure: function(status){alert(status) }

            })
        };

    </script>

        <table id="messages"  align="center" border="1" width="20%"  cellpadding="0" cellspacing="0">
            <tr>
                <td height="300px" >

                    <table id="msgview" cellpadding="5" cellspacing="0"     >

                    </table>
                </td>
            </tr>
            <tr>
                <td><textarea type="text" runat="server" id="inp" class="inp"></textarea> </td>
            </tr>
            <tr>
                <td align="center">
                  <button id="btn" onclick="sendMsg()">Send</button>
                </td>
            </tr>
        </table>
   
</body>
</html>
