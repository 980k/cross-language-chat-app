using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CrossLangChat.Models;

namespace CrossLangChat.Data
{
    public class CrossLangChatContext : DbContext
    {
        public CrossLangChatContext (DbContextOptions<CrossLangChatContext> options)
            : base(options)
        {
        }

        public DbSet<CrossLangChat.Models.User> User { get; set; } = default!;

        public DbSet<CrossLangChat.Models.ChatRoom> ChatRoom { get; set; } = default!;

        public DbSet<CrossLangChat.Models.Message> Message { get; set; } = default!;

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatRooms)
                .WithMany(cr => cr.Participants);
                

            // Other configurations for your entities if needed...

            base.OnModelCreating(modelBuilder);
        }
    }
}
