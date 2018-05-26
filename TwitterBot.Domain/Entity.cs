using System.ComponentModel.DataAnnotations;

namespace TwitterBot.Domain
{
    public abstract class Entity
    {
        [Key]
        public virtual int? Id { get; set; }
    }
}