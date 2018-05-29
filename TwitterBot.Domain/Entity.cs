using System;
using System.ComponentModel.DataAnnotations;

namespace TwitterBot.Domain
{
    public abstract class Entity
    {
        [Key]
        public virtual Guid? Id { get; set; }
    }
}