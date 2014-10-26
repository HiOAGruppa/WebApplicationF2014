using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class Global
    {            
        Database.SetInitializer<StoreContext>(new StoreInitializer());
    }
}
