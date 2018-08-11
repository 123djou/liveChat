Imports System.Data.SqlClient
Imports System.Data

Partial Class ws_www_p_livechat_admin
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim list As New List(Of conversation)

        Dim conv As New conversation()


        list = conv.getAll()

        For Each v In list

            Dim tr As New HtmlTableRow
            Dim td, td1, td2 As New HtmlTableCell

            Dim label, label1, label2 As New Label

            Dim msg As New Message()

            msg = v.getLastMsg(v.id)

            label.Text = v.client

            If msg.msg.Length > 20 Then

                label1.Text = msg.msg.Substring(0, 15) & "..."

            Else

                label1.Text = msg.msg
            End If

            label2.Text = msg.time

            label.CssClass = "client"
            label1.CssClass = "msg"

            label2.CssClass = "time"


            td.Controls.Add(label)
            td1.Controls.Add(label1)
            td2.Controls.Add(label2)
            td2.Align = "right"

            tr.Attributes.Add("onclick", "getConv(this)")
            tr.ID = v.id

            tr.Cells.Add(td)
            tr.Cells.Add(td1)
            tr.Cells.Add(td2)

            ConvList.Rows.Add(tr)

        Next


    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getAll(ByVal dateNow As String, ByVal convID As String) As List(Of Message)

        Dim msgList As New List(Of Message)

        Dim sql As String = "select * from website__Messages "

        sql += "where ConversationID = '" & convID & " '"

        sql += "and date > '" & dateNow & "' order by date ASC "

        Dim DB As New DB()

        db.Connection_On()

        db.Execute_Sql(sql, "Reader")

        While db.SdrData.Read()
            Dim msg As New Message()
            msg.id = db.SdrData(0).ToString()
            msg.msg = db.SdrData("Message")
            msg.time = db.SdrData("date") & "." & db.SdrData("date").Millisecond
            msg.dist = db.SdrData("Distination")
            msg.source = db.SdrData("Source")
            msgList.Add(msg)

        End While

        db.Connection_Off()


        Return msgList
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function getMesages(ByVal id As String) As List(Of Message)



        Dim msgList As New List(Of Message)

        Dim msg As New Message()

        msgList = msg.getMsg(id)

        Return msgList

    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Sub sendMesage(ByVal id As String, ByVal msg As String)

        Dim conv As New conversation()

        conv = conv.getConvById(id)

        Dim message As New Message()

        message.msg = msg

        message.source = "admin"

        message.dist = conv.client

        message.convID = conv.id

        message.send(message.msg, message.convID, message.dist, message.source)



    End Sub



    Public Class Message

        Public id As String = ""
        Public msg As String = ""
        Public convID As String = ""
        Public time As String = ""
        Public dist As String = ""
        Public source As String = ""

        Public Sub send(ByVal msg As String, ByVal conv As String, ByVal dist As String, ByVal Source As String)

            Dim DB As New DB()

            Dim sql As String = "insert into website__Messages(Message,ConversationID,Distination,Source)VALUES('" & msg & "','" & conv & "','" & dist & "','" & Source & "' )"

            db.Connection_On()

            db.Execute_Sql(sql, "Scalar")

            db.Connection_Off()

        End Sub

        Public Function getMsg(ByVal conv As String) As List(Of Message)

            Dim list As New List(Of Message)


            Dim DB As New DB()

            Dim sql As String = ""

            sql = "select * from website__Messages where ConversationID = '" & conv & "' order by date"

            db.Connection_On()

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                Dim msg As New Message()

                msg.id = db.SdrData(0).ToString()
                msg.msg = db.SdrData(1)
                msg.time = db.SdrData("date") & "." & db.SdrData("date").Millisecond
                msg.dist = db.SdrData(4)
                msg.source = db.SdrData(5)

                list.Add(msg)

            End While

            db.Connection_Off()

            Return list

        End Function

    End Class


    Public Class conversation
        Public id As String = ""
        Public admin As String = ""
        Public client As String = ""
        Public is_Read As Boolean = False

        Public Function isExist(ByVal user As String) As Boolean
            Dim exist As Boolean = False

            Dim DB As New DB()

            db.Connection_On()

            Dim sql As String = "select top 1 Client from Conversations where Client= '" & user & "'"

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                If db.SdrData(0) <> Nothing Then
                    exist = True
                End If


            End While

            db.Connection_Off()


            Return exist

        End Function
        Public Sub Create(ByVal admin As String, ByVal client As String)

            Dim DB As New DB()

            db.Connection_On()

            Dim sql As String = "insert into Conversations(Admin,Client,is_Read)VALUES('" & admin & "','" & client & "',0)"

            db.Execute_Sql(sql, "Scalar")

            db.Connection_Off()

        End Sub

        Public Function getConv(ByVal user As String) As conversation
            Dim conv As New conversation

            Dim DB As New DB()

            Dim sql As String = ""

            sql = "select Top 1 * from Conversations where Client = '" & user & "'"

            db.Connection_On()

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                conv.id = db.SdrData(0).ToString()
                conv.admin = db.SdrData(1).ToString()
                conv.client = db.SdrData(2).ToString()
                conv.is_Read = db.SdrData(3)

            End While

            db.Connection_Off()

            Return conv

        End Function
        Public Function getAll() As List(Of conversation)

            Dim list As New List(Of conversation)


            Dim DB As New DB()


            Dim sql As String = ""

            db.Connection_On()

            sql = "select * from Conversations"



            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                Dim conv As New conversation()

                conv.id = db.SdrData(0).ToString()
                conv.admin = db.SdrData(1).ToString()
                conv.client = db.SdrData(2).ToString()
                conv.is_Read = db.SdrData(3)
                list.Add(conv)

            End While

            db.Connection_Off()

            Return list
        End Function

        Public Function getLastMsg(ByVal conv As String) As Message

            Dim msg As New Message()

            Dim DB As New DB()

            Dim sql As String = "select top 1 * from website__Messages "

            sql += "where ConversationID = '" & conv & "' ORDER BY Date DESC"

            db.Connection_On()

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()


                msg.msg = db.SdrData("Message").ToString()
                msg.time = db.SdrData("Date")

            End While

            db.Connection_Off()

            Return msg

        End Function

        Public Function getConvById(ByVal id As String) As conversation

            Dim conv As New conversation()

            Dim DB As New DB()

            db.Connection_On()

            db.Execute_Sql("select * from Conversations where ID = '" & id & "'", "Reader")
            While db.SdrData.Read()
                conv.id = db.SdrData(0).ToString()
                conv.admin = db.SdrData(1)
                conv.client = db.SdrData(2)
                conv.is_Read = db.SdrData(3)

            End While

            db.Connection_Off()

            Return conv

        End Function

    End Class




End Class
