using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    /// <summary>
    /// Базовый класс для сущностей
    /// </summary>
    public class Entity 
    {
        public Entity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        protected bool Equals(Entity other)
        {
            return Id == other.Id && CreatedDate.Equals(other.CreatedDate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ CreatedDate.GetHashCode();
            }
        }
        [Required]
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Entity) obj);
        }
    }
}
