using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Vizex.Connecting;
using Vizex.PlatformMoving;

namespace Vizex.Main
{
    [RequireComponent (typeof(ConnectionCreator))]
    public class Main : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private GameObject _spawnableObject;

        protected const int _objectsCount = 10;

        public float Radius = 10;

        protected Plane _plane;

        private void Awake()
        {
            _plane = new Plane(_spawnTransform.up, _spawnTransform.position);
        }

        protected virtual void Start()
        {
            ConnectionAnchor[] connectionAnchors = new ConnectionAnchor[_objectsCount];
            for (int i = 0; i < _objectsCount; i++)
            {
                GameObject newObj = Instantiate(_spawnableObject, _spawnTransform.position + Vector2InPlaneToWorld(Radius * Random.insideUnitCircle, _spawnTransform), _spawnTransform.rotation, transform);

                PlatformMover platformMover = newObj.GetComponent<PlatformMover>();
                if (platformMover != null)
                {
                    platformMover.Init(_plane);
                }
                else
                {
                    Debug.LogError("Spawnable object doesn't have platform mover");
                }

                connectionAnchors[i] = newObj.GetComponentInChildren<ConnectionAnchor>();
                if (connectionAnchors[i] == null)
                {
                    Debug.LogError("Spawnable object doesn't have connection anchor");
                }
            }

            GetComponent<ConnectionCreator>().Init(connectionAnchors);
        }

        protected Vector3 Vector2InPlaneToWorld(Vector2 vector2, Transform planeTransform)
        {
            return vector2.x * planeTransform.forward + vector2.y * planeTransform.right;
        }
    }
}
