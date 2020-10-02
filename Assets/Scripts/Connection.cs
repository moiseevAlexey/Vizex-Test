using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vizex.Connecting
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] protected float _baseLength;

        [HideInInspector] public Transform First;
        [HideInInspector] public Transform Second;

        public void Init(Transform first, Transform second)
        {
            First = first;
            Second = second;

            UpdatePosition();
        }

        protected virtual void Update()
        {
            UpdatePosition();
        }

        protected void UpdatePosition()
        {
            transform.position = (First.position + Second.position) / 2;
            transform.up = First.position - Second.position;
            transform.localScale = new Vector3(transform.localScale.x, (First.position - Second.position).magnitude / _baseLength, transform.localScale.z);
        }
    }
}
