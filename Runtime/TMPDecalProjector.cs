using UnityEngine;
using TMPro;

namespace TMPMeshDecal
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class TMPDecalProjector : MonoBehaviour
    {
        private TMP_Text textmesh;

        [Header("Decal Settings")]
        [Tooltip("How far the ray will search for a surface.")]
        public float projectionDistance = 2f;

        [Tooltip("Pushes the text slightly off the wall to prevent flickering (Z-fighting).")]
        public float surfaceOffset = 0.005f;

        [Tooltip("Which layers should the text stick to?")]
        public LayerMask targetLayers = ~0;

        [Tooltip("Cast rays from slightly behind the text in case it is clipping inside the wall.")]
        public float raycastBackOffset = 0.1f;

        private bool needsUpdate = false;
        private Vector3 lastPosition;
        private Quaternion lastRotation;
        private Vector3 lastScale;

        void Awake()
        {
            textmesh = GetComponent<TMP_Text>();
        }

        void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
            needsUpdate = true;
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
            textmesh.ForceMeshUpdate();
        }

        void OnValidate()
        {
            UpdateDecal();
        }

        private void OnTextChanged(Object obj)
        {
            if (obj == textmesh)
            {
                needsUpdate = true;
            }
        }

        void LateUpdate()
        {
            if (transform.position != lastPosition ||
                transform.rotation != lastRotation ||
                transform.localScale != lastScale)
            {
                needsUpdate = true;
                lastPosition = transform.position;
                lastRotation = transform.rotation;
                lastScale = transform.localScale;
            }

            if (needsUpdate)
            {
                UpdateDecal();
                needsUpdate = false;
            }
        }

        private void UpdateDecal()
        {
            if (textmesh == null) textmesh = GetComponent<TMP_Text>();

            textmesh.ForceMeshUpdate();
            TMP_TextInfo textInfo = textmesh.textInfo;

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                Vector3[] vertices = meshInfo.vertices;
                Vector3[] normals = meshInfo.normals;

                for (int j = 0; j < vertices.Length; j++)
                {
                    Vector3 worldVert = transform.TransformPoint(vertices[j]);
                    Vector3 rayOrigin = worldVert - (transform.forward * raycastBackOffset);

                    if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, projectionDistance + raycastBackOffset, targetLayers))
                    {
                        Vector3 projectedWorldPos = hit.point + (hit.normal * surfaceOffset);
                        vertices[j] = transform.InverseTransformPoint(projectedWorldPos);
                        normals[j] = transform.InverseTransformDirection(hit.normal);
                    }
                    else
                    {
                    }
                }
            }

            textmesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, transform.forward * projectionDistance);
        }
    }
}