﻿namespace LetsChat.Data;

public class LetsChatDbContext : DbContext
{
    public LetsChatDbContext(DbContextOptions<LetsChatDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(x => x.Id);

        modelBuilder.Entity<Message>().HasKey(x => x.Id);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.SenderId)
            .IsUnique(false);

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.ReceiverId)
            .IsUnique(false);
    }
}
