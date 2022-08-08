using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    // �̱���
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
            // �ʱ� ��ġ ���� �� ���� * ������ŭ�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.position = Vector3.Lerp(transform.position, startPosition + Random.insideUnitSphere * shakeInstensity, Time.deltaTime);

            // �ð� ����
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

            // �ʱ� ��ġ ���� �� ���� * ������ŭ�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeInstensity * power);

            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        //��鸮�� ������ ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(startRotation);
    }

}
