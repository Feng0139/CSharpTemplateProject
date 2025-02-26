namespace TemplateProject.Core.Domain;

public interface IEntityBase { }

public interface IEntity : IEntityBase
{
    public Guid Id { get; set; }
}

public interface IEntity<T> : IEntityBase
{
    T Id { get; set; }
}

public interface IEntityCreated : IEntityBase
{
    public DateTimeOffset CreatedAt { get; set; }
}

public interface IEntityModified : IEntityBase
{
    public DateTimeOffset? UpdatedAt { get; set; }
}