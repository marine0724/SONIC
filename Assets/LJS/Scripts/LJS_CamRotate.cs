using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_CamRotate : MonoBehaviour
{
    public static LJS_CamRotate Instance;

    private void Awake()
    {
        Instance = this;
    }

    float distance = 8f;
    Vector3 reDistance;

    public float x = 0;
    public float y = 0;


    // ī�޶� ��鸲
    private float shakeTime;
    private float shakeInstensity;

    [Header("ī�޶� y ��")]
    public float cameraPosition_Y = 0f;
    [Header("ī�޶� �Ÿ�")]
    public float cameraZoomDistance = 5;

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
        //ī�޶��� �̼��� ��鸲


        // isSensitive ���� true �̸�
        if (LJS_Player.Instance.isBuzz == true)
        {
            transform.position = transform.position + Random.insideUnitSphere * (LJS_Player.Instance.speed * 0.0015f);
        }
        else
            return;

    }

    private void CaemraRotate()
    {

        // ���콺 �¿� �̵� ����
        x += Input.GetAxis("Mouse X");
        // ���콺 ���� �̵� ����
        y -= Input.GetAxis("Mouse Y");

        // �̵����� ���� ī�޶� �ٶ󺸴� ���� ����
        transform.rotation = Quaternion.Euler(y, x, 0);



        // ���ư� �� �ִ� ���� ����
        y = Mathf.Clamp(y, -10, 30);

        //ī�޶�� �÷��̾��� �Ÿ�����
        reDistance = new Vector3(0, -cameraPosition_Y, distance);
        transform.position = LJS_Player.Instance.transform.position - transform.rotation * reDistance;
    }

    private void ZoomOut()
    {

        if (LJS_Player.Instance.isDash == true)
        {
            distance = Mathf.Lerp(distance, cameraZoomDistance + 2, 4 * Time.deltaTime);

        }

    }

    private void ZoomIn()
    {

        if (LJS_Player.Instance.isDash == false)
        {
            distance = Mathf.Lerp(distance, cameraZoomDistance, 4 * Time.deltaTime);

        }
    }

    #region  ī�޶� ��鸲(��ġ)
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
            // �ʱ� ��ġ ���� �� ���� * ������ŭ�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.position = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * shakeInstensity , 1);

            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = LJS_Player.Instance.transform.position - transform.rotation * reDistance;

    }
    #endregion

    #region ī�޶� ��鸲(����)
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

            // �ʱ� ��ġ ���� �� ���� * ������ŭ�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeInstensity * power);

            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        //��鸮�� ������ ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(startRotation);
    }
    #endregion
}