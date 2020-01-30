using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    const float NOZZLE_AREA = 0.012f; // ペットボトルの口の面積
    const float GRAVITY = 9.8f; // 重力
    const float miri = 0.001f; // ミリ単位
    const float AIR_RATIO = 2 / 7; // 空気の比率
    const float pressurRatio = 0.928f;
    const float airDensity = 1.1760f; // 空気密度, 27 degrees celsius, 1013hPa

    private Rigidbody rb; // Rigidbodyの宣言

    private GameObject rocket;

    private Vector3 forceDirection = new Vector3(1.0f, 1.2f, 0f); // 射出角度
    private Vector3 windForceDirection = new Vector3(1.0f, 1.2f, 0); // 風力の向き

    private Vector3 force; // ロケットに与える力
    private Vector3 windForce; // 風力

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Transform highestTramsform;
    private float timeElapsed;
    public float timeOut;
    public float flyingDistance; // 飛んだ距離

    public float injectionForce;//力の値

    public float airPressure;//空気圧
    public float nozzlePressure;//大気圧
    public float waterVolume;//水量
    // public float wingArea;//翼の面積
    // public float flySpeed;//速さ
    public float highestX;
    public float highestY;
    public float highestZ;

    private float waterPressure;// 水圧
    private float pressure;// 気圧/大気圧の値
    private float allPressure;// 気圧の合計

    private Quaternion from = Quaternion.Euler(0, 0, 0);
    private Quaternion to = Quaternion.Euler(0, 0, -90);

    private float t = 0f;
    private bool a = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // 初期位置をこのオブジェクトの位置に
        waterPressure = waterVolume * miri * GRAVITY; // 水量から水圧を計算
        rb = this.GetComponent<Rigidbody>(); // このオブジェクトのRigidbodyを取得
        highestY = this.transform.position.y; // 最高点の記録
        pressure = allPressure / nozzlePressure;
        // for debug (display value to console)
        allPressure = airPressure + waterPressure; // 気圧の合計を算出
        this.transform.rotation = Quaternion.Euler(0, 0, -45);
    }

    // Update is called once per frame
    void Update() {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeOut) {
            if (highestY < this.transform.position.y) {
                highestX = this.transform.position.x;
                highestY = this.transform.position.y;
                highestZ = this.transform.position.z;
            }

            timeElapsed = 0.0f;
        }
        if (t < 0 && a == true) {
            t += Time.deltaTime;
            this.transform.rotation = Quaternion.Slerp(from, to, t);
        }
        // ref: http://www.asahi-net.or.jp/~hy9n-knk/sec4.htm (12)
        injectionForce = 7 * NOZZLE_AREA * allPressure * (Mathf.Pow(pressure, AIR_RATIO) - 1) + NOZZLE_AREA * (allPressure * pressurRatio - nozzlePressure);//力の値を計算
        windForce = injectionForce * windForceDirection;//風力を決定
        force = injectionForce * forceDirection;  //発射の力を設定

        //rb.AddForce(-airresistance * rb.velocity); //空気抵抗を加える
        if (this.transform.position.x > 15) {
            //this.transform.LookAt(new Vector3(-36.74836f, -0.5f, 0f), Vector3.right);
        } else {
            //this.transform.LookAt(new Vector3(5.19159f, 6.0f, 0f), Vector3.right);
        }

        if (Input.GetKeyUp(KeyCode.Return))//enterキーを押した時
        {
            rb.AddForce(windForce, ForceMode.Impulse);//風力を加える
            rb.AddForce(force, ForceMode.Impulse);//ロケットを発射
            rb.useGravity = true;
            a = true;
        }
        if (startPosition.x < transform.position.x) // 高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        {
            endPosition = transform.position;
            flyingDistance = Vector3.Distance(endPosition, startPosition);
        }

    }
}
