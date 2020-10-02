using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vizex.Connecting
{
    public class ConnectionAnchor : MonoBehaviour
    {
        public void SetActive()
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        public void SetPossible()
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        public void SetDisactive()
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
