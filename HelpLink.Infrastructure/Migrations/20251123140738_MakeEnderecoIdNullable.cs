using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeEnderecoIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instituicoes_Enderecos_EnderecoId",
                table: "Instituicoes");

            migrationBuilder.AlterColumn<int>(
                name: "EnderecoId",
                table: "Instituicoes",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddForeignKey(
                name: "FK_Instituicoes_Enderecos_EnderecoId",
                table: "Instituicoes",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instituicoes_Enderecos_EnderecoId",
                table: "Instituicoes");

            migrationBuilder.AlterColumn<int>(
                name: "EnderecoId",
                table: "Instituicoes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Instituicoes_Enderecos_EnderecoId",
                table: "Instituicoes",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
