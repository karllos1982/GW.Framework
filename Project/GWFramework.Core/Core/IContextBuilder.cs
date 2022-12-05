using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core
{
    public interface IContextBuilder
    {
        ISettings Settings { get; set; }

        void BuilderContext(IContext context);


    }
}
