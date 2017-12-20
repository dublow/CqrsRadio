using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsRadio.Infrastructure.Providers
{
    public interface IProvider
    {
        DbConnection Create();
    }
}
