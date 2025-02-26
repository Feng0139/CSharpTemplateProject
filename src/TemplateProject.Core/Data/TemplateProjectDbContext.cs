using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TemplateProject.Core.Domain;
using TemplateProject.Core.Settings.System;

namespace TemplateProject.Core.Data;

public class TemplateProjectDbContext(ConnectionStringSetting connectionString) : DbContext, IUnitOfWork
{
    private readonly string _dbConnectionString = connectionString.Mysql;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_dbConnectionString, new MySqlServerVersion(new Version(8, 0, 3)));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var allTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && typeof(IEntity).IsAssignableFrom(t))
            .ToList();

        foreach (var type in allTypes.Where(type => modelBuilder.Model.FindEntityType(type) == null))
        {
            modelBuilder.Model.AddEntityType(type);
        }
    }
    
    public bool ShouldSaveChanges { get; set; }
    
    public override int SaveChanges()
    {
        UpdateCreatedFields();
        UpdateLastModifiedFields();
        return ShouldSaveChanges ? base.SaveChanges() : 0;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateCreatedFields();
        UpdateLastModifiedFields();
        return ShouldSaveChanges ? base.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    private void UpdateCreatedFields()
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e is { State: EntityState.Added, Entity: IEntityCreated }))
        {
            if (((IEntityCreated)entry.Entity).CreatedAt == default)
            {
                ((IEntityCreated)entry.Entity).CreatedAt= DateTimeOffset.Now;
            }
        }
    }
    
    private void UpdateLastModifiedFields()
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e is { State: EntityState.Modified, Entity: IEntityModified }))
        {
            ((IEntityModified)entry.Entity).UpdatedAt= DateTimeOffset.Now;
        }
    }
}