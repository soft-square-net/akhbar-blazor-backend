using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.ElsaStore
{
    /// <inheritdoc />
    public partial class InitializeElsaStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "elsastore");

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                schema: "elsastore",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false),
                    Owner = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Roles = table.Column<List<string>>(type: "text[]", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys",
                schema: "elsastore");
        }
    }
}
