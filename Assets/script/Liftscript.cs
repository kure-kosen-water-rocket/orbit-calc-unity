using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftscript : MonoBehaviour
{
    Forcescript fs;

    public Transform _body;
    public Transform _leftWing;
    public Transform _rightWing;
    public AnimationCurve _liftCoeff;
    public AnimationCurve _dragCoeff;
    public ParticleSystem _lparticle;
    public ParticleSystem _rparticle;
    public float _pitchSpeed = 0.5f;
    public float _rollSpeed = 0.5f;
    public float _acc = 1000f;
    public float _rho = 1.225f;
    public float _area = 12f;
    public float _initVelocity = 30f;
    public bool _useLift = true;

    public float petweit;
    public Vector3 lift;
    private Rigidbody _rigid;

    private void Start() {
        _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.forward * _initVelocity;

        _lparticle.Stop();
        _rparticle.Stop();
    }

    private void Update() {
        _rigid.AddForce(lift,ForceMode.Force);
    }

    private void FixedUpdate() {
        if (_useLift) {
            CalcLift();
        }
    }


    private void CalcLift() {
        Vector3 lpos = _leftWing.position;
        Vector3 rpos = _rightWing.position;

        Vector3 lup = _leftWing.up;
        Vector3 rup = _rightWing.up;

        Vector3 v = _rigid.velocity;

        float m = v.magnitude;
        float velocitySqr = m * m;

        Vector3 dir = v.normalized;

        // 揚力、抵抗ともに使う係数の計算（ρ/2 * q^2 * A）
        // ρ ... 密度
        // q ... 速度
        // A ... 面積
        float k = _rho / 2f * _area * velocitySqr;

        Debug.DrawLine(_body.position, _body.position + dir, Color.black);

        float ldot = Vector3.Dot(lup, dir);
        float lrad = Mathf.Acos(ldot);

        float rdot = Vector3.Dot(rup, dir);
        float rrad = Mathf.Acos(rdot);

        float langle = (lrad * Mathf.Rad2Deg) - 90f;
        float rangle = (rrad * Mathf.Rad2Deg) - 90f;

        float lcl = _liftCoeff.Evaluate(langle);
        float rcl = _liftCoeff.Evaluate(rangle);

        // 単位: N = kg・m/s^2
        float ll = lcl * k;
        float rl = rcl * k;

        // 進行方向に対しての「右」を識別する
        Vector3 lcheckDir = Vector3.Cross(_leftWing.up, dir);
        Vector3 rcheckDir = Vector3.Cross(_rightWing.up, dir);

        float lcheckd = Vector3.Dot(_leftWing.right, lcheckDir);
        float rcheckd = Vector3.Dot(_rightWing.right, rcheckDir);

        Vector3 lright = (lcheckd < 0) ? -_leftWing.right : _leftWing.right;
        Vector3 rright = (rcheckd < 0) ? -_rightWing.right : _rightWing.right;

        Vector3 ldir = Vector3.Cross(dir, lright);
        Vector3 rdir = Vector3.Cross(dir, rright);

        Vector3 lv = ldir * ll;
        Vector3 rv = rdir * rl;

        Debug.DrawLine(_leftWing.position, _leftWing.position + lv, Color.cyan);
        Debug.DrawLine(_rightWing.position, _rightWing.position + rv, Color.cyan);

        float lcd = _dragCoeff.Evaluate(langle);
        float rcd = _dragCoeff.Evaluate(rangle);

        float ldrag = lcd * k;
        float rdrag = rcd * k;

        Vector3 drag = -dir * (ldrag + rdrag);

        Debug.DrawLine(_body.position, _body.position + drag);

        Vector3 force = (lv + rv + drag) * Time.deltaTime; ;

        _rigid.AddForce(force);
    }
}