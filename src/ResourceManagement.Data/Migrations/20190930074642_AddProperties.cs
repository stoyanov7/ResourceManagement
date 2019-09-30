using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResourceManagement.Data.Migrations
{
    public partial class AddProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ScheduleItem",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "RecurringSchedule",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecurringSchedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ScheduleItem");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RecurringSchedule");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RecurringSchedule");
        }
    }
}
