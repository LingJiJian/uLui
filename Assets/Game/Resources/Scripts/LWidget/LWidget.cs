using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lui
{
    public delegate void LAction();
    public delegate void LAction<T0>(T0 arg0);
    public delegate void LAction<T0, T1>(T0 arg0, T1 arg1);

    public delegate T0 LDataSourceAdapter<T0, T1>(T0 arg0, T1 arg1);
}
