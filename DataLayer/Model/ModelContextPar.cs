using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public partial class ModelContext
    {
        public ModelContext()
            : base(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {
        }
    }
}
