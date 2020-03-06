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

public class DisPlayerController : CharacterClass
{
    #region 변수

    //
    // 기본 이동 관련 변수
    //
    public Vector3 vDir;
    public float fMoveSpeed;
    public float fRotSpeed;

    private Transform TF;
    private State sState;
    private float fHorizontal;
    private float fVertical;
    private float fRotate;

    private Vector3 vDestination;
    private Quaternion vRot;
    private float fRot;

    //
    // 시간 관련 변수
    //
    private float fOldTime;
    private float fCurTime;

    //
    // 서버 관련 변수
    //
    private JsonClass jcReceiveData;
    private string sDataPath;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    //private void JsonOverWirte()
    //{
    //    JsonClass json = new JsonClass(this.gameObject.transform, state, vDir);
    //    string jsonData = jsonMgr.ObjectToJson(json);
    //    //jsonMgr.CreateJsonFile(dataPath, "DisPlayer", jsonData);

    //    byte[] data = Encoding.UTF8.GetBytes(jsonData);

    //    TransportTCP.instance.Send(data, data.Length);
    //}

    private void JsonRead()
    {
        jcReceiveData = new JsonMgr().LoadJsonFile<JsonClass>(sDataPath, "DisPlayer");

        if (TF.position != vDestination)
            TF.position = vDestination;

        //if (TF.rotation != vRot)
        //    TF.rotation = vRot;

        vDestination = jcReceiveData.vPos;
        vRot = jcReceiveData.vRot;
        TF.localScale = jcReceiveData.vScale;
        sState = jcReceiveData.state;
        vDir = jcReceiveData.vDir;
        fRot = jcReceiveData.fRot;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        TF = GetComponent<Transform>();
        vDir = Vector3.zero;
        fHorizontal = .0f;
        fVertical = .0f;
        fRotate = .0f;
        fMoveSpeed = 1.0f;
        fRotSpeed = 30.0f;

        sDataPath = Application.dataPath + "/Resources/Json";

        fOldTime = .2f;
        fCurTime = .0f;

        jcReceiveData = new JsonClass();
        sState = State.Idle;
    }

    private void Update()
    {
        fCurTime += Time.deltaTime;
        if (fCurTime >= fOldTime)
        {
            fCurTime -= fOldTime;
            JsonRead();
        }

        if (sState != State.Idle)
        {
            TF.Translate(vDir.normalized * fMoveSpeed * Time.deltaTime);

            //TF.Rotate(TF.localRotation * Vector3.up * fRotSpeed * Time.deltaTime);
            //TF.Rotate(Vector3.up * fRot * fRotSpeed * Time.deltaTime);

            TF.rotation = Quaternion.Slerp(TF.rotation, vRot, .2f);
        }





        //fHorizontal = Input.GetAxis("Horizontal");
        //fVertical = Input.GetAxis("Vertical");
        //fRotate = Input.GetAxis("Mouse X");

        //Vector3 moveDir = (Vector3.forward * fVertical) + (Vector3.right * fHorizontal);

        //transform.Translate(moveDir.normalized * fMoveSpeed * Time.deltaTime);

        //transform.Rotate(Vector3.up * fRotate * fRotSpeed * Time.deltaTime);

        //sState = fHorizontal != 0 || fVertical != 0 ? State.Walk : State.Idle;

        //vDir = new Vector3(fHorizontal, 0, fVertical);
    }

    #endregion
}
