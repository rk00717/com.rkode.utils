#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RKode.Utils {
public static class SceneUtility {
    public static Vector3 GetSceneCenterPosition(float depth = 10f) {
        var centerPosition = Vector3.zero;

        var sceneView = SceneView.lastActiveSceneView;
        if(sceneView == null) {
            Debug.LogError("No SceneView is Open.");
            return centerPosition;
        }

        var sceneViewCamera = sceneView.camera;
        if(sceneViewCamera == null) {
            Debug.LogError("Scene View Camera not found.");
            return centerPosition;
        }

        var ray = sceneViewCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
            centerPosition = hit.point;
        }else {
            centerPosition = sceneViewCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, depth));
        }

        return centerPosition;
    }
}
}
#endif