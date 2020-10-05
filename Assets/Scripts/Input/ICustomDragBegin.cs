using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vizex.CustomInput
{
    public interface ICustomDragBegin : ICustomInputEvent
    {
        void OnBeginDrag(CustomInputInfo info);
    }
}
