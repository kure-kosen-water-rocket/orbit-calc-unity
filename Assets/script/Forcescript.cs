using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    Rigidbody rb;//Rigidbodyの宣言

    Vector3 windforce;// 風力

    public Vector3 startPosition;
    public Vector3 endPosition;

    public float airPressure;           // 空気圧
    public float water;                 // 水量
    public float flyingDistance;        // 飛距離

    const float outerPressure = 101.3f; // 大気圧
    const float nozzleArea = 0.012f;    // ペットボトルの口の面積
    const float gravity = 9.8f;         // 重力
    const float miri = 0.001f;          // ミリ単位
    const float pressurRatio = 0.928f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>(); // このオブジェクトのRigidbodyを取得
        startPosition = transform.position;        // 初期位置をこのオブジェクトの位置に                      
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(-airresistance * rb.velocity); //空気抵抗を加える
        if (Input.GetKeyUp(KeyCode.Return))//enterキーを押した時
        {
            Launch();
        }

        //高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        if (startPosition.x < transform.position.x)
        {
            endPosition = transform.position;
            flyingDistance = Vector3.Distance(endPosition, startPosition);
        }
    }

    void Launch()
    {
        const float launchRadian = 60.0f * Mathf.Deg2Rad;
        Vector3 forceDirection = new Vector3(Mathf.Cos(launchRadian), Mathf.Sin(launchRadian), 0f); //射出角度
        Vector3 windForceDirection = new Vector3(1.0f, 1.2f, 0); //風力の向き

        const float airRatio = 2 / 7;                    // 空気の比率
        float waterPressure = water * miri * gravity;    // 水量から水圧を計算
        float allPressure = airPressure + waterPressure; // 気圧の合計を算出
        float pre = allPressure / outerPressure;

        double num = Mathf.Pow(pre, airRatio);
        float injectionForce = 7 * nozzleArea * allPressure * ((float)num - 1) + nozzleArea * (allPressure * pressurRatio - outerPressure);//力の値を計算
        windforce = injectionForce * windForceDirection; // 風力を決定

        Vector3 force = injectionForce * forceDirection; // 発射の力を設定

        rb.AddForce(windforce, ForceMode.Impulse);       // 風力を加える
        rb.AddForce(force, ForceMode.Impulse);           // ロケットを発射
    }
}