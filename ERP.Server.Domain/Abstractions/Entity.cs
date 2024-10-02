using MongoDB.Bson.Serialization.Attributes;

namespace ERP.Server.Domain.Abstractions;
public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    [BsonId]
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public DateTime? DeletedUserId { get; set; }
    public DateTime? DeletedUser { get; set; }

    public DateTime? CreatedUserId { get; set; }
    public DateTime? CreatedUser { get; set; }
    public DateTime? UpdatedUserId { get; set; }
    public DateTime? UpdatedUser { get; set; }
}
