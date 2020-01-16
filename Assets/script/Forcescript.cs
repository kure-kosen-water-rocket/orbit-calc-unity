using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    Rigidbody rb;//Rigidbodyの宣言

    Vector3 forceDirection = new Vector3(2.0f, 1.0f, 0f);//射出角度
    Vector3 windForceDirection = new Vector3(1.0f, 1.2f, 0);//風力の向き

    Vector3 force;//ロケットに与える力
    Vector3 windForce;//風力

    public Vector3 startPosition;//発射点
    public Vector3 endPosition;//着地点
    Vector3 gravityMugnituid;//重力
  
    public float injectionForce;//力の値 
    public float mass;//質量
    public float nozzlEarea;//ペットボトルの口の面積
    public float airPressure;//空気圧
    public float outPressure;//大気圧
    public float water;//水量
    public float flyingDistance;//飛んだ距離
    const float liftCoefficient=0.5f;//揚力係数
    public float flySpeed;//速さ
    const float gravity=9.8f;//重力
    const float airRatio=2/7;//空気の比率
    const float pressurRatio=0.928f;
    const float airDensity = 0.5f;//空気密度
    float waterPressure;//水圧
    float pre; //気圧/大気圧の値
    float allPressur;//気圧の合計
    public float lift;//揚力

    GameObject target;     //    最高点

    // Start is called before the first frame update
    void Start()
    {
        pre = allPressur / outPressure;
        waterPressure = water * gravity;//水量から水圧を計算
        target = gameObject;            //ターゲットをこのオブジェクトに
        rb = gameObject.GetComponent<Rigidbody>();//このオブジェクトのRigidbodyを取得
        startPosition = transform.position;           //初期位置をこのオブジェクトの位置に                      
        allPressur = airPressure + waterPressure;   //気圧の合計を算出
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        flySpeed = rb.velocity.magnitude;
        lift = liftCoefficient * flySpeed * flySpeed * airDensity;//揚力の計算
    }
    void Update() {
        rb.AddForce(gravityMugnituid, ForceMode.Force);
        double num = Mathf.Pow(pre,airRatio);
        injectionForce = 7 * nozzlEarea * allPressur * ((float)num-1)+nozzlEarea*(allPressur*pressurRatio-outPressure);//力の値を計算
        windForce = injectionForce * windforceDirection;//風力を決定
        force = injectionForce * forceDirection;  //発射の力を設定

        //rb.AddForce(-airresistance * rb.velocity); //空気抵抗を加える

        if (Input.GetKeyUp(KeyCode.Return))//enterキーを押した時
        {
            Inp();
        }
  
        if  (startPosition.x< transform.position.x)//高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        {
            endPosition = transform.position;//到達地点を設定
            flyingDistance = Vector3.Distance(endPosition, startPosition);
           
        }

       
    }
    void Inp()
    {
        rb.AddForce(windForce, ForceMode.Impulse);//風力を加える
        rb.AddForce(force, ForceMode.Impulse);//ロケットを発射
    }
}
