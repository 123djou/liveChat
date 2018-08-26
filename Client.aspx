<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Client.aspx.vb" Inherits="ws_www_p_livechat_users" %>
 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>live chat apps | Client</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        .messages{

            border:1px solid gray;
            box-shadow: -10px -10px 15px #cfd0d1;
            border-radius :5px;
            padding: 20px 0 30px 0;
            width:450px;
            height:500px;
            position:fixed;
            right:0;
            bottom:35px;
            display:block;
            display:flex;
            justify-content:center;

            }
       #msgview td{
            width:400px;
        }
       
        .inp{
            width:420px;
            height:100px;
            border:1px solid gray;
            resize:none;
        }
        .inbox {
            background:#F0FFFF;
              font-family:Calibri ;
        font-size:18px;
        
        }
               @media (max-width:500px){
.messages{
    width:100%;
    height:100%;
    margin:auto;
}
.inp{
    width:300px;
}
        }
        #msgview{
  
  height: 300px;
  overflow-y: auto;
  width : 100%;
  padding : 0 10px 40px 0;
  display:block;
  position:relative;

        }
    #btn{
        color:white;
        background:#2583e8;
        width:70px;
        height:30px;
        border:0;
        border-radius:5px;
        font-family:Arial;
        font-size:16px;
       float:right;
       margin:0 10px;
       cursor:pointer;

    }
    .left{
        text-align:left;
    }
    .right{
        text-align:right
    }
    #chat{
        width:200px;
        height:35px;
         background:#1766b5;
         color:white;
         border:2px outset #79afe5;
         border-radius: 5px 5px 0  0  ;
         font-family:arial;
         font-size:18px;
         position:fixed;
         right:10px;
         bottom:0;
         cursor:pointer;
    }
    </style>
</head>
<body>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
 
    <button id="chat">Live chat</button>
        <table id="messages" class="messages"  align="center" >
            <tr>
                <td>

                    <table id="msgview"  >

                    </table>
                </td>
            </tr>
            <tr>
                <td align="center"><textarea type="text" runat="server" id="inp" class="inp"></textarea> </td>
            </tr>
            <tr>
                <td >
                  <button id="btn" onclick="sendMsg()">Send</button>
                </td>
            </tr>
        </table>
      <script type="text/javascript">
          let count = 0

          let begin = 0

          $(function () {
              $("#messages").hide()
              
              $("#chat").click(function () {
                  begin = 1

                  $("#messages").fadeToggle()
                 

              })
          }
          )


          setInterval(function getMessages() {

              
                  $.ajax({
                      type: "POST",
                      url: "Client.aspx/getAll",
                      data: '{count : "' + count + '"}',
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (data) {

                          if (data.d.length > count) {
                              count = data.d.length
                              begin = count
                          } else if (data.d.length != 0) {

                              count += data.d.length
                          }

                          for (var j = 0; j <= data.d.length ; j++) {


                              var msg = data[Object.keys(data)[0]][j].msg

                              var msglist = document.getElementById("msgview")

                              var tr = document.createElement("tr")

                              var td = document.createElement("td")

                              var txt = document.createTextNode(msg)

                              var a = document.createElement("label")

                              a.className = "inbox"

                              a.appendChild(txt)

                              if (data[Object.keys(data)[0]][j].dist == "admin") {

                                  td.classList.add("left")
                              } else {
                                  td.classList.add("right")
                              }
                              td.width = "100%"

                              td.appendChild(a)
                              tr.appendChild(td)
                              msglist.appendChild(tr)

                              msglist.scrollTop = msglist.scrollHeight;

                          }
                      }
                  })
              



          }, 1000);


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

                  }

              })
          };

    </script>

</body>
</html>
