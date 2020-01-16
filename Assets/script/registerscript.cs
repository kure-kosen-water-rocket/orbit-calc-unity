using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class registerscript : MonoBehaviour
{
    const float registerRatio = 0.34f;//抵抗係数
    public float wingAngle;//羽の角度
    public float liftPower;//揚力
    public float altitude;//ロケットの高度
    public float registerForceMugnitude;//抵抗力の大きさ
    float speed;//ロケットの速度
    bool registerSwith;//揚力の切り替え
    Vector3 registerForce;//抵抗力
    Forcescript fs;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        fs = GetComponent<Forcescript>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        liftPower = 1 + wingAngle * 0.5f;
        speed = rb.velocity.magnitude;
        registerForceMugnitude = speed * registerRatio+liftPower;

    }
    void Update()
    {
        Vector3 liftForce = new Vector3(0.0f, fs.lift*0.1f, 0.0f);
        h = this.transform.position.y;
        if (h > 27)
        {
            registerSwith = true;
        }
        if (registerSwith ==false) {
            registerForce = new Vector3(-registerForceMugnitude*0.5f, 0.0f, 0.0f);
        }
        if(registerSwith)   {
            registerForce = new Vector3(registerForceMugnitude,0.0f, 0.0f);
        }
        if (h>3.5f) {
            rb.AddForce(liftForce, ForceMode.Force);
            rb.AddForce(registerForce, ForceMode.Force);
        }
    }
}
