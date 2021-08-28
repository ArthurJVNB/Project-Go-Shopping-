using UnityEngine;

namespace SIM.Core
{
    public class LayerSorterer : MonoBehaviour
    {
        [SerializeField] bool isStatic = false;
        [SerializeField] bool useLocalScaleY = false;
        [SerializeField] float offsetY = 0f;
        [SerializeField] int refreshRate = 30;

        Renderer myRenderer;
        float refreshRateInSeconds;
        float time = Mathf.Infinity;

        private void Awake()
        {
            refreshRateInSeconds = 1 / (float)refreshRate;
            myRenderer = GetComponent<Renderer>();
        }

        private void LateUpdate()
        {
            time += Time.deltaTime;
            if (time > refreshRateInSeconds)
            {
                time = 0;
                UpdateSortingLayer();
            }
        }

        private void UpdateSortingLayer()
        {
            const float SPREAD_RESULT = 1000f;
            float order;

            if (useLocalScaleY) { order = -(transform.position.y + transform.localScale.y * offsetY); }
            else { order = -(transform.position.y + offsetY); }

            order *= SPREAD_RESULT;

            myRenderer.sortingOrder = (int)order;

            if (isStatic) Destroy(this);
        }

        private void OnDrawGizmosSelected()
        {
            const float SPHERE_RADIUS = .1f;
            Vector3 position;

            if (useLocalScaleY)
                position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y * offsetY);
            else
                position = new Vector3(transform.position.x, transform.position.y + offsetY);


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, SPHERE_RADIUS);
        }
    }
}