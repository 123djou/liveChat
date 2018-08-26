Imports System.Data.SqlClient
Imports System.Data

Partial Class ws_www_p_livechat_admin
    Inherits System.Web.UI.Page

    Class info
        Public conv As New conversation()
        Public txt As String = ""
        Public time As String = ""
    End Class

    <System.Web.Services.WebMethod()> _
    Public Shared Function InitConv(ByVal count As Integer) As List(Of info)
        Dim convList As New List(Of info)

        Dim conv As New conversation()
        Dim msg As New Message()

        Dim c As Integer = conv.Count() - count

        For Each x In conv.getAll(c)

            Dim inf As New info()
            inf.conv.admin = x.admin
            inf.conv.client = x.client
            inf.conv.id = x.id

            inf.txt = x.getLastMsg(x.id).msg
            inf.time = x.getLastMsg(x.id).time
            convList.Add(inf)


        Next



        Return convList


    End Function



    Public Shared Function getCount(ByVal id As String) As Integer
        Dim i As Integer
        Dim db As New DB()
        Dim sql As String = "select count(*) from website__Messages where ConversationId = '" + id + "'"
        db.Connection_On()
        db.Execute_Sql(sql, "Reader")
        While db.SdrData.Read()
            i = db.SdrData(0)
        End While
        db.Connection_Off()
        Return i
    End Function



    <System.Web.Services.WebMethod()> _
    Public Shared Function getMesages(ByVal id As String, ByVal count As Integer) As List(Of Message)



        Dim msgList As New List(Of Message)

        Dim msg As New Message()

        msgList = msg.getMsg(id, count)

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

            Dim db As New DB()

            Dim sql As String = "insert into website__Messages(Message,ConversationID,Distination,Source)VALUES('" & msg & "','" & conv & "','" & dist & "','" & Source & "' )"

            db.Connection_On()

            db.Execute_Sql(sql, "Scalar")



        End Sub

        Public Function getMsg(ByVal conv As String, c As Integer) As List(Of Message)

            Dim list As New List(Of Message)

            Dim i = getCount(conv) - c

            Dim db As New DB()

            Dim sql As String = ""
            Dim ord As String = ""
            If (c <> 0) Then
                ord = "desc"
            End If


            sql = "select top " & i & " * from website__Messages where ConversationID = '" & conv & "' order by date  " & ord


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

            Return list

        End Function

    End Class


    Public Class conversation
        Public id As String = ""
        Public admin As String = ""
        Public client As String = ""
        Public is_Read As Boolean = False

    


        Public Function Count() As Integer

            Dim i = 0

            Dim sql As String = "select count(*) from Conversations"

            Dim db As New DB()

            db.Connection_On()

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                i = db.SdrData(0)

            End While

            db.Connection_Off()
            Return i


        End Function


        Public Function getConv(ByVal user As String) As conversation
            Dim conv As New conversation

            Dim db As New DB()

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


            Return conv

        End Function
        Public Function getAll(ByVal c As Integer) As List(Of conversation)

            Dim list As New List(Of conversation)


            Dim db As New DB()


            Dim sql As String = ""

            db.Connection_On()

            Dim ord As String = ""

            If (c > 0) Then
                ord = "DESC"
            End If

            sql = "select top " & c & " * from Conversations order by ID " & ord



            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()

                Dim conv As New conversation()

                conv.id = db.SdrData(0).ToString()
                conv.admin = db.SdrData(1).ToString()
                conv.client = db.SdrData(2).ToString()
                conv.is_Read = db.SdrData(3)
                list.Add(conv)

            End While


            Return list
        End Function

        Public Function getLastMsg(ByVal conv As String) As Message

            Dim msg As New Message()

            Dim db As New DB()

            Dim sql As String = "select top 1 * from website__Messages "

            sql += "where ConversationID = '" & conv & "' ORDER BY Date DESC"

            db.Connection_On()

            db.Execute_Sql(sql, "Reader")

            While db.SdrData.Read()


                msg.msg = db.SdrData("Message").ToString()
                msg.time = db.SdrData("Date")

            End While



            Return msg

        End Function

        Public Function getConvById(ByVal id As String) As conversation

            Dim conv As New conversation()

            Dim db As New DB()

            db.Connection_On()

            db.Execute_Sql("select * from Conversations where ID = '" & id & "'", "Reader")
            While db.SdrData.Read()
                conv.id = db.SdrData(0).ToString()
                conv.admin = db.SdrData(1)
                conv.client = db.SdrData(2)
                conv.is_Read = db.SdrData(3)

            End While


            Return conv

        End Function

    End Class


    Public Class DB1

        Public Property ConnectionString() As String
            Get
                Return _ConnectionString
            End Get
            Set(ByVal ConnectionString As String)
                _ConnectionString = ConnectionString
            End Set
        End Property
        Private _ConnectionString As String

        Dim Sql As String
        Dim Sqlconn As New SqlConnection
        Dim SqlCmd As New SqlCommand
        Public SdrData As SqlDataReader


        Public Sub New()


            _ConnectionString = "Data Source=WIN-Q77N0RVDTMS;Initial Catalog=DB__sp1__002----jn____d9-F0@;"
            _ConnectionString += "User ID=w@__jn__user1; Password=d@Vze5f5.ze_wx6@kj;"

            _ConnectionString += "pooling=false;connect timeout=900;"


        End Sub


        Sub Connection_On()

            Sqlconn.ConnectionString = _ConnectionString

            If Sqlconn.State = ConnectionState.Closed Then
                Sqlconn.Open()
            End If
            SqlCmd.CommandTimeout = 0
            SqlCmd.Connection = Sqlconn

        End Sub

        Sub Connection_Off()



            If Sqlconn.State = ConnectionState.Open Then
                Sqlconn.Close()
                Sqlconn.Dispose()
            End If

            SqlCmd.Dispose()

            Sql = Nothing

        End Sub


        Sub Execute_Sql(ByVal Sql As String, ByVal Reader_or_Scalar As String)

            SqlCmd.CommandText = Sql

            SqlCmd.CommandType = CommandType.Text

            If Reader_or_Scalar = "Reader" Then

                SdrData = SqlCmd.ExecuteReader()

            ElseIf Reader_or_Scalar = "Scalar" Then

                SdrData = SqlCmd.ExecuteScalar()

            End If

        End Sub

        Protected Overrides Sub Finalize()


            Sql = Nothing
            Sqlconn = Nothing
            SqlCmd = Nothing
            SdrData = Nothing

            MyBase.Finalize()

        End Sub

    End Class


End Class
