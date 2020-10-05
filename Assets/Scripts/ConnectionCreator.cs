using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vizex.CustomInput;

namespace Vizex.Connecting
{
    public class ConnectionCreator : MonoBehaviour, ICustomClick, ICustomDragBegin, ICustomDrag, ICustomDragEnd
    {
        protected abstract class ConnectorCreatorState
        {
            protected ConnectionCreator _connectionCreator;

            public ConnectorCreatorState(ConnectionCreator connectionCreator)
            {
                _connectionCreator = connectionCreator;
            }

            abstract public void OnClickAnchor(ConnectionAnchor connectionAnchor);
            abstract public void OnBeginDragAnchor(ConnectionAnchor connectionAnchor);
            abstract public void OnDragAnchor();
            abstract public void OnEndDragAnchor();
            abstract public void OnEmptyClick();
        }

        protected class DisactiveState : ConnectorCreatorState
        {
            public DisactiveState(ConnectionCreator connectionCreator) : base(connectionCreator)
            { }

            public override void OnClickAnchor(ConnectionAnchor connectionAnchor)
            {
                _connectionCreator.ActivateAnchor(connectionAnchor);

                _connectionCreator._state = new AnchorClickedState(_connectionCreator);
            }
            public override void OnBeginDragAnchor(ConnectionAnchor connectionAnchor)
            {
                _connectionCreator.ActivateAnchor(connectionAnchor);

                _connectionCreator._mouseTransform.position = _connectionCreator._activeAnchor.transform.position;
                _connectionCreator._tempConnection = _connectionCreator.CreateConnection(_connectionCreator._mouseTransform, _connectionCreator._activeAnchor.transform);

                _connectionCreator._state = new AnchorDragedState(_connectionCreator);
            }

            public override void OnDragAnchor()
            { }

            public override void OnEndDragAnchor()
            { }

            public override void OnEmptyClick()
            { }
        }

        protected class AnchorClickedState : ConnectorCreatorState
        {
            public AnchorClickedState(ConnectionCreator connectionCreator) : base(connectionCreator)
            { }

            public override void OnClickAnchor(ConnectionAnchor connectionAnchor)
            {
                if (_connectionCreator._activeAnchor != connectionAnchor)
                {
                    _connectionCreator.CreateConnection(_connectionCreator._activeAnchor.transform, connectionAnchor.transform);
                }
                _connectionCreator.DisactivateAnchors();

                _connectionCreator._state = new DisactiveState(_connectionCreator);

            }

            public override void OnBeginDragAnchor(ConnectionAnchor connectionAnchor)
            { }

            public override void OnDragAnchor()
            { }

            public override void OnEndDragAnchor()
            { }

            public override void OnEmptyClick()
            {
                _connectionCreator.DisactivateAnchors();

                _connectionCreator._state = new DisactiveState(_connectionCreator);
            }
        }

        protected class AnchorDragedState : ConnectorCreatorState
        {
            public AnchorDragedState(ConnectionCreator connectionCreator) : base(connectionCreator)
            { }

            public override void OnClickAnchor(ConnectionAnchor connectionAnchor)
            {
                /*_connectionCreator.ActivateAnchor(connectionAnchor);

                Destroy(_connectionCreator._tempConnection.gameObject);

                _connectionCreator._state = new AnchorClickedState(_connectionCreator);*/
            }
            public override void OnBeginDragAnchor(ConnectionAnchor connectionAnchor)
            { }

            public override void OnDragAnchor()
            {
                Ray ray = _connectionCreator._camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ConnectionAnchor overedAnchor = hit.collider.GetComponent<ConnectionAnchor>();
                    if (overedAnchor != null && _connectionCreator._overedAnchor != overedAnchor && overedAnchor != _connectionCreator._activeAnchor)
                    {
                        overedAnchor.SetActive();
                        _connectionCreator._overedAnchor?.SetPossible();
                        _connectionCreator._overedAnchor = overedAnchor;
                    }
                    else if (overedAnchor == null && _connectionCreator._overedAnchor != null)
                    {
                        _connectionCreator._overedAnchor.SetPossible();
                        _connectionCreator._overedAnchor = null;
                    }
                }

                float enter;
                if (_connectionCreator._plane.Raycast(ray, out enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    _connectionCreator._mouseTransform.position = hitPoint;
                }
            }

            public override void OnEndDragAnchor()
            {
                if (_connectionCreator._overedAnchor != null)
                {
                    _connectionCreator._tempConnection.First = _connectionCreator._overedAnchor.transform;
                }
                else
                {
                    Destroy(_connectionCreator._tempConnection.gameObject);
                }

                _connectionCreator.DisactivateAnchors();

                _connectionCreator._state = new DisactiveState(_connectionCreator);
            }

            public override void OnEmptyClick()
            { }
        }

        [SerializeField] protected GameObject _connectionObject;

        protected ConnectionAnchor _activeAnchor;
        protected ConnectionAnchor _overedAnchor;
        protected ConnectionAnchor[] _connectionAnchors;
        protected Connection _tempConnection;
        protected ConnectorCreatorState _state;
        protected Transform _mouseTransform;
        protected Camera _camera;
        protected Plane _plane;

        private void Awake()
        {
            _mouseTransform = new GameObject().transform;
            _state = new DisactiveState(this);
            _camera = Camera.main;
        }

        public void Init(ConnectionAnchor[] connectionAnchors)
        {
            _connectionAnchors = connectionAnchors;
            if (_connectionAnchors.Length > 0)
            {
                _plane = new Plane(_connectionAnchors[0].transform.up, _connectionAnchors[0].transform.position);
            }
        }

        protected void ActivateAnchor(ConnectionAnchor connectionAnchor)
        {
            foreach (ConnectionAnchor anchor in _connectionAnchors)
            {
                anchor.SetPossible();
            }
            connectionAnchor.SetActive();
            _activeAnchor = connectionAnchor;
        }

        protected void DisactivateAnchors()
        {
            foreach (ConnectionAnchor anchor in _connectionAnchors)
            {
                anchor.SetDisactive();
            }
        }

        protected Connection CreateConnection(Transform first, Transform second)
        {
            GameObject newObj = Instantiate(_connectionObject, transform);
            Connection connection = newObj.GetComponent<Connection>();
            connection.Init(first, second);
            return connection;
        }

        public void OnClick(CustomInputInfo info)
        {
            ConnectionAnchor connectionAnchor = info.CollidedGameObject.GetComponent<ConnectionAnchor>();
            if (connectionAnchor != null)
            {
                _state.OnClickAnchor(connectionAnchor);
            }
            else
            {
                _state.OnEmptyClick();
            }
        }

        public void OnBeginDrag(CustomInputInfo info)
        {
            ConnectionAnchor connectionAnchor = info.CollidedGameObject.GetComponent<ConnectionAnchor>();
            if (connectionAnchor != null)
            {
                _state.OnBeginDragAnchor(connectionAnchor);
            }
            else
            {
                _state.OnEmptyClick();
            }
        }

        public void OnDrag(CustomInputInfo info)
        {
            _state.OnDragAnchor();
        }

        public void OnEndDrag(CustomInputInfo info)
        {
            _state.OnEndDragAnchor();
        }
    }
}
