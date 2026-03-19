using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletCorp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBenefitPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BenefitPlanId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BenefitPlanId",
                table: "Users",
                column: "BenefitPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BenefitPlans_BenefitPlanId",
                table: "Users",
                column: "BenefitPlanId",
                principalTable: "BenefitPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_BenefitPlans_BenefitPlanId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BenefitPlanId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BenefitPlanId",
                table: "Users");
        }
    }
}
