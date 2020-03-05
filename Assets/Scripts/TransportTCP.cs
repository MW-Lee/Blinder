using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

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
        m_socket.Send(data, i, 0);
    }

    // json 받기 테스트
    private void handle_receive()
    {
        try
        {
            byte[] bytes = new byte[512];

            int bytesRec = m_socket.Receive(bytes);

            Debug.Log(bytesRec);

            if(bytesRec <= 0 || bytesRec > bytes.Length)
            {
                return;
            }
            else
            {
                string msg = Encoding.Default.GetString(bytes);
                string[] DummyMsg = msg.Split('\0');

                jsonmgr.CreateJsonFile(datapath, "Displayer", DummyMsg[0]);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
