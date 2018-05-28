using System.ComponentModel.DataAnnotations;

namespace TwitterBot.Domain
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }
    }
}