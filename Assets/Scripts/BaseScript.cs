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
using UnityEngine;

public static class Constant
{
    public const short REQ_IN = 1;

    public const short RES_IN = 2;

    public const short REQ_CHAT = 6;

    public const short NOTICE_CHAT = 7;
}

public struct PACKET_HEADER
{
    // c++ 에서 short = 2byte c# 에서는 4byte 이므로 int 형으로 변경
    public int nID;
    public int nSize;
}

public struct PKT_NOTICE_CHAT
{

}
