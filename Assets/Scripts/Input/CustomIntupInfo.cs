using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vizex.CustomInput
{
    public class CustomInputInfo
    {
        public GameObject CollidedGameObject;

        public CustomInputInfo(GameObject gameObject)
        {
            CollidedGameObject = gameObject;
        }
    }
}
