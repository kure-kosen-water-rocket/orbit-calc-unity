using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftpowerscript : MonoBehaviour
{
    Rigidbody rigid;

    Vector3 v;
    Vector3 LiftDirection;

    public float LiftPower;//揚力係数
    public float petVerosity;//ロケットの速度
    public float airDensity = 1.225f;//空気密度
    const float area = 12.0f;//接触面積

    float m;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        LiftPower = airDensity / 2 * area * petVerosity;
        v = rigid.velocity * 0.1f;
        m = v.magnitude;
        petVerosity = m * m;
        LiftDirection = new Vector3(0.5f, 1.5f, 0) * LiftPower;
        rigid.AddForce(LiftDirection, ForceMode.Force);
    }
}