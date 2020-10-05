using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vizex.CustomInput
{
    public interface ICustomClick : ICustomInputEvent
    {
        void OnClick(CustomInputInfo info);
    }
}
