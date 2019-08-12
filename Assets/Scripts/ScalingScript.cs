using UnityEngine;
using System.Collections;

public class ScalingScript : MonoBehaviour
{

    public float orthographicSize = 5;
    public float aspect = 1136/640;
    void Start()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                Camera.main.nearClipPlane, Camera.main.farClipPlane);
    }
}
