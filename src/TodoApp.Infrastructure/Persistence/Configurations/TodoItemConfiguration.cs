using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence.Configurations;

public sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(todo => todo.Id);

        builder.Property(todo => todo.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(todo => todo.IsCompleted).IsRequired();
        builder.Property(todo => todo.CreatedAt).IsRequired();

        builder.HasIndex(todo => todo.IsCompleted);
    }
}
