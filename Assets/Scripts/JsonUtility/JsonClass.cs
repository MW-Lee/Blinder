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
    public Vector3 vDir;
    public State state;

    public JsonClass() { }

    public JsonClass(Transform transform, State _state, Vector3 _vDir)
    {
        vPos = transform.position;

        vRot = transform.rotation;
        vScale = transform.localScale;

        vDir = _vDir;

        state = _state;
    }
}
