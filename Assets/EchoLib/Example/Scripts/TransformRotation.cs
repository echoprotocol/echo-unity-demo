using UnityEngine;


public class TransformRotation : CustomTools.CachedBehaviour.CachedMonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        CachedTransform.Rotate((Vector3.right * 20f + Vector3.up * 60f) * Time.deltaTime, Space.Self);
    }
}