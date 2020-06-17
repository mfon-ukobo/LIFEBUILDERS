using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LB.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsSeen = table.Column<bool>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAccesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Alt = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_AdminUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Responsibility = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.AppUserId);
                    table.ForeignKey(
                        name: "FK_Members_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    StartDate = table.Column<string>(nullable: false),
                    EndDate = table.Column<string>(nullable: true),
                    StartTime = table.Column<string>(nullable: false),
                    EndTime = table.Column<string>(nullable: true),
                    Venue = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    ImageId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Published = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_SiteImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrontPageCarousels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(maxLength: 35, nullable: true),
                    Details = table.Column<string>(nullable: true),
                    HasButton = table.Column<bool>(nullable: false),
                    ButtonText = table.Column<string>(nullable: true),
                    ButtonLink = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontPageCarousels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrontPageCarousels_SiteImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gallery_SiteImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Published = table.Column<int>(nullable: false),
                    AudioUrl = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_SiteImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    ResourceUrl = table.Column<string>(nullable: true),
                    IsLocalResource = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_ResourceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ResourceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resources_SiteImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdminTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    AdminUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminTasks_AdminUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AdminUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipMembers",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(nullable: false),
                    DiscipleshipGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipMembers", x => new { x.MemberId, x.DiscipleshipGroupId });
                    table.ForeignKey(
                        name: "FK_DiscipleshipMembers_DiscipleshipGroups_DiscipleshipGroupId",
                        column: x => x.DiscipleshipGroupId,
                        principalTable: "DiscipleshipGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscipleshipMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscipleshipRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscipleshipRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscipleshipRegistrations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MailListMember",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(nullable: false),
                    MailListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailListMember", x => new { x.MailListId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_MailListMember_MailLists_MailListId",
                        column: x => x.MailListId,
                        principalTable: "MailLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailListMember_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAccessMember",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(nullable: false),
                    ResourceAccessId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAccessMember", x => new { x.MemberId, x.ResourceAccessId });
                    table.ForeignKey(
                        name: "FK_ResourceAccessMember_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceAccessMember_ResourceAccesses_ResourceAccessId",
                        column: x => x.ResourceAccessId,
                        principalTable: "ResourceAccesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    LBEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_LBEventId",
                        column: x => x.LBEventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PostCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategory_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceAccessRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<Guid>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Finished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAccessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceAccessRequests_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceAccessRequests_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceDownloads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<Guid>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceDownloads_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceDownloads_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceToAccess",
                columns: table => new
                {
                    ResourceId = table.Column<int>(nullable: false),
                    ResourceAccessId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceToAccess", x => new { x.ResourceId, x.ResourceAccessId });
                    table.ForeignKey(
                        name: "FK_ResourceToAccess_ResourceAccesses_ResourceAccessId",
                        column: x => x.ResourceAccessId,
                        principalTable: "ResourceAccesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceToAccess_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    CommentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminTasks_AdminUserId",
                table: "AdminTasks",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipMembers_DiscipleshipGroupId",
                table: "DiscipleshipMembers",
                column: "DiscipleshipGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscipleshipRegistrations_MemberId",
                table: "DiscipleshipRegistrations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_LBEventId",
                table: "EventRegistrations",
                column: "LBEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ImageId",
                table: "Events",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Title",
                table: "Events",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FrontPageCarousels_ImageId",
                table: "FrontPageCarousels",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_ImageId",
                table: "Gallery",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailListMember_MemberId",
                table: "MailListMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CountryId",
                table: "Members",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ImageId",
                table: "Posts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                table: "Posts",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentId",
                table: "Replies",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAccesses_Name",
                table: "ResourceAccesses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAccessMember_ResourceAccessId",
                table: "ResourceAccessMember",
                column: "ResourceAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAccessRequests_MemberId",
                table: "ResourceAccessRequests",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAccessRequests_ResourceId",
                table: "ResourceAccessRequests",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceCategories_Name",
                table: "ResourceCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceDownloads_MemberId",
                table: "ResourceDownloads",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceDownloads_ResourceId",
                table: "ResourceDownloads",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CategoryId",
                table: "Resources",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ImageId",
                table: "Resources",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Title",
                table: "Resources",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceToAccess_ResourceAccessId",
                table: "ResourceToAccess",
                column: "ResourceAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminTasks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DiscipleshipMembers");

            migrationBuilder.DropTable(
                name: "DiscipleshipRegistrations");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "FrontPageCarousels");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "MailListMember");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "ResourceAccessMember");

            migrationBuilder.DropTable(
                name: "ResourceAccessRequests");

            migrationBuilder.DropTable(
                name: "ResourceDownloads");

            migrationBuilder.DropTable(
                name: "ResourceToAccess");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DiscipleshipGroups");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MailLists");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "ResourceAccesses");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ResourceCategories");

            migrationBuilder.DropTable(
                name: "SiteImages");
        }
    }
}
