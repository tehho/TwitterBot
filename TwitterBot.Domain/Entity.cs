using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }
    }
}
