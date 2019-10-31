using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    Rigidbody rb;//Rigidbodyの宣言

    Vector3 forcedirection = new Vector3(1.0f, 1.2f, 0f);//射出角度
    Vector3 windforcedirection = new Vector3(1.0f, 1.2f, 0);//風力の向き

    Vector3 force, windforce;//ロケットに与える力、風力

    public Vector3 startposition;
    public Vector3 endposition;

    public float injectionForce;//力の値 

    public float nozzleArea;//ペットボトルの口の面積
    public float airPressure;//空気圧
    public float outPressure;//大気圧
    public float water;//水量
    public float flyingDistance;//飛んだ距離
    public float wingArea;//翼の面積
    public float flySpeed;//速さ

    const float gravity = 9.8f;//重力
    const float miri = 0.001f;//ミリ単位
    const float airRatio = 2 / 7;//空気の比率
    const float pressurRatio = 0.928f;
    const float airDensity = 1.293f;//空気密度
    float waterPressur;//水圧
    float pre;//気圧/大気圧の値
    float allpressur;//気圧の合計

    GameObject target;     //    最高点

    // Start is called before the first frame update
    void Start()
    {
        pre = allpressur / outPressure;
        waterPressur = water * miri * gravity;//水量から水圧を計算
        target = gameObject;            //ターゲットをこのオブジェクトに
        rb = gameObject.GetComponent<Rigidbody>();//このオブジェクトのRigidbodyを取得
        startposition = transform.position;           //初期位置をこのオブジェクトの位置に                      
        allpressur = airPressure + waterPressur;   //気圧の合計を算出

    }
    // Update is called once per frame
    void Update()
    {

        double num = Mathf.Pow(pre, airRatio);
        injectionForce = 7 * nozzleArea * allpressur * ((float)num - 1) + nozzleArea * (allpressur * pressurRatio - outPressure);//力の値を計算
        windforce = injectionForce * windforcedirection;//風力を決定
        force = injectionForce * forcedirection;  //発射の力を設定

        //rb.AddForce(-airresistance * rb.velocity); //空気抵抗を加える

        if (Input.GetKeyUp(KeyCode.Return))//enterキーを押した時
        {
            Inp();
        }

        if (startposition.x < transform.position.x)//高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        {
            endposition = transform.position;//到達地点を設定
            flyingDistance = Vector3.Distance(endposition, startposition);

        }
    }
    void Inp()
    {
        rb.AddForce(windforce, ForceMode.Impulse);//風力を加える
        rb.AddForce(force, ForceMode.Impulse);//ロケットを発射
    }
}