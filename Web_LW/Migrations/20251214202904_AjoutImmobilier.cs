using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_LW.Migrations
{
    /// <inheritdoc />
    public partial class AjoutImmobilier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImmobilierAnnonce_Annonces_AnnonceId",
                table: "ImmobilierAnnonce");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImmobilierAnnonce",
                table: "ImmobilierAnnonce");

            migrationBuilder.RenameTable(
                name: "ImmobilierAnnonce",
                newName: "ImmobilierAnnonces");

            migrationBuilder.RenameIndex(
                name: "IX_ImmobilierAnnonce_AnnonceId",
                table: "ImmobilierAnnonces",
                newName: "IX_ImmobilierAnnonces_AnnonceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImmobilierAnnonces",
                table: "ImmobilierAnnonces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImmobilierAnnonces_Annonces_AnnonceId",
                table: "ImmobilierAnnonces",
                column: "AnnonceId",
                principalTable: "Annonces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImmobilierAnnonces_Annonces_AnnonceId",
                table: "ImmobilierAnnonces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImmobilierAnnonces",
                table: "ImmobilierAnnonces");

            migrationBuilder.RenameTable(
                name: "ImmobilierAnnonces",
                newName: "ImmobilierAnnonce");

            migrationBuilder.RenameIndex(
                name: "IX_ImmobilierAnnonces_AnnonceId",
                table: "ImmobilierAnnonce",
                newName: "IX_ImmobilierAnnonce_AnnonceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImmobilierAnnonce",
                table: "ImmobilierAnnonce",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImmobilierAnnonce_Annonces_AnnonceId",
                table: "ImmobilierAnnonce",
                column: "AnnonceId",
                principalTable: "Annonces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
