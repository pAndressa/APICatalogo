using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome,ImagemUrl) VALUES('Bebidas','https://just.com.br/upload/refrigerantes.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome,ImagemUrl) VALUES('Lanches','https://just.com.br/upload/refrigerantes.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome,ImagemUrl) VALUES('Sobremesas','https://just.com.br/upload/refrigerantes.jpg')");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES('Coca-Cola Diet','Refrigerante de Coca',5.45,'https://www.drogariaminasbrasil.com.br/media/catalog/product/1/8/18526.jpg',50,getdate(),(SELECT CategoriaId FROM Categorias where Nome = 'Bebidas'))");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES('Lanche de Presunto','Lanche de presunto com maionese',8.50,'https://www.drogariaminasbrasil.com.br/media/catalog/product/1/8/18526.jpg',10,getdate(),(SELECT CategoriaId FROM Categorias where Nome = 'Lanches'))");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES('Pudim 100g','Pudim de leite condensado 100g',6.75,'https://www.drogariaminasbrasil.com.br/media/catalog/product/1/8/18526.jpg',20,getdate(),(SELECT CategoriaId FROM Categorias where Nome = 'Sobremesas'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Categorias");
            mb.Sql("DELETE FROM Produtos");
        }
    }
}
