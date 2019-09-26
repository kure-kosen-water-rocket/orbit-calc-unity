using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    Rigidbody rb;//Rigidbodyの宣言

    Vector3 forceway = new Vector3(1.0f, 1.7f, 0f);//射出角度
    Vector3 windforcewind = new Vector3(1.0f, 1.2f, 0);//風力の向き

    Vector3 force,force1;//ロケットに与える力、風力
    Vector3 look;        //最高点の方向
    public Vector3  startposi;
    public Vector3 endposi;
  
    public float forceMagnitude;//力の値 
    public float area,pressure,outpressure,water;//ペットボトルの口の面積,空気圧、大気圧、水量
    public float dis;          //飛んだ距離

    public bool calculation;   //距離計算のスイッチ

    float waterpressur, pre, allpre;//水圧、気圧/大気圧の値、気圧の合計

    GameObject target;     //    最高点


   

   

    // Start is called before the first frame update
    void Start()
    {
        pre = allpre / outpressure;
        waterpressur = water * 0.001f * 9.8f;//水量から水圧を計算
        target = gameObject;            //ターゲットをこのオブジェクトに
        rb = gameObject.GetComponent<Rigidbody>();//このオブジェクトのRigidbodyを取得
        startposi = transform.position;           //初期位置をこのオブジェクトの位置に
        calculation = true;                 //距離計算をオフ
        allpre = pressure + waterpressur;   //気圧の合計を算出
        
    }
    // Update is called once per frame
    void Update() {
      
        double num = Mathf.Pow(pre, 2 / 7);
        forceMagnitude = 7 * area * allpre * ((float)num-1)+area*(allpre*0.928f-outpressure);//力の値を計算
        force1 = forceMagnitude * windforcewind;//風力を決定
        force = forceMagnitude * forceway;  //発射の力を設定

        look = target.transform.position - transform.position;//最高点の座標を算出
       Quaternion to = Quaternion.FromToRotation(Vector3.up, look);//ロケットの向きを決定

        transform.rotation= Quaternion.Slerp(transform.rotation,to , Time.deltaTime);//最高点の方向を向く

        if (Input.GetKeyDown(KeyCode.Return))//enterキーを押した時
        {
            Inp();//実行
        }
        if ((transform.position.y <= 1) && (startposi.x< transform.position.x))//高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        {
            endposi = transform.position;//到達地点を設定
            if (calculation==true)//距離の計算をオン
            {
                dis = Vector3.Distance(endposi, startposi);//距離の計算
                calculation = false;//距離計算をオフ
            }
        }


    }

    void Inp()
    {
        rb.AddForce(force1, ForceMode.Impulse);//風力を加える
        rb.AddForce(force, ForceMode.Impulse);//ロケットを発射
    }


}