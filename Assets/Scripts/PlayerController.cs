using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using System.IO;
using System.Text;

public class PlayerController : MonoBehaviour
{
    // 이동, 회전 관련 변수.
    private float horizontal = 0.0f;

    private float vertical = 0.0f;

    private float rotate = 0.0f;

    public float moveSpeed = 0.0f;

    public float rotSpeed = 0.0f;

    public Vector3 vDir = new Vector3();

    private new Transform transform = null;

    // 시간 관련 변수.
    private float oldTime = 0.2f;
    private float curTime = 0.0f;

    // 서버 관련 변수.
    JsonMgr jsonMgr = new JsonMgr();

    State state = State.Idle;

    protected string dataPath;

    private void Start()
    {
        transform = GetComponent<Transform>();

        vDir = Vector3.zero;    

        dataPath = Application.dataPath + "/Resources/Json";
    }

    private void Update()
    {
        curTime += Time.deltaTime;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        transform.Rotate(Vector3.up * rotate * rotSpeed * Time.deltaTime);

        state = horizontal != 0 || vertical != 0 ? State.Walk : State.Idle;

        vDir = new Vector3(horizontal, 0, vertical);

        if (curTime >= oldTime)
        {
            curTime -= oldTime;
            //JsonOverWirte();
            //print(dataPath);
        }
    }

    //private void JsonOverWirte()
    //{
    //    JsonClass json = new JsonClass(this.gameObject.transform, state, vDir);
    //    string jsonData = jsonMgr.ObjectToJson(json);
    //    //jsonMgr.CreateJsonFile(dataPath, "DisPlayer", jsonData);

    //    byte[] data = Encoding.UTF8.GetBytes(jsonData);

    //    TransportTCP.instance.Send(data, data.Length);
    //}
}
