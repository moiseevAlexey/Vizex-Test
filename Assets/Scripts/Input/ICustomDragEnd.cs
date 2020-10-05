using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vizex.CustomInput;

namespace Vizex.CustomInput
{
    public interface ICustomDragEnd : ICustomInputEvent
    {
        void OnEndDrag(CustomInputInfo info);
    }
}
