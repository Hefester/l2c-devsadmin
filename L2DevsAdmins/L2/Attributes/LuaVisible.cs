using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2.Net.Attributes
{
    /// <summary>
    /// Attribute to use to view code LUA in script.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class LuaVisible : System.Attribute
    {
        public bool Visible = false;
    }
}
