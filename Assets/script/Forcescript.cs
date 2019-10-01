using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcescript : MonoBehaviour
{
    Rigidbody rb;//Rigidbodyの宣言

    Vector3 forcedirection = new Vector3(1.0f, 1.7f, 0f);//射出角度
    Vector3 windforcedirection = new Vector3(1.0f, 1.2f, 0);//風力の向き

    Vector3 force,windforce;//ロケットに与える力、風力
  //  Vector3 look;        //最高点の方向
    public Vector3  startposition;
    public Vector3 endposition;
  
    public float injectionforce;//力の値 
    public float nozzlearea;//ペットボトルの口の面積
    public float airpressure;//空気圧
    public float outpressure;//大気圧
    public float water;//水量
    public float flyinddistance;          //飛んだ距離

    float gravity=9.8f;
    float miri=0.001f;
    float airratio;
    float pressurratio=0.928f;
    float waterpressur, pre, allpressur;//水圧、気圧/大気圧の値、気圧の合計

    GameObject target;     //    最高点

    // Start is called before the first frame update
    void Start()
    {
        airratio = 2 / 7;
        pre = allpressur / outpressure;
        waterpressur = water * miri * gravity;//水量から水圧を計算
        target = gameObject;            //ターゲットをこのオブジェクトに
        rb = gameObject.GetComponent<Rigidbody>();//このオブジェクトのRigidbodyを取得
        startposition = transform.position;           //初期位置をこのオブジェクトの位置に                      
        allpressur = airpressure + waterpressur;   //気圧の合計を算出
       
    }
    // Update is called once per frame
    void Update() {
       
        double num = Mathf.Pow(pre,airratio);
        injectionforce = 7 * nozzlearea * allpressur * ((float)num-1)+nozzlearea*(allpressur*pressurratio-outpressure);//力の値を計算
        windforce = injectionforce * windforcedirection;//風力を決定
        force = injectionforce * forcedirection;  //発射の力を設定

     //   look = target.transform.position - transform.position;//最高点の座標を算出
    //  Quaternion to = Quaternion.FromToRotation(Vector3.up, look);//ロケットの向きを決定

      //  transform.rotation= Quaternion.Slerp(transform.rotation,to , Time.deltaTime);//最高点の方向を向く

        if (Input.GetKeyUp(KeyCode.Return))//enterキーを押した時
        {
            Inp();
        }
  
        if  (startposition.x< transform.position.x)//高さが1より小さく、このオブジェクトの座標が初期座標より大きい時
        {
            endposition = transform.position;//到達地点を設定
            flyinddistance = Vector3.Distance(endposition, startposition);
           
        }
    }
    void Inp()
    {
        rb.AddForce(windforce, ForceMode.Impulse);//風力を加える
        rb.AddForce(force, ForceMode.Impulse);//ロケットを発射
    }
}