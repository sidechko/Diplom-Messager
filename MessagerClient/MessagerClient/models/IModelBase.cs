using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagerClient.models
{
    public interface IModelBase<T>
    {
        public bool IsNull();

        public void Update(T? obj, bool soft) { }
    }
}
