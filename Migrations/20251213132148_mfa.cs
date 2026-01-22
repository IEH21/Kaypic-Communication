using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web3_kaypic.Migrations
{
    /// <inheritdoc />
    public partial class mfa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Courriel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmerMotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TTeamManager",
                columns: table => new
                {
                    tm_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tm_fname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    tm_lname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    tm_email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTeamManager", x => x.tm_id);
                });

            migrationBuilder.CreateTable(
                name: "TUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRoleClaim_TRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TTeamSeason",
                columns: table => new
                {
                    ts_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ts_status = table.Column<string>(type: "char(2)", maxLength: 2, nullable: false, defaultValue: "ac"),
                    ts_chat_key = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    ts_name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    TTeamManagertm_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTeamSeason", x => x.ts_id);
                    table.ForeignKey(
                        name: "FK_TTeamSeason_TTeamManager_TTeamManagertm_id",
                        column: x => x.TTeamManagertm_id,
                        principalTable: "TTeamManager",
                        principalColumn: "tm_id");
                });

            migrationBuilder.CreateTable(
                name: "TUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TUserClaim_TUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_TUserLogin_TUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_TUserRole_TRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TUserRole_TUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TUserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_TUserToken_TUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TMessagingPersona",
                columns: table => new
                {
                    mp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    mp_status = table.Column<string>(type: "char(2)", maxLength: 2, nullable: false, defaultValue: "ac"),
                    mp_category = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    mp_email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    mp_lname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    mp_fname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessagingPersona", x => x.mp_id);
                    table.ForeignKey(
                        name: "FK_TMessagingPersona_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TMessagingChat",
                columns: table => new
                {
                    mc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    mc_status = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "active"),
                    mc_title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    created_by_mp_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessagingChat", x => x.mc_id);
                    table.ForeignKey(
                        name: "FK_TMessagingChat_TMessagingPersona_created_by_mp_id",
                        column: x => x.created_by_mp_id,
                        principalTable: "TMessagingPersona",
                        principalColumn: "mp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMessagingChat_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TNews",
                columns: table => new
                {
                    news_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    news_status = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "active"),
                    news_title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    news_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    news_date_posted = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    news_date_start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    news_date_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    news_media_category = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false, defaultValue: "n"),
                    news_media_url = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    news_author_mp_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TNews", x => x.news_id);
                    table.ForeignKey(
                        name: "FK_TNews_TMessagingPersona_news_author_mp_id",
                        column: x => x.news_author_mp_id,
                        principalTable: "TMessagingPersona",
                        principalColumn: "mp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TNews_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TPlayer",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    player_fname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    player_lname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    statut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    player_email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    player_guardian_fname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    player_guardian_lname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    player_guardian_email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    IdSaison = table.Column<int>(type: "int", nullable: false),
                    PersonneMessageriemp_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPlayer", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_TPlayer_TMessagingPersona_PersonneMessageriemp_id",
                        column: x => x.PersonneMessageriemp_id,
                        principalTable: "TMessagingPersona",
                        principalColumn: "mp_id");
                    table.ForeignKey(
                        name: "FK_TPlayer_TTeamSeason_IdSaison",
                        column: x => x.IdSaison,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TMessagingChatPersona",
                columns: table => new
                {
                    mcp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mc_id = table.Column<int>(type: "int", nullable: false),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    mp_id = table.Column<int>(type: "int", nullable: false),
                    mcp_status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    added_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessagingChatPersona", x => x.mcp_id);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersona_TMessagingChat_mc_id",
                        column: x => x.mc_id,
                        principalTable: "TMessagingChat",
                        principalColumn: "mc_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersona_TMessagingPersona_mp_id",
                        column: x => x.mp_id,
                        principalTable: "TMessagingPersona",
                        principalColumn: "mp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersona_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id");
                });

            migrationBuilder.CreateTable(
                name: "TMessagingMedia",
                columns: table => new
                {
                    mcm_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mc_id = table.Column<int>(type: "int", nullable: false),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    mcm_media_category = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    mcm_url = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    created_by_mp_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessagingMedia", x => x.mcm_id);
                    table.ForeignKey(
                        name: "FK_TMessagingMedia_TMessagingChat_mc_id",
                        column: x => x.mc_id,
                        principalTable: "TMessagingChat",
                        principalColumn: "mc_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TMessagingMedia_TMessagingPersona_created_by_mp_id",
                        column: x => x.created_by_mp_id,
                        principalTable: "TMessagingPersona",
                        principalColumn: "mp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMessagingMedia_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id");
                });

            migrationBuilder.CreateTable(
                name: "TMessagingChatPersonaMessage",
                columns: table => new
                {
                    mcpm_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mcp_id = table.Column<int>(type: "int", nullable: false),
                    mc_id = table.Column<int>(type: "int", nullable: false),
                    ts_id = table.Column<int>(type: "int", nullable: false),
                    mcpm_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reply_to_id = table.Column<long>(type: "bigint", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    edited_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessagingChatPersonaMessage", x => x.mcpm_id);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersonaMessage_TMessagingChatPersonaMessage_reply_to_id",
                        column: x => x.reply_to_id,
                        principalTable: "TMessagingChatPersonaMessage",
                        principalColumn: "mcpm_id");
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersonaMessage_TMessagingChatPersona_mcp_id",
                        column: x => x.mcp_id,
                        principalTable: "TMessagingChatPersona",
                        principalColumn: "mcp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersonaMessage_TMessagingChat_mc_id",
                        column: x => x.mc_id,
                        principalTable: "TMessagingChat",
                        principalColumn: "mc_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMessagingChatPersonaMessage_TTeamSeason_ts_id",
                        column: x => x.ts_id,
                        principalTable: "TTeamSeason",
                        principalColumn: "ts_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChat_created_by_mp_id",
                table: "TMessagingChat",
                column: "created_by_mp_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChat_ts_id",
                table: "TMessagingChat",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersona_mc_id",
                table: "TMessagingChatPersona",
                column: "mc_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersona_mp_id",
                table: "TMessagingChatPersona",
                column: "mp_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersona_ts_id",
                table: "TMessagingChatPersona",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersonaMessage_mc_id",
                table: "TMessagingChatPersonaMessage",
                column: "mc_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersonaMessage_mcp_id",
                table: "TMessagingChatPersonaMessage",
                column: "mcp_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersonaMessage_reply_to_id",
                table: "TMessagingChatPersonaMessage",
                column: "reply_to_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingChatPersonaMessage_ts_id",
                table: "TMessagingChatPersonaMessage",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingMedia_created_by_mp_id",
                table: "TMessagingMedia",
                column: "created_by_mp_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingMedia_mc_id",
                table: "TMessagingMedia",
                column: "mc_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingMedia_ts_id",
                table: "TMessagingMedia",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingPersona_mp_email",
                table: "TMessagingPersona",
                column: "mp_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TMessagingPersona_ts_id",
                table: "TMessagingPersona",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TNews_news_author_mp_id",
                table: "TNews",
                column: "news_author_mp_id");

            migrationBuilder.CreateIndex(
                name: "IX_TNews_ts_id",
                table: "TNews",
                column: "ts_id");

            migrationBuilder.CreateIndex(
                name: "IX_TPlayer_IdSaison",
                table: "TPlayer",
                column: "IdSaison");

            migrationBuilder.CreateIndex(
                name: "IX_TPlayer_PersonneMessageriemp_id",
                table: "TPlayer",
                column: "PersonneMessageriemp_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "TRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TRoleClaim_RoleId",
                table: "TRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TTeamManager_tm_email",
                table: "TTeamManager",
                column: "tm_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TTeamSeason_ts_chat_key",
                table: "TTeamSeason",
                column: "ts_chat_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TTeamSeason_TTeamManagertm_id",
                table: "TTeamSeason",
                column: "TTeamManagertm_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "TUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "TUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TUserClaim_UserId",
                table: "TUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TUserLogin_UserId",
                table: "TUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TUserRole_RoleId",
                table: "TUserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscription");

            migrationBuilder.DropTable(
                name: "TMessagingChatPersonaMessage");

            migrationBuilder.DropTable(
                name: "TMessagingMedia");

            migrationBuilder.DropTable(
                name: "TNews");

            migrationBuilder.DropTable(
                name: "TPlayer");

            migrationBuilder.DropTable(
                name: "TRoleClaim");

            migrationBuilder.DropTable(
                name: "TUserClaim");

            migrationBuilder.DropTable(
                name: "TUserLogin");

            migrationBuilder.DropTable(
                name: "TUserRole");

            migrationBuilder.DropTable(
                name: "TUserToken");

            migrationBuilder.DropTable(
                name: "TMessagingChatPersona");

            migrationBuilder.DropTable(
                name: "TRole");

            migrationBuilder.DropTable(
                name: "TUser");

            migrationBuilder.DropTable(
                name: "TMessagingChat");

            migrationBuilder.DropTable(
                name: "TMessagingPersona");

            migrationBuilder.DropTable(
                name: "TTeamSeason");

            migrationBuilder.DropTable(
                name: "TTeamManager");
        }
    }
}
