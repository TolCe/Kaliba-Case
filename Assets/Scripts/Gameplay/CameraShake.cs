using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;
    private float timeAtCurrentFrame;
    private float timeAtLastFrame;
    private float fakeDelta;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        timeAtCurrentFrame = Time.realtimeSinceStartup;
        fakeDelta = timeAtCurrentFrame - timeAtLastFrame;
        timeAtLastFrame = timeAtCurrentFrame;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake(0.1f, 0.1f);
        }
    }

    public void Shake(float duration, float amount)
    {
        originalPos = transform.localPosition;
        StopAllCoroutines();
        StartCoroutine(CShake(duration, amount));
    }

    private IEnumerator CShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            duration -= fakeDelta;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
