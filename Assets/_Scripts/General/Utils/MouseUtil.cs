using UnityEngine;

namespace _Scripts.General.Utils
{
    public static class MouseUtil
    {
        private static Camera camera = Camera.main;

        public static Vector3 GetMousePositionInWorld(float z = 0)
        {
            Plane dragPlane = new(camera.transform.forward, new Vector3(0, 0, z));
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            return dragPlane.Raycast(ray, out float distance)
                ? ray.GetPoint(distance)
                : Vector3.zero;
        }
    }
}