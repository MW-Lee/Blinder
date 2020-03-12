////////////////////////////////////////////////
//
// 
//
// 상대쪽 캐릭터를 움직이기 위한 스크립트
// 
// 20. 03. 11
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Constant
{
    public const short REQ_IN = 1;

    public const short RES_IN = 2;

    public const short REQ_CHAT = 6;

    public const short NOTICE_CHAT = 7;

    //
    //
    //
    public const int MAX_NAME_LEN = 17;

    public const int MAX_MESSAGE_LEN = 512;

    public const int MAX_RECEIVE_BUFFER_LEN = 512;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PACKET_HEADER
{
    // c++ 에서 short = 2byte c# 에서는 4byte 이므로 int 형으로 변경
    public int nID;
    public int nSize;

    //public PACKET_HEADER()
    //{
    //    nID = 0;
    //    nSize = 0;
    //}

    public PACKET_HEADER(int id, int size)
    {
        nID = id;
        nSize = size;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct PKT_NOTICE_CHAT
{
    [MarshalAs(UnmanagedType.ByValArray,SizeConst = Constant.MAX_NAME_LEN)]
    public char[] szName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.MAX_MESSAGE_LEN)]
    public char[] szMessage;

    //string szName;
    //string szMessage;

    //public PKT_NOTICE_CHAT()
    //{
    //    //szName = new char[Constant.MAX_NAME_LEN];
    //    //szMessage = new char[Constant.MAX_MESSAGE_LEN];

    //    //szName = string.Empty;
    //    //szMessage = string.Empty;
    //}
}
