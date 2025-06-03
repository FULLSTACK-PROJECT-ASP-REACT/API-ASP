using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiNetCore.Entities
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_category",
                columns: table => new
                {
                    id_cat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_cat = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    status_cat = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValue: "A"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_category_pk", x => x.id_cat);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    id_pro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code_pro = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    name_pro = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    description_pro = table.Column<string>(type: "text", nullable: true),
                    price_unit_pro = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    update_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    status_pro = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValue: "A"),
                    stock_pro = table.Column<int>(type: "int", nullable: false, defaultValueSql: "('10000')"),
                    image_pro = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_product_pk", x => x.id_pro)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "tbl_transaction",
                columns: table => new
                {
                    id_tra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    emission_date_tra = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    code_stub_tra = table.Column<string>(type: "varchar(50)", nullable: true),
                    type_tra = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    price_unit_tra = table.Column<int>(type: "int", nullable: false),
                    total_amount_tra = table.Column<int>(type: "int", nullable: false),
                    status_tra = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true, defaultValue: "A"),
                    message_tra = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_transaction_pk", x => x.id_tra);
                });

            migrationBuilder.CreateTable(
                name: "tbl_category_product",
                columns: table => new
                {
                    id_ca_pr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pro_id = table.Column<int>(type: "int", nullable: true),
                    cat_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_category_product_pk", x => x.id_ca_pr);
                    table.ForeignKey(
                        name: "tbl_category_product_tbl_category_id_cat_fk",
                        column: x => x.cat_id,
                        principalTable: "tbl_category",
                        principalColumn: "id_cat");
                    table.ForeignKey(
                        name: "tbl_category_product_tbl_product_id_pro_fk",
                        column: x => x.pro_id,
                        principalTable: "tbl_product",
                        principalColumn: "id_pro");
                });

            migrationBuilder.CreateTable(
                name: "tbl_detail_transaction",
                columns: table => new
                {
                    id_d_t = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pro_id = table.Column<int>(type: "int", nullable: true),
                    tra_id = table.Column<int>(type: "int", nullable: true),
                    code_stub = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    price_unit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    description_tra = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_detail_tran_pk", x => x.id_d_t);
                    table.ForeignKey(
                        name: "tbl_detail_tran_tbl_product_id_pro_fk",
                        column: x => x.pro_id,
                        principalTable: "tbl_product",
                        principalColumn: "id_pro");
                    table.ForeignKey(
                        name: "tbl_detail_tran_tbl_transaction_id_tra_fk",
                        column: x => x.tra_id,
                        principalTable: "tbl_transaction",
                        principalColumn: "id_tra");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_category_product_cat_id",
                table: "tbl_category_product",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_category_product_pro_id",
                table: "tbl_category_product",
                column: "pro_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_detail_transaction_pro_id",
                table: "tbl_detail_transaction",
                column: "pro_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_detail_transaction_tra_id",
                table: "tbl_detail_transaction",
                column: "tra_id");

            migrationBuilder.CreateIndex(
                name: "tbl_product_id_pro_index",
                table: "tbl_product",
                column: "id_pro")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_category_product");

            migrationBuilder.DropTable(
                name: "tbl_detail_transaction");

            migrationBuilder.DropTable(
                name: "tbl_category");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_transaction");
        }
    }
}
