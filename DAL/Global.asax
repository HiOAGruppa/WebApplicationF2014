using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Global
    {           
        protected void  Application_Start(){ 
        Database.SetInitializer<StoreContext>(new StoreInitializer());
            }
    }
}
