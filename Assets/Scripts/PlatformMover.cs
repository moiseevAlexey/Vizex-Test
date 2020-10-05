using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using Vizex.CustomInput;

namespace Vizex.PlatformMoving
{
    public class PlatformMover : MonoBehaviour, ICustomDragBegin, ICustomDrag, ICustomDragEnd
    {
        [SerializeField] protected GameObject _platform;

        protected bool _isCanMove;
        protected Plane _plane;
        protected Vector3 _beginDragPosition;
        protected Vector3 _beginPlatformPosition;
        protected Camera _camera;

        protected virtual void Awake()
        {
            _camera = Camera.main;
        }

        public void Init(Plane plane)
        {
            _plane = plane;
        }

        public void OnBeginDrag(CustomInputInfo info)
        {
            if (info.CollidedGameObject == _platform)
            {
                _isCanMove = true;

                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                float enter;
                if (_plane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);

                    _beginDragPosition = hitPoint;
                    _beginPlatformPosition = transform.position;
                }
            }
        }

        public void OnDrag(CustomInputInfo info)
        {
            if (_isCanMove)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                float enter;
                if (_plane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);

                    transform.position = _beginPlatformPosition + (hitPoint - _beginDragPosition);
                }
            }
        }

        public void OnEndDrag(CustomInputInfo info)
        {
            if (info.CollidedGameObject == _platform)
            {
                _isCanMove = false;
            }
        }
    }
}
