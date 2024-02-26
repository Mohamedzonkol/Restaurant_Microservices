using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.MessagesBus
{
    public class MessagesBase
    {
        public int? Id { get; set; }
        public DateTime? MessageCreated { get; set; }
    }
}
