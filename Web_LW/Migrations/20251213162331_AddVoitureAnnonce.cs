using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_LW.Migrations
{
    /// <inheritdoc />
    public partial class AddVoitureAnnonce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Prix",
                table: "Annonces",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ImmobilierAnnonce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeBien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeTransaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surface = table.Column<double>(type: "float", nullable: false),
                    NombreChambres = table.Column<int>(type: "int", nullable: false),
                    NombreSallesBain = table.Column<int>(type: "int", nullable: false),
                    Etage = table.Column<int>(type: "int", nullable: false),
                    NombreEtages = table.Column<int>(type: "int", nullable: false),
                    Ascenseur = table.Column<bool>(type: "bit", nullable: false),
                    Parking = table.Column<bool>(type: "bit", nullable: false),
                    Jardin = table.Column<bool>(type: "bit", nullable: false),
                    Balcon = table.Column<bool>(type: "bit", nullable: false),
                    Meuble = table.Column<bool>(type: "bit", nullable: false),
                    AnneeConstruction = table.Column<int>(type: "int", nullable: false),
                    Orientation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EtatBien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoyerMensuel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Caution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AnnonceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmobilierAnnonce", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImmobilierAnnonce_Annonces_AnnonceId",
                        column: x => x.AnnonceId,
                        principalTable: "Annonces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoitureAnnonces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    Kilometrage = table.Column<int>(type: "int", nullable: false),
                    Carburant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoiteVitesse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Couleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puissance = table.Column<int>(type: "int", nullable: false),
                    NombrePortes = table.Column<int>(type: "int", nullable: false),
                    Climatisation = table.Column<bool>(type: "bit", nullable: false),
                    GPS = table.Column<bool>(type: "bit", nullable: false),
                    CameraRecul = table.Column<bool>(type: "bit", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    ABS = table.Column<bool>(type: "bit", nullable: false),
                    Airbags = table.Column<bool>(type: "bit", nullable: false),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnonceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoitureAnnonces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoitureAnnonces_Annonces_AnnonceId",
                        column: x => x.AnnonceId,
                        principalTable: "Annonces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImmobilierAnnonce_AnnonceId",
                table: "ImmobilierAnnonce",
                column: "AnnonceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoitureAnnonces_AnnonceId",
                table: "VoitureAnnonces",
                column: "AnnonceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImmobilierAnnonce");

            migrationBuilder.DropTable(
                name: "VoitureAnnonces");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prix",
                table: "Annonces",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
