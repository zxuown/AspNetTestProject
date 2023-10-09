using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetTest.Migrations
{
    /// <inheritdoc />
    public partial class FillData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.InsertData(
		 table: "AboutMe", // Назва вашої таблиці
		 columns: new[] { "Name", "SecondName","Age" }, // Колонки, в які ви хочете вставити дані
		 values: new object[,]
		 {
				{ "Andriy", "Pawlowich", 16 },
				
			 // Додавайте інші записи
		 });

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
