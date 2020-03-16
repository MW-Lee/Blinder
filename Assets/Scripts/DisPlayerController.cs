////////////////////////////////////////////////
//
// DisPlayerController
//
// 상대쪽 캐릭터를 움직이기 위한 스크립트
// 
// 20. 03. 06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPlayerController : MonoBehaviour
{
    #region 변수

    //
    // 기본 이동 관련 변수
    //
    /// <summary>
    /// (받은 데이터) 움직이기 위한 방향벡터
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
    /// (받은 데이터) 캐릭터의 현재 상태
    /// </summary>
    private State sState;
    /// <summary>
    /// (임시 데이터) 0.2초마다 움직일 목적지
    /// </summary>
    private Vector3 vDestination;
    /// <summary>
    /// (받은 데이터) 회전하기 위한 Quaternion
    /// </summary>
    private Quaternion vRot;

    //
    // 시간 관련 변수
    //
    /// <summary>
    /// 0.2초 고정
    /// </summary>
    private float fOldTime;
    /// <summary>
    /// DeltaTime을 더하여 0.2초마다 자르기 위해 시간을 담을 변수
    /// </summary>
    private float fCurTime;

    //
    // 서버 관련 변수
    //
    /// <summary>
    /// 상대쪽에서 받은 데이터를 저장하는 JsonClass
    /// </summary>
    private JsonClass jcReceiveData;
    /// <summary>
    /// 로드할 파일 위치
    /// </summary>
    private string sDataPath;

    public static bool bIsOnline;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 상대 쪽에서 보낸 데이터를 받아서 적용시키기 위한 함수
    /// </summary>
    private void JsonRead()
    {
        // 만들어진 Json파일을 JsonClass형식으로 로드
        //jcReceiveData = new JsonMgr().LoadJsonFile<JsonClass>(sDataPath, "DisPlayer");

        string temp = new JsonMgr().LoadJsonFile<string>(sDataPath, "DisPlayer");
        JsonClass _json = new JsonMgr().JsonToObject<JsonClass>(temp);

        // 받은 데이터로 캐릭터를 움직이기
        vDestination = _json.vPos;
        vRot = _json.vRot;
        TF.localScale = _json.vScale;
        sState = _json.state;
        vDir = _json.vDir;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        // 초기 세팅
        TF = GetComponent<Transform>();
        vDir = Vector3.zero;
        fMoveSpeed = 1.0f;
        fRotSpeed = 30.0f;

        sDataPath = Application.dataPath + "/Resources/Json";

        fOldTime = .2f;
        fCurTime = .0f;

        jcReceiveData = new JsonClass();
        sState = State.Idle;

        bIsOnline = false;
    }

    private void Update()
    {
        if (bIsOnline)
        {
            // 0.2초 마다 갱신하기
            fCurTime += Time.deltaTime;
            if (fCurTime >= fOldTime)
            {
                fCurTime -= fOldTime;
                JsonRead();
            }

            // 현재 상태가 가만히 있지 않을 때만 작동
            if (sState != State.Idle)
            {
                TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);

                TF.rotation = Quaternion.Slerp(TF.rotation, vRot, .2f);
            }
            // 현재 상태가 가만히 있는데 위치가 받은 위치와 다른 경우 보간이동
            else if (TF.position != vDestination && sState == State.Idle)
            {
                TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);
                TF.position = new Vector3(
                    Mathf.Lerp(TF.position.x, vDestination.x, 0.2f),
                    Mathf.Lerp(TF.position.y, vDestination.y, 0.2f),
                    Mathf.Lerp(TF.position.z, vDestination.z, 0.2f));
            }
        }
    }

    #endregion
}
