using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Runtime.InteropServices;

public class TransportTCP : MonoBehaviour
{
    private Socket m_socket;

    string ipAdress = "192.168.43.35";

    private int port = 31400;

    byte[] sendBytes;

    JsonMgr jsonmgr = new JsonMgr();

    string datapath = null;

    // Use this for initialization
    private void Start()
    {
        // create socket
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // connect
        try
        {
            IPAddress ipAddr = IPAddress.Parse(ipAdress);

            IPEndPoint ipendPoint = new IPEndPoint(ipAddr, port);

            m_socket.Connect(ipendPoint);
        }
        catch(SocketException se)
        {
            Debug.Log("Socket connect error ! : " + se.ToString());

            return;
        }

        datapath = Application.dataPath + "/Resources";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            try
            {
                //StringBuilder sb = new StringBuilder();

                //sb.Append("Test 1 - send data!!");

                //int i = Encoding.Default.GetByteCount(sb.ToString());

                //byte[] d = Encoding.Default.GetBytes(sb.ToString());

                //m_socket.Send(d, i, 0);

                handle_send();
            }
            catch(Exception e)
            {
                Debug.Log("Socket send or receive error ! : " + e.ToString());
            }
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            m_socket.Disconnect(true);

            m_socket.Close();
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            handle_receive();
        }
    }

    // json 보내기 테스트 성공
    private void handle_send()
    {
        JsonClass json = new JsonClass(this.gameObject.transform, State.Idle, Vector3.zero);
        string jsonData = jsonmgr.ObjectToJson(json);

        int i = Encoding.Default.GetByteCount(jsonData);

        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        byte[] Buffer = new byte[4 + data.Length];

        PACKET_HEADER temp;
        temp.nID = 6;
        temp.nSize = (short)Buffer.Length;

        byte[] Header = StructToByte(temp);

        Array.Copy(Header, 0, Buffer, 0, Header.Length);
        Array.Copy(data, 0, Buffer, Header.Length, data.Length);
        

        m_socket.Send(Buffer, i, 0);
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

    // json 받기 테스트
    private void handle_receive()
    {
        try
        {
            byte[] bytes = new byte[512];

            int bytesRec = m_socket.Receive(bytes);

            //Debug.Log(bytesRec);

            if(bytesRec <= 0 || bytesRec > bytes.Length)
            {
                return;
            }
            else
            {
                PACKET_HEADER temp = new PACKET_HEADER();
                byte[] tempByte = new byte[512];

                Array.Copy(bytes, 0, tempByte, 0, 4);
                temp = ByteToStruct<PACKET_HEADER>(tempByte);
                Array.Clear(tempByte, 0, tempByte.Length);

                Array.Copy(bytes, 20, tempByte, 0, (bytes.Length - 20));
                string msg = Encoding.Default.GetString(tempByte);
                string[] DummyMsg = msg.Split('\0');

                jsonmgr.CreateJsonFile(datapath, "Displayer", DummyMsg[0]);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
        }
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

        switch(nSessionID)
        {
            case Constant.RES_IN:

                break;

            case Constant.NOTICE_CHAT:

                break;
        }
    }
}
