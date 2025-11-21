using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithNetSuffix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CATEGORIES_NET",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    limite_mensal = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CATEGORIES_NET", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "TB_USERS_NET",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    senha_hash = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USERS_NET", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "TB_GOALS_NET",
                columns: table => new
                {
                    id_goal = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    id_user = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    titulo = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    tipo = table.Column<string>(type: "NVARCHAR2(12)", maxLength: 12, nullable: false),
                    valor_alvo = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    dias_alvo = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    dias_concluidos = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    qtd_alvo_diaria = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    unidade = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    data_inicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    data_fim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    status = table.Column<string>(type: "NVARCHAR2(12)", maxLength: 12, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GOALS_NET", x => x.id_goal);
                    table.ForeignKey(
                        name: "FK_TB_GOALS_NET_TB_USERS_NET_id_user",
                        column: x => x.id_user,
                        principalTable: "TB_USERS_NET",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_TRANSACTIONS_NET",
                columns: table => new
                {
                    id_transaction = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    id_user = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_category = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    id_goal = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    tipo = table.Column<string>(type: "NVARCHAR2(12)", maxLength: 12, nullable: false),
                    valor = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    descricao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    merchant = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    data_transacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TRANSACTIONS_NET", x => x.id_transaction);
                    table.ForeignKey(
                        name: "FK_TB_TRANSACTIONS_NET_TB_CATEGORIES_NET_id_category",
                        column: x => x.id_category,
                        principalTable: "TB_CATEGORIES_NET",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_TRANSACTIONS_NET_TB_GOALS_NET_id_goal",
                        column: x => x.id_goal,
                        principalTable: "TB_GOALS_NET",
                        principalColumn: "id_goal",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TB_TRANSACTIONS_NET_TB_USERS_NET_id_user",
                        column: x => x.id_user,
                        principalTable: "TB_USERS_NET",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CATEGORIES_NET_nome",
                table: "TB_CATEGORIES_NET",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_CATEGORIES_NET_tipo",
                table: "TB_CATEGORIES_NET",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GOALS_NET_id_user",
                table: "TB_GOALS_NET",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GOALS_NET_tipo",
                table: "TB_GOALS_NET",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSACTIONS_NET_id_category",
                table: "TB_TRANSACTIONS_NET",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSACTIONS_NET_id_goal",
                table: "TB_TRANSACTIONS_NET",
                column: "id_goal");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRANSACTIONS_NET_id_user",
                table: "TB_TRANSACTIONS_NET",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USERS_NET_created_at",
                table: "TB_USERS_NET",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USERS_NET_email",
                table: "TB_USERS_NET",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_TRANSACTIONS_NET");

            migrationBuilder.DropTable(
                name: "TB_CATEGORIES_NET");

            migrationBuilder.DropTable(
                name: "TB_GOALS_NET");

            migrationBuilder.DropTable(
                name: "TB_USERS_NET");
        }
    }
}
