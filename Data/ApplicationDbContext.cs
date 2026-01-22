using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web3_kaypic.Models;
using Web3_kaypic.Models;
using Web3_kaypic.Models.Finance;

namespace Web3_kaypic.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
    options) : base(options)
        {
        }
        //TABLES PRINCIPALES
         
        public DbSet<TTeamSeason> TTeamSeason { get; set; }
        public DbSet<TPlayer> TPlayer { get; set; }
        public DbSet<TTeamManager> TTeamManager { get; set; }

        // MESSAGERIE

        public DbSet<TMessagingPersona> TMessagingPersona { get; set; }
        public DbSet<TMessagingChat> TMessagingChat { get; set; }
        public DbSet<TMessagingChatPersonaMessage> TMessagingChatPersonaMessage { get; set; }
        public DbSet<TMessagingChatPersona> TMessagingChatPersona { get; set; }

        //MÉDIAS
        public DbSet<TMessagingMedia> TMessagingMedia { get; set; }

        // === AJOUT DU MODULE FINANCIER ===
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<StripePayment> StripePayments { get; set; }

        //NEWS / FIL D’ACTUALITÉ
        public DbSet<TNews> TNews { get; set; }

        //Sondages
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<OptionSondage> OptionsSondages { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Match> Matches { get; set; }


        //Circle
        public DbSet<TMessage> TMessages { get; set; }
        public DbSet<TMessageComment> TMessageComments { get; set; }

        //AUTRES (si tu les utilises déjà)
        public DbSet<Inscription> Inscription { get; set; }
        /*
         * protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ici tu vas mettre les Fluent API (si nécessaires)
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === TTeamSeason ===
            modelBuilder.Entity<TTeamSeason>(e =>
            {
                e.ToTable("TTeamSeason");
                e.HasKey(x => x.ts_id);
                e.Property(x => x.ts_status)
                    .HasColumnType("char(2)")
                    .HasDefaultValue("ac")
                    .IsRequired();
                e.Property(x => x.ts_chat_key)
                    .HasColumnType("varchar(64)")
                    .IsRequired();
                e.HasIndex(x => x.ts_chat_key).IsUnique();
                e.Property(x => x.ts_name)
                    .HasColumnType("nvarchar(120)")
                    .IsRequired();
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // === TMessagingPersona ===
            modelBuilder.Entity<TMessagingPersona>(e =>
            {
                e.ToTable("TMessagingPersona");
                e.HasKey(x => x.mp_id);
                e.Property(x => x.mp_status)
                    .HasColumnType("char(2)")
                    .HasDefaultValue("ac")
                    .IsRequired();
                e.Property(x => x.mp_category)
                    .HasColumnType("varchar(12)")
                    .IsRequired();
                e.Property(x => x.mp_email)
                    .HasColumnType("varchar(120)")
                    .IsRequired();
                e.HasIndex(x => x.mp_email).IsUnique();
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.Personas)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === TMessagingChat ===
            modelBuilder.Entity<TMessagingChat>(e =>
            {
                e.ToTable("TMessagingChat");
                e.HasKey(x => x.mc_id);
                e.Property(x => x.mc_status)
                    .HasColumnType("varchar(10)")
                    .HasDefaultValue("active")
                    .IsRequired();
                e.Property(x => x.mc_title)
                    .HasColumnType("nvarchar(160)");
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.Chats)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.CreatedBy)
                    .WithMany(p => p.CreatedChats)
                    .HasForeignKey(x => x.created_by_mp_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === TMessagingChatPersona ===
            modelBuilder.Entity<TMessagingChatPersona>(e =>
            {
                e.HasKey(x => x.mcp_id);

                e.HasOne(x => x.Chat)
                    .WithMany(c => c.ChatPersonas)
                    .HasForeignKey(x => x.mc_id)
                    .OnDelete(DeleteBehavior.Cascade); // OK

                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.ChatPersonas)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.NoAction); // éviter cascade multiple

                e.HasOne(x => x.Persona)
                    .WithMany(p => p.ChatPersonas)
                    .HasForeignKey(x => x.mp_id)
                    .OnDelete(DeleteBehavior.Restrict); // éviter cycle
            });

            // === TMessagingChatPersonaMessage ===
            // === TMessagingChatPersonaMessage ===
            modelBuilder.Entity<TMessagingChatPersonaMessage>(e =>
            {
                e.ToTable("TMessagingChatPersonaMessage");
                e.HasKey(x => x.mcpm_id);

                e.Property(x => x.mcpm_message)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                e.Property(x => x.is_deleted)
                    .HasColumnType("bit")
                    .HasDefaultValue(false)
                    .IsRequired();

                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                // Relation vers ChatPersona
                e.HasOne(x => x.ChatPersona)
                    .WithMany(cp => cp.Messages)
                    .HasForeignKey(x => x.mcp_id)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation vers Chat (⚠️ éviter cascade multiple → Restrict)
                e.HasOne(x => x.Chat)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(x => x.mc_id)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relation vers TeamSeason (⚠️ éviter cascade multiple → NoAction)
                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.ChatMessages)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relation vers ReplyToMessage (⚠️ éviter cascade multiple → NoAction)
                e.HasOne(x => x.ReplyToMessage)
                    .WithMany(x => x.Replies)
                    .HasForeignKey(x => x.reply_to_id)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // === TMessagingMedia ===
            modelBuilder.Entity<TMessagingMedia>(e =>
            {
                e.ToTable("TMessagingMedia");
                e.HasKey(x => x.mcm_id);
                e.Property(x => x.mcm_media_category)
                    .HasColumnType("char(1)")
                    .IsRequired();
                e.Property(x => x.mcm_url)
                    .HasColumnType("nvarchar(400)")
                    .IsRequired();
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                e.HasOne(x => x.MessagingChat)
                    .WithMany(c => c.Medias)
                    .HasForeignKey(x => x.mc_id)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.Medias)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.NoAction);
                e.HasOne(x => x.Creator)
                    .WithMany(p => p.Medias)
                    .HasForeignKey(x => x.created_by_mp_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === TNews ===
            modelBuilder.Entity<TNews>(e =>
            {
                e.ToTable("TNews");
                e.HasKey(x => x.news_id);
                e.Property(x => x.news_status)
                    .HasColumnType("varchar(10)")
                    .HasDefaultValue("active")
                    .IsRequired();
                e.Property(x => x.news_title)
                    .HasColumnType("nvarchar(160)")
                    .IsRequired();
                e.Property(x => x.news_body)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();
                e.Property(x => x.news_date_posted)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                e.Property(x => x.news_media_category)
                    .HasColumnType("char(1)")
                    .HasDefaultValue("n")
                    .IsRequired();
                e.Property(x => x.news_media_url)
                    .HasColumnType("nvarchar(400)");
                e.HasOne(x => x.TeamSeason)
                    .WithMany(s => s.News)
                    .HasForeignKey(x => x.ts_id)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.AuthorPersona)
                    .WithMany(p => p.AuthoredNews)
                    .HasForeignKey(x => x.news_author_mp_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === TTeamManager ===
            modelBuilder.Entity<TTeamManager>(e =>
            {
                e.ToTable("TTeamManager");
                e.HasKey(x => x.tm_id);
                e.HasIndex(x => x.tm_email).IsUnique();
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // === TPlayer ===
            modelBuilder.Entity<TPlayer>(e =>
            {
                e.ToTable("TPlayer");
                e.HasKey(x => x.player_id);
                e.Property(x => x.created_at)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // === TMessage ===
            modelBuilder.Entity<TMessage>(entity =>
            {
                entity.ToTable("TMessage");
                entity.HasKey(e => e.msg_id);
                entity.Property(e => e.msg_created_at).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.msg_image_url).IsRequired(false);
            });

            // === TMessageComment ===
            modelBuilder.Entity<TMessageComment>(entity =>
            {
                entity.ToTable("TMessageComment");
                entity.HasKey(e => e.comment_id);
                entity.Property(e => e.comment_created_at).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Message)
                    .WithMany(m => m.Comments)
                    .HasForeignKey(e => e.msg_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // Renommer les tables Identity
            modelBuilder.Entity<ApplicationUser>().ToTable("TUser");
            modelBuilder.Entity<IdentityRole>().ToTable("TRole");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("TUserRole");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("TUserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("TUserLogin");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("TRoleClaim");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("TUserToken");

        }

    }
}

