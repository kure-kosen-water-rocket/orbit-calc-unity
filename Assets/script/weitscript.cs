using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weitscript : MonoBehaviour
{
    public  float weit=1.5f;//重さ
    Rigidbody rigi;//Rigidbodyの宣言
    Vector3 force;//力
    // Start is called before the first frame update
    void Start()
    {
        rigi = gameObject.GetComponent<Rigidbody>();//Rigidbodyの取得
       force= new Vector3(0, -9.8f*weit, 0);//重力を決定
    }

    // Update is called once per frame
    void Update()
    {
        rigi.AddForce(force * weit, ForceMode.Force);//重力をかける
            }
}
