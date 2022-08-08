using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    // 싱글톤
    public static CamShake Instance;

    private void Awake()
    {
        Instance = this;
    }


    private float shakeTime;
    private float shakeInstensity;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShakeCamera(float shakeTime = 1.0f, float shakeInstensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeInstensity = shakeInstensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");

    }
    public void OnShakeCamera_Rot(float shakeTime = 1.0f, float shakeInstensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeInstensity = shakeInstensity;

        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation");

    }







    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = transform.position;

        while (shakeTime > 0.0f)
        {
            // 초기 위치 부터 구 범위 * 강도만큼의 범위 안에서 카메라 위치 변동
            transform.position = Vector3.Lerp(transform.position, startPosition + Random.insideUnitSphere * shakeInstensity, Time.deltaTime);

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
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

}
