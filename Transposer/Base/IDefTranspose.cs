using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public interface IDefTranspose
    {
        object DoTranspose(object value);

        string TargetProperty { get; set; }
    }
}
