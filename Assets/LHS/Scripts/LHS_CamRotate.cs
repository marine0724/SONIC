using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_CamRotate : MonoBehaviour
{
    public static LHS_CamRotate Instance;

    private void Awake()
    {
        Instance = this;
    }

    float distance = 13f;
    Vector3 reDistance;

    float x = 0;
    float y = 0;


    // 카메라 흔들림
    private float shakeTime;
    private float shakeInstensity;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CaemraRotate();
        ZoomOut();
        ZoomIn();
        CameraBuzz();
    }

    public void CameraBuzz()
    {
        //카메라의 미세한 흔들림


        // isSensitive 값이 true 이면
        if (Player.Instance.isBuzz == true)
        {
            transform.position = transform.position + Random.insideUnitSphere * (Player.Instance.speed * 0.003f);
        }
        else
            return;

    }

    private void CaemraRotate()
    {

        // 마우스 좌우 이동 누적
        x += Input.GetAxis("Mouse X");
        // 마우스 상하 이동 누적
        y -= Input.GetAxis("Mouse Y");

        // 이동량에 따라 카메라가 바라보는 방향 조정
        transform.rotation = Quaternion.Euler(y, x, 0);
        // 돌아갈 수 있는 각도 제한
        y = Mathf.Clamp(y, -10, 30);

        //카메라와 플레이어의 거리조정
        reDistance = new Vector3(0, -4f, distance);
        transform.position = Player.Instance.transform.position - transform.rotation * reDistance;
    }


    private void ZoomOut()
    {

        if (Player.Instance.isDash == true)
        {
            distance = Mathf.Lerp(distance, 40, 4 * Time.deltaTime);

        }

    }


    private void ZoomIn()
    {

        if (Player.Instance.isDash == false)
        {
            distance = Mathf.Lerp(distance, 13, 4 * Time.deltaTime);

        }
    }

    #region  카메라 흔들림(위치)
    public void OnShakeCamera(float shakeTime = 1.0f, float shakeInstensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeInstensity = shakeInstensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }



    private IEnumerator ShakeByPosition()
    {

        while (shakeTime > 0.0f)
        {
            // 초기 위치 부터 구 범위 * 강도만큼의 범위 안에서 카메라 위치 변동
            transform.position = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * shakeInstensity , 1);

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = Player.Instance.transform.position - transform.rotation * reDistance;

    }
    #endregion

    #region 카메라 흔들림(각도)
    public void OnShakeCamera_Rot(float shakeTime = 1.0f, float shakeInstensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeInstensity = shakeInstensity;

        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation");

    }


    private IEnumerator ShakeByRotation()
    {
        Vector3 startRotation = transform.eulerAngles;

        float power = 10f;

        while (shakeTime > 0.0f)
        {

            float x = 0;
            float y = 0;
            float z = Random.Range(-1f, 1f);

            // 초기 위치 부터 구 범위 * 강도만큼의 범위 안에서 카메라 위치 변동
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeInstensity * power);

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        //흔들리기 직전의 회전 값으로 설정
        transform.rotation = Quaternion.Euler(startRotation);
    }
    #endregion
}
