using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using System.IO;
using System.Text;

public class PlayerController : MonoBehaviour
{
    #region 변수

    //
    // 기본 이동 관련 변수
    //
    /// <summary>
    /// (보낼 데이터) 움직이기 위한 방향벡터
    /// </summary>
    public Vector3 vDir;
    /// <summary>
    /// 움직이는 속도
    /// </summary>
    public float fMoveSpeed;
    /// <summary>
    /// 회전 속도
    /// </summary>
    public float fRotSpeed;

    /// <summary>
    /// Transform을 미리 파싱해놓음 (최적화)
    /// </summary>
    private Transform TF;
    /// <summary>
    /// (보낼 데이터) 캐릭터의 현재 상태
    /// </summary>
    private State sState;
    /// <summary>
    /// (보낼 데이터) 캐릭터의 X축 이동량
    /// </summary>
    private float fHorizontal;
    /// <summary>
    /// (보낼 데이터) 캐릭터의 Y축 이동량 
    /// </summary>
    private float fVertical;
    /// <summary>
    /// (보낼 데이터) 캐릭터의 Y축 기준 회전량
    /// </summary>
    private float fRotate;

    //
    // 시간 관련 변수
    //
    /// <summary>
    /// 0.2초로 고정
    /// </summary>
    private float fOldTime;
    /// <summary>
    /// DeltaTime을 더하여 0.2초 마다 자르기 위해 시간을 담을 변수
    /// </summary>
    private float fCurTime;

    //
    // 서버 관련 변수
    //
    /// <summary>
    /// 상대쪽으로 데이터를 보내기 위한 JsonManager
    /// </summary>
    private JsonMgr jsonMgr;
    /// <summary>
    /// 상대쪽에 Json파일을 만들 위치
    /// </summary>
    private string sDataPath;

    public static bool bIsOnline;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 상대쪽으로 데이터를 보내는 함수
    /// </summary>
    private void JsonOverWirte()
    {
        // 보낼 내용을 정리
        //JsonClass json = new JsonClass(this.gameObject.transform, sState, vDir);

        object json = new JsonClass(this.gameObject.transform, sState, vDir);

        // 정리된 내용을 Json으로 변경
        string jsonData = jsonMgr.ObjectToJson(json);

        jsonData += '\0';

        // (Test) Json파일 생성
        //jsonMgr.CreateJsonFile(sDataPath, "DisPlayer", jsonData);

        // 변경된 Json을 치환
        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        // 쏘세요!
        AsyncClient.instance.Send(data, 6);
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        // 초기 세팅
        TF = GetComponent<Transform>();
        vDir = Vector3.zero;
        fHorizontal = .0f;
        fVertical = .0f;
        fRotate = .0f;
        fMoveSpeed = 1.0f;
        fRotSpeed = 30.0f;

        //sDataPath = Application.dataPath + "/Resources";
        sDataPath = Application.streamingAssetsPath;

        fOldTime = .2f;
        fCurTime = .0f;

        jsonMgr = new JsonMgr();
        sState = State.Idle;

        bIsOnline = false;
    }

    private void Update()
    {
        if (bIsOnline)
        {
            // 0.2초마다 갱신하기
            fCurTime += Time.deltaTime;

            // 유니티 제공 InputAxis 활용 > 이동량, 회전량 받기
            fHorizontal = Input.GetAxis("Horizontal");
            fVertical = Input.GetAxis("Vertical");
            fRotate = Input.GetAxis("Mouse X");

            // 방향벡터 계산
            Vector3 moveDir = (Vector3.forward * fVertical) + (Vector3.right * fHorizontal);

            // 방향으로 이동
            transform.Translate(moveDir.normalized * fMoveSpeed * Time.deltaTime, Space.Self);

            // 회전
            transform.Rotate(Vector3.up * fRotate * fRotSpeed * Time.deltaTime);

            // 캐릭터가 X || Z 축으로 조금이라도 움직일경우 Idle 상태에서 Walk로 변경
            sState = fHorizontal != 0 || fVertical != 0 ? State.Walk : State.Idle;

            // 보낼 방향벡터 계산
            vDir = new Vector3(fHorizontal, 0, fVertical);

            if (fCurTime >= fOldTime)
            {
                fCurTime -= fOldTime;
                JsonOverWirte();
            }
        }
        #endregion
    }
}