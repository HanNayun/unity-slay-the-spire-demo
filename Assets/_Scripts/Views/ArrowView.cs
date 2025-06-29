using _Scripts.General.Utils;
using UnityEngine;

namespace _Scripts.Views
{
    public class ArrowView : MonoBehaviour
    {
        [SerializeField]
        private GameObject arrowHead;

        [SerializeField]
        private LineRenderer lineRenderer;

        private Vector3 startPosition;

        private void Update()
        {
            Vector3 endPos = MouseUtil.GetMousePositionInWorld();
            Vector3 dir = (arrowHead.transform.position - startPosition).normalized;
            lineRenderer.SetPosition(1, endPos - dir * .5f);
            arrowHead.transform.position = endPos;
            arrowHead.transform.right = dir;
        }

        public void Setup(Vector3 startPosition)
        {
            this.startPosition = startPosition;
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorld());
        }
    }
}