using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftscript : MonoBehaviour
{
    [SerializeField]
    private Transform _body;

    [SerializeField]
    private Transform _leftWing;

    [SerializeField]
    private Transform _rightWing;

    [SerializeField]
    private AnimationCurve _liftCoeff;

    [SerializeField]
    private AnimationCurve _dragCoeff;

    [SerializeField]
    private ParticleSystem _lparticle;

    [SerializeField]
    private ParticleSystem _rparticle;

    [SerializeField]
    private float _pitchSpeed = 0.5f;

    [SerializeField]
    private float _rollSpeed = 0.5f;

    [SerializeField]
    private float _acc = 1000f;

    [SerializeField]
    private float _rho = 1.225f;

    [SerializeField]
    private float _area = 12f;

    [SerializeField]
    private float _initVelocity = 30f;

    [SerializeField]
    private bool _useLift = true;

    private Rigidbody _rigid;

    private void Start() {
        _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.forward * _initVelocity;

        _lparticle.Stop();
        _rparticle.Stop();
    }

    private void Update() {
        Control();
    }

    private void FixedUpdate() {
        if (_useLift) {
            CalcLift();
        }
    }

    private void Control() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _lparticle.Play();
            _rparticle.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            _lparticle.Stop();
            _rparticle.Stop();
        }

        if (Input.GetKey(KeyCode.Space)) {
            _rigid.AddForce(_body.forward * _acc);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            _body.Rotate(_body.worldToLocalMatrix.MultiplyVector(_body.right), -_pitchSpeed);
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            _body.Rotate(_body.worldToLocalMatrix.MultiplyVector(_body.right), _pitchSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            _body.Rotate(_body.worldToLocalMatrix.MultiplyVector(_body.forward), _rollSpeed);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            _body.Rotate(_body.worldToLocalMatrix.MultiplyVector(_body.forward), -_rollSpeed);
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