using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservsoft.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartamentos",
                columns: table => new
                {
                    IdApartamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoApartamento = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Tamaño = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Piso = table.Column<int>(type: "int", nullable: false),
                    Tarifa = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartamentos", x => x.IdApartamento);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    NumReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDoc = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    NumDoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acompanantes = table.Column<int>(type: "int", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalReserva = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.NumReserva);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Descuentos",
                columns: table => new
                {
                    IdDescuento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdApartamento = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuentos = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descuentos", x => x.IdDescuento);
                    table.ForeignKey(
                        name: "FK_Descuentos_Apartamentos_IdApartamento",
                        column: x => x.IdApartamento,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApartamento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mobiliarios",
                columns: table => new
                {
                    IdMobiliario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdApartamento = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdentMobiliario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mobiliarios", x => x.IdMobiliario);
                    table.ForeignKey(
                        name: "FK_Mobiliarios_Apartamentos_IdApartamento",
                        column: x => x.IdApartamento,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApartamento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservasApartamentos",
                columns: table => new
                {
                    ApartamentosIdApartamento = table.Column<int>(type: "int", nullable: false),
                    ReservaNumReserva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasApartamentos", x => new { x.ApartamentosIdApartamento, x.ReservaNumReserva });
                    table.ForeignKey(
                        name: "FK_ReservasApartamentos_Apartamentos_ApartamentosIdApartamento",
                        column: x => x.ApartamentosIdApartamento,
                        principalTable: "Apartamentos",
                        principalColumn: "IdApartamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservasApartamentos_Reservas_ReservaNumReserva",
                        column: x => x.ReservaNumReserva,
                        principalTable: "Reservas",
                        principalColumn: "NumReserva",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Descuentos_IdApartamento",
                table: "Descuentos",
                column: "IdApartamento");

            migrationBuilder.CreateIndex(
                name: "IX_IdentMobiliario_Unique",
                table: "Mobiliarios",
                column: "IdentMobiliario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mobiliarios_IdApartamento",
                table: "Mobiliarios",
                column: "IdApartamento");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasApartamentos_ReservaNumReserva",
                table: "ReservasApartamentos",
                column: "ReservaNumReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Descuentos");

            migrationBuilder.DropTable(
                name: "Mobiliarios");

            migrationBuilder.DropTable(
                name: "ReservasApartamentos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Apartamentos");

            migrationBuilder.DropTable(
                name: "Reservas");
        }
    }
}
