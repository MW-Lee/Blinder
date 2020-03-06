using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using System.IO;
using System.Text;

//public class PlayerController : MonoBehaviour
//{
//    // 이동, 회전 관련 변수.
//    private float horizontal = 0.0f;

//    private float vertical = 0.0f;

//    private float rotate = 0.0f;

//    public float moveSpeed = 0.0f;

//    public float rotSpeed = 0.0f;

//    public Vector3 vDir = new Vector3();

//    private new Transform transform = null;

//    // 시간 관련 변수.
//    private float oldTime = 0.2f;
//    private float curTime = 0.0f;

//    // 서버 관련 변수.
//    JsonMgr jsonMgr = new JsonMgr();

//    State state = State.Idle;

//    protected string dataPath;

//    private void Start()
//    {
//        transform = GetComponent<Transform>();

//        vDir = Vector3.zero;    

//        dataPath = Application.dataPath + "/Resources/Json";
//    }

//    private void Update()
//    {
//        curTime += Time.deltaTime;

//        horizontal = Input.GetAxis("Horizontal");
//        vertical = Input.GetAxis("Vertical");
//        rotate = Input.GetAxis("Mouse X");

//        Vector3 moveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal);

//        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);

//        transform.Rotate(Vector3.up * rotate * rotSpeed * Time.deltaTime);

//        state = horizontal != 0 || vertical != 0 ? State.Walk : State.Idle;

//        vDir = new Vector3(horizontal, 0, vertical);

//        if (curTime >= oldTime)
//        {
//            curTime -= oldTime;
//            //JsonOverWirte();
//        }
//    }


//}

public class PlayerController : MonoBehaviour
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

    //
    // 시간 관련 변수
    //
    private float fOldTime;
    private float fCurTime;

    //
    // 서버 관련 변수
    //
    private JsonMgr jsonMgr;
    private string sDataPath;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    private void JsonOverWirte()
    {
        JsonClass json = new JsonClass(this.gameObject.transform, sState, vDir, fRotate);
        string jsonData = jsonMgr.ObjectToJson(json);
        jsonMgr.CreateJsonFile(sDataPath, "DisPlayer", jsonData);

        //byte[] data = Encoding.UTF8.GetBytes(jsonData);

        //TransportTCP.instance.Send(data, data.Length);
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

        jsonMgr = new JsonMgr();
        sState = State.Idle;
    }

    private void Update()
    {
        fCurTime += Time.deltaTime;

        fHorizontal = Input.GetAxis("Horizontal");
        fVertical = Input.GetAxis("Vertical");
        fRotate = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * fVertical) + (Vector3.right * fHorizontal);

        transform.Translate(moveDir.normalized * fMoveSpeed * Time.deltaTime, Space.Self);

        transform.Rotate(Vector3.up * fRotate * fRotSpeed * Time.deltaTime);

        sState = fHorizontal != 0 || fVertical != 0 ? State.Walk : State.Idle;

        vDir = new Vector3(fHorizontal, 0, fVertical);

        if (fCurTime >= fOldTime)
        {
            fCurTime -= fOldTime;
            JsonOverWirte();
        }
    }

    #endregion
}
