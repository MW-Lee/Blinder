////////////////////////////////////////////////
//
// BaseScript
//
// 정보만 가진 클래스 및 구조체를 모아놓은 스크립트
// 서버 / 클라 공용
// 
// 20. 03. 12
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 전역 공통으로 사용할 변수 모음
/// </summary>
public static class Constant
{
    //
    // Server Packet Header > nID 구분하는 변수
    //
    /// <summary>
    /// Request IN
    /// </summary>
    public const short REQ_IN = 1;
    /// <summary>
    /// Result IN
    /// </summary>
    public const short RES_IN = 2;
    /// <summary>
    /// Request Chat
    /// </summary>
    public const short REQ_CHAT = 6;
    /// <summary>
    /// Result(Notice) Chat
    /// </summary>
    public const short NOTICE_CHAT = 7;

    //
    // Server Packet 사용을 위한 변수
    //
    /// <summary>
    /// Chat > Max Name Length
    /// </summary>
    public const int MAX_NAME_LEN = 17;
    /// <summary>
    /// Chat > Max Message Length
    /// </summary>
    public const int MAX_MESSAGE_LEN = 512;
    /// <summary>
    /// Server > Max Receive Buffer Length
    /// </summary>
    public const int MAX_RECEIVE_BUFFER_LEN = 512;
}

public class ThreadingTimer
{
    public System.Threading.Timer myTimer;

    /// <summary>
    /// 스레드 타이머를 가동시키는 함수
    /// </summary>
    /// <param name="callback">일정 시간마다 호출할 함수</param>
    /// <param name="starttime">언제부터 시작할지 정하는 변수, (기본으로 바로 시작함)</param>
    /// <param name="sendtime">몇 초마다 호출될지 정하는 변수, (기본으로 0.2초)</param>
    public void ThreadTimerStart(System.Threading.TimerCallback callback, int starttime = 0, int sendtime = 200)
    {
        myTimer = new System.Threading.Timer(callback, null, starttime, sendtime);
    }

    /// <summary>
    /// 가동된 스레드 타이머를 정지시키는 함수
    /// </summary>
    public void ThreadTimerStop()
    {
        myTimer.Dispose();
    }
}

/// <summary>
/// 서버에서 주고받을 Packet의 Header 구조체
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PACKET_HEADER
{
    // c++ 에서 short = 2byte c# 에서는 4byte 이므로 int 형으로 변경
    /// <summary>
    /// Packet Header ID
    /// </summary>
    [MarshalAs(UnmanagedType.SysInt)]
    public int nID;
    /// <summary>
    /// Packet Buffer Size
    /// </summary>
    [MarshalAs(UnmanagedType.SysInt)]
    public int nSize;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="id">Send시 보내는 Packet의 ID</param>
    /// <param name="size">Send시 보내는 Packet의 크기</param>
    public PACKET_HEADER(int id, int size)
    {
        nID = id;
        nSize = size;
    }
}

/// <summary>
/// 받은 Packet이 Chat일 때 사용하기 위한 구조체
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct PKT_NOTICE_CHAT
{
    /// <summary>
    /// 채팅을 보낸 플레이어의 닉네임
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray,SizeConst = Constant.MAX_NAME_LEN)]
    public char[] szName;
    /// <summary>
    /// 보낸 채팅의 내용
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.MAX_MESSAGE_LEN)]
    public char[] szMessage;
}
