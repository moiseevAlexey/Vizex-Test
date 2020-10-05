using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vizex.CustomInput
{
    [RequireComponent(typeof(Camera))]
    public class CustomInput : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layerMask;
        [SerializeField] protected float _dragThreshold;
        [SerializeField] protected float _maxRayDistance;

        protected bool _isCurrentEventDrag;
        protected Vector3 _startMousePosition;
        protected Camera _camera;
        protected CustomInputInfo _currentInfo;
        protected LinkedList<ICustomInputEvent> _currentEvents;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startMousePosition = Input.mousePosition;

                Ray ray = _camera.ScreenPointToRay(_startMousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _maxRayDistance, _layerMask.value))
                {
                    _currentInfo = new CustomInputInfo(hit.collider.gameObject);
                    _currentEvents = new LinkedList<ICustomInputEvent>();

                    GameObject currentObject = hit.collider.gameObject;
                    while (currentObject != null)
                    {
                        ICustomInputEvent customInputEvent = currentObject.GetComponent<ICustomInputEvent>();
                        if (customInputEvent != null)
                        {
                            _currentEvents.AddLast(customInputEvent);
                        }

                        currentObject = currentObject.transform.parent?.gameObject;
                    }
                }
            }
            if (Input.GetMouseButton(0) && _currentEvents != null)
            {
                if (_isCurrentEventDrag)
                {
                    CustomDrag();
                }
                else
                {
                    if ((Input.mousePosition - _startMousePosition).magnitude >= _dragThreshold)
                    {
                        _isCurrentEventDrag = true;

                        foreach (ICustomInputEvent customEvent in _currentEvents)
                        {
                            if (customEvent is ICustomDragBegin)
                            {
                                (customEvent as ICustomDragBegin).OnBeginDrag(_currentInfo);
                            }
                        }

                        CustomDrag();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) && _currentEvents != null)
            {
                foreach (ICustomInputEvent customEvent in _currentEvents)
                {
                    if (_isCurrentEventDrag && customEvent is ICustomDragEnd)
                    {
                        (customEvent as ICustomDragEnd).OnEndDrag(_currentInfo);
                    }
                    else if (!_isCurrentEventDrag && customEvent is ICustomClick)
                    {
                        (customEvent as ICustomClick).OnClick(_currentInfo);
                    }
                }

                _isCurrentEventDrag = false;
                _currentEvents = null;
                _currentInfo = null;
            }
        }

        protected void CustomDrag()
        {
            foreach (ICustomInputEvent customEvent in _currentEvents)
            {
                if (customEvent is ICustomDrag)
                {
                    (customEvent as ICustomDrag).OnDrag(_currentInfo);
                }
            }
        }
    }
}
