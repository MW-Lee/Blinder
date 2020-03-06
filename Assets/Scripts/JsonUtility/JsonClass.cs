using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Walk,
    Jump,
    Intr,
}

public class JsonClass
{
    // Object Transform
    public Vector3 vPos;
    public Quaternion vRot;
    public Vector3 vScale;
    public State state;
    public float fRot;
    public Vector3 vDir;

    public JsonClass() { }

    public JsonClass(Transform transform, State _state, Vector3 _vDir, float _fRot)
    {
        vPos = transform.position;
        vRot = transform.rotation;
        vScale = transform.localScale;

        state = _state;

        vDir = _vDir;

        fRot = _fRot;
    }
}
