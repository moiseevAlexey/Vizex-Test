using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vizex.CustomInput
{
    public interface ICustomDrag : ICustomInputEvent
    {
        void OnDrag(CustomInputInfo info);
    }
}
