using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class registerscript : MonoBehaviour
{
    const float registerRatio = 0.34f;
    public float wingAngle;
    public float liftPower;
    public float h;
    public float registerForceMugnitude;
    float speed;
    bool registerswith;
    Vector3 registerForce;
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
            registerswith = true;
        }
        if (registerswith ==false) {
            registerForce = new Vector3(-registerForceMugnitude*0.5f, 0.0f, 0.0f);
        }
        if(registerswith ==true)   {
            registerForce = new Vector3(registerForceMugnitude,0.0f, 0.0f);
        }
        if (h>3.5f) {
            rb.AddForce(liftForce, ForceMode.Force);
            rb.AddForce(registerForce, ForceMode.Force);
        }
    }
}
