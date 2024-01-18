using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiClass.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    DisciplineId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.DisciplineId);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GradeNumber = table.Column<int>(type: "integer", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTypes",
                columns: table => new
                {
                    InstitutionTypeId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTypes", x => x.InstitutionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.InstitutionId);
                    table.ForeignKey(
                        name: "FK_Institutions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId");
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTypesInstitutions",
                columns: table => new
                {
                    InstitutionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTypesInstitutions", x => new { x.InstitutionTypeId, x.InstitutionId });
                    table.ForeignKey(
                        name: "FK_InstitutionTypesInstitutions_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "InstitutionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionTypesInstitutions_InstitutionTypes_InstitutionTy~",
                        column: x => x.InstitutionTypeId,
                        principalTable: "InstitutionTypes",
                        principalColumn: "InstitutionTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "text", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordResetCode = table.Column<string>(type: "text", nullable: true),
                    VerificationCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IsCreatedAccount = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsInstitutionVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FirstName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    LastName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IsATeacher = table.Column<bool>(type: "boolean", nullable: true),
                    IsAnExpert = table.Column<bool>(type: "boolean", nullable: true),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CityId = table.Column<Guid>(type: "uuid", nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Rating = table.Column<double>(type: "numeric(3,2)", nullable: false, defaultValue: 0.0),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PhotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BannerPhotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastOnlineAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "InstitutionId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    GradeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDisciplines",
                columns: table => new
                {
                    DisciplineId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDisciplines", x => new { x.UserId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_UserDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "DisciplineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDisciplines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGrades",
                columns: table => new
                {
                    GradeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGrades", x => new { x.UserId, x.GradeId });
                    table.ForeignKey(
                        name: "FK_UserGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGrades_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguages",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguages", x => new { x.UserId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_UserLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassDisciplines",
                columns: table => new
                {
                    DisciplineId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassDisciplines", x => new { x.ClassId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_ClassDisciplines_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "DisciplineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLanguages",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLanguages", x => new { x.LanguageId, x.ClassId });
                    table.ForeignKey(
                        name: "FK_ClassLanguages_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    InvitationId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserSenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassSenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateOfInvitation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Pending"),
                    InvitationText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_Invitations_Classes_ClassReceiverId",
                        column: x => x.ClassReceiverId,
                        principalTable: "Classes",
                        principalColumn: "ClassId");
                    table.ForeignKey(
                        name: "FK_Invitations_Classes_ClassSenderId",
                        column: x => x.ClassSenderId,
                        principalTable: "Classes",
                        principalColumn: "ClassId");
                    table.ForeignKey(
                        name: "FK_Invitations_Users_UserReceiverId",
                        column: x => x.UserReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Invitations_Users_UserSenderId",
                        column: x => x.UserSenderId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    InvitationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserSenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserRecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    WasTheJointLesson = table.Column<bool>(type: "boolean", nullable: false),
                    ReasonForNotConducting = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReviewText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<short>(type: "SMALLINT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Invitations_InvitationId",
                        column: x => x.InvitationId,
                        principalTable: "Invitations",
                        principalColumn: "InvitationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserRecipientId",
                        column: x => x.UserRecipientId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserSenderId",
                        column: x => x.UserSenderId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Disciplines",
                columns: new[] { "DisciplineId", "Title" },
                values: new object[,]
                {
                    { new Guid("09cbff99-719b-4d7b-a19f-797ccea6b2d8"), "Social science" },
                    { new Guid("0ef1a191-9611-4544-82b6-b233bc991a05"), "World art" },
                    { new Guid("1a40572f-91f4-4f11-87c3-f922a247742d"), "German as a foreign language" },
                    { new Guid("337abea7-1415-4937-b311-2278125ba3ae"), "Crafts" },
                    { new Guid("3cdb9e49-2924-40ae-a04e-b67f2dc9b449"), "Chinese as a foreign language" },
                    { new Guid("4018cae9-a812-45dc-867e-f4a645d8c03b"), "Russian literature" },
                    { new Guid("4053a10b-304a-4c65-a582-81a3389aa790"), "Vacation education" },
                    { new Guid("480b561c-5ded-4359-a3c8-28231c9cfc4e"), "French as a foreign language" },
                    { new Guid("4c55afb4-d23a-43f2-8c28-32848f5a58d4"), "Russian language" },
                    { new Guid("4dc6942c-6ea1-468b-8f31-f5a97717d7a6"), "Computer science" },
                    { new Guid("569c0e0a-68fd-4081-9e15-9eeeb20ceb1d"), "Italian as a foreign language" },
                    { new Guid("56a1fdcc-19f1-46f5-92dc-c72e549da754"), "Spanish as a foreign language" },
                    { new Guid("5c93bf57-725a-4191-adc6-117a0ab4da91"), "Music" },
                    { new Guid("5f1c300a-aeef-4e2a-8276-e1c22e668477"), "Physics" },
                    { new Guid("60b51b03-7e21-4f0f-9d98-88b5042c31d6"), "Project-based learning" },
                    { new Guid("6fb8b96d-5ef4-4771-a346-f2cdde905a44"), "Chemistry" },
                    { new Guid("75aef381-c80b-414a-9b37-0396621118bf"), "Technology" },
                    { new Guid("7bec3001-7978-4fd5-ae57-2c88829c1b92"), "Cultural exchange" },
                    { new Guid("94e4774e-bbb4-441c-94a4-6f1b55b701b6"), "Mathematics" },
                    { new Guid("9557f2f1-2c5e-467c-b2e0-9e62bff34991"), "Natural science" },
                    { new Guid("a1251358-4375-4184-ac17-fb7746221997"), "Biology" },
                    { new Guid("b3a9b8f9-b457-4476-b3f1-011e47e0bc2c"), "Fine arts" },
                    { new Guid("bdb9c610-cd2f-4f66-802a-3a35e7c4b8b8"), "Geography" },
                    { new Guid("c517fcb8-0eb4-43c0-a932-d388d01aec2d"), "Economics" },
                    { new Guid("c9204aaa-2f32-41eb-8399-b3d46add9247"), "History" },
                    { new Guid("dfd06a69-2519-45db-adaa-cc70363c7ee6"), "Astronomy" },
                    { new Guid("e10da506-728f-495d-899d-89d765812343"), "Regional studies" },
                    { new Guid("f0434994-eefc-4632-a5d3-cb36cd87124b"), "English as a foreign language" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeId", "GradeNumber" },
                values: new object[,]
                {
                    { new Guid("00c66b7a-06ab-4477-b20e-571702267ceb"), 12 },
                    { new Guid("2255f732-4a11-4e5c-bc3e-7ceda27bacd9"), 3 },
                    { new Guid("2bbb6c3e-2c3f-4009-bd9b-0f1e43ce33ba"), 10 },
                    { new Guid("64dbd017-ebbe-4069-b92e-388b24097686"), 5 },
                    { new Guid("86cbe4a1-89e5-426f-b0bb-a4497a1415cb"), 2 },
                    { new Guid("9db15e60-2d4b-4610-ab50-f33541e1c3c6"), 8 },
                    { new Guid("9e452ed0-9079-443d-91c5-6d80164add18"), 1 },
                    { new Guid("b050ea7b-057f-4b91-a1a9-bb184d36233d"), 9 },
                    { new Guid("b8047931-8913-4536-969c-749bb203af1b"), 11 },
                    { new Guid("c9a03837-8057-4c84-9783-4038cee41300"), 7 },
                    { new Guid("d724b1ac-9486-49c8-af12-0c9af10d7d2f"), 4 },
                    { new Guid("f965bdd8-fc54-4095-9d3f-0ac3c61d29ea"), 6 }
                });

            migrationBuilder.InsertData(
                table: "InstitutionTypes",
                columns: new[] { "InstitutionTypeId", "Title" },
                values: new object[,]
                {
                    { new Guid("284651e0-bd8f-46a9-b7fb-07aff1a232a5"), "College" },
                    { new Guid("5a167097-699e-48f3-97db-d6f1ecbb3b90"), "School" },
                    { new Guid("68741f87-137f-4d6c-8d40-1b8effcef242"), "Gymnasium" },
                    { new Guid("f85a7898-d0ec-4bef-925d-76fca251e837"), "Lyceum" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageId", "Title" },
                values: new object[,]
                {
                    { new Guid("010384ec-2c1a-4608-b042-966e197d788c"), "Uzbek" },
                    { new Guid("27f18704-5290-4e88-8f33-af958db279d2"), "English" },
                    { new Guid("29852c01-7915-41f5-8ea3-7ea51ffe674a"), "Hungarian" },
                    { new Guid("2d46efe6-c1a2-4703-a565-ad056c934141"), "Spanish" },
                    { new Guid("2dd6d00a-ae9b-4ffc-b802-9d0aba33e427"), "Georgian" },
                    { new Guid("430dfa30-4a14-421a-82a3-2ea29c220b58"), "Portuguese" },
                    { new Guid("49736615-80f7-423a-a5be-7002ea65df30"), "Tajik" },
                    { new Guid("5cdbd342-11ad-4795-8a8f-b5b4d3673f33"), "Kazakh" },
                    { new Guid("6b7cfb17-e862-43f0-b2d0-1cae3470cd49"), "Armenian" },
                    { new Guid("835217ba-20ac-4514-a92a-2e60ffd91ad2"), "Kyrgyz" },
                    { new Guid("8fe73360-2344-4adc-86fe-f706e1c555fe"), "Belarusian" },
                    { new Guid("a374806f-06e1-4768-9008-d434217a8ec1"), "Ukrainian" },
                    { new Guid("bb7b3a2d-25a8-4ce4-9210-7c4a5d2056b1"), "German" },
                    { new Guid("cfdd2b5c-51cb-4a65-b371-a9a6a4f4c606"), "Russian" },
                    { new Guid("d32c0860-7f11-478d-8123-a8e7e1386f22"), "Azerbaijani" },
                    { new Guid("e0b5736f-0f1e-41f6-82e4-defa24f9c0fd"), "French" },
                    { new Guid("eec0d474-0f9b-44b5-98bb-fdb16907fd29"), "Italian" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Title" },
                values: new object[,]
                {
                    { new Guid("6b2560af-66f9-42bd-a4c2-c943cd62935e"), "Manager" },
                    { new Guid("902dc8f3-0a7d-4dd2-9a02-abc1a4bfb21f"), "User" },
                    { new Guid("a4e32536-215d-4075-a04b-97421ff50782"), "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityId",
                table: "Cities",
                column: "CityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassDisciplines_ClassId_DisciplineId",
                table: "ClassDisciplines",
                columns: new[] { "ClassId", "DisciplineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassDisciplines_DisciplineId",
                table: "ClassDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassId",
                table: "Classes",
                column: "ClassId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GradeId",
                table: "Classes",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UserId",
                table: "Classes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLanguages_ClassId",
                table: "ClassLanguages",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLanguages_LanguageId_ClassId",
                table: "ClassLanguages",
                columns: new[] { "LanguageId", "ClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryId",
                table: "Countries",
                column: "CountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_DisciplineId",
                table: "Disciplines",
                column: "DisciplineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeId",
                table: "Grades",
                column: "GradeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_CityId",
                table: "Institutions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_InstitutionId",
                table: "Institutions",
                column: "InstitutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTypes_InstitutionTypeId",
                table: "InstitutionTypes",
                column: "InstitutionTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTypesInstitutions_InstitutionId",
                table: "InstitutionTypesInstitutions",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTypesInstitutions_InstitutionTypeId_InstitutionId",
                table: "InstitutionTypesInstitutions",
                columns: new[] { "InstitutionTypeId", "InstitutionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ClassReceiverId",
                table: "Invitations",
                column: "ClassReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ClassSenderId",
                table: "Invitations",
                column: "ClassSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitationId",
                table: "Invitations",
                column: "InvitationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserReceiverId",
                table: "Invitations",
                column: "UserReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserSenderId",
                table: "Invitations",
                column: "UserSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageId",
                table: "Languages",
                column: "LanguageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_InvitationId",
                table: "Reviews",
                column: "InvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewId",
                table: "Reviews",
                column: "ReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserRecipientId",
                table: "Reviews",
                column: "UserRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserSenderId",
                table: "Reviews",
                column: "UserSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleId",
                table: "Roles",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDisciplines_DisciplineId",
                table: "UserDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDisciplines_UserId_DisciplineId",
                table: "UserDisciplines",
                columns: new[] { "UserId", "DisciplineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGrades_GradeId",
                table: "UserGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGrades_UserId_GradeId",
                table: "UserGrades",
                columns: new[] { "UserId", "GradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_LanguageId",
                table: "UserLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_UserId_LanguageId",
                table: "UserLanguages",
                columns: new[] { "UserId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_InstitutionId",
                table: "Users",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassDisciplines");

            migrationBuilder.DropTable(
                name: "ClassLanguages");

            migrationBuilder.DropTable(
                name: "InstitutionTypesInstitutions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "UserDisciplines");

            migrationBuilder.DropTable(
                name: "UserGrades");

            migrationBuilder.DropTable(
                name: "UserLanguages");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "InstitutionTypes");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
