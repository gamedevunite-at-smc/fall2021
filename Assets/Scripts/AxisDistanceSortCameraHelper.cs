using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class AxisDistanceSortCameraHelper : MonoBehaviour
{
    void Start()
    {
        var camera = GetComponent<Camera>();
        camera.transparencySortMode = TransparencySortMode.CustomAxis;
        camera.transparencySortAxis = new Vector3(0.0f, 1.0f, -0.49f);

#if UNITY_EDITOR
        foreach (SceneView sv in SceneView.sceneViews)
        {
            sv.camera.transparencySortMode = TransparencySortMode.CustomAxis;
            sv.camera.transparencySortAxis = new Vector3(0.0f, 1.0f, -0.49f);
        }
#endif      
    }
}