using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

public class StateObject
{
    public Socket workSocket = null;

    public const int BufferSize = 512;

    public byte[] buffer = new byte[BufferSize];

    public StringBuilder sb = new StringBuilder();
}

public class AsyncClient : MonoBehaviour
{
    public Socket socket;

    string ipAdress = "127.0.0.1";

    private int port = 31400;

    private IPEndPoint ipep;

    public ManualResetEvent connectDone;
    public ManualResetEvent sendDone;
    public ManualResetEvent receiveDone;

    string response;

    private JsonMgr jsonmgr = new JsonMgr();

    byte[] sendBuffer = new byte[512];

    private void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ipAddr = IPAddress.Parse(ipAdress);

        ipep = new IPEndPoint(ipAddr, port);

        Connect(ipep, socket);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Send(socket, sendBuffer);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Receive(socket);
        }
    }

    public void Connect(IPEndPoint ipEP, Socket client)
    {
        client.BeginConnect(ipEP, new AsyncCallback(ConnectCallback), client);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            client.EndConnect(ar);

            connectDone.Set();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void Send(Socket client, byte[] datas)
    {
        JsonClass json = new JsonClass(this.gameObject.transform, State.Idle, Vector3.zero);
        string jsonData = jsonmgr.ObjectToJson(json);

        // 해더를 추가한 만큼 데이터를 보내야 하기때문에 아래쪽에서 연산한 temp.nSize 만큼 크기를 보내주기로 변경.
        //int i = Encoding.Default.GetByteCount(jsonData);

        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        byte[] Buffer = new byte[8 + data.Length];

        PACKET_HEADER temp;
        temp.nID = 6;
        temp.nSize = (short)Buffer.Length;

        byte[] Header = StructToByte(temp);

        Array.Copy(Header, 0, Buffer, 0, Header.Length);
        Array.Copy(data, 0, Buffer, Header.Length, data.Length);;

        client.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            int bytesSend = client.EndSend(ar);

            Console.WriteLine("Send {0} bytes to server.", bytesSend);

            sendDone.Set();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void Receive(Socket client)
    {
        try
        {
            StateObject state = new StateObject();

            state.workSocket = client;

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;

            Socket client = state.workSocket;

            int bytesRead = client.EndReceive(ar);

            if(bytesRead >0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                if(state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }

                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private byte[] StructToByte(object _obj)
    {
        int size = Marshal.SizeOf(_obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(_obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    private T ByteToStruct<T>(byte[] _inputHeader) where T : struct
    {
        int size = 4;
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(_inputHeader, 0, ptr, size);
        T oResult = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);

        return oResult;
    }

    private void ProcessPacket(int nSessionID, string[] pData)
    {

        switch (nSessionID)
        {
            case Constant.RES_IN:

                break;

            case Constant.NOTICE_CHAT:

                break;
        }
    }
}
