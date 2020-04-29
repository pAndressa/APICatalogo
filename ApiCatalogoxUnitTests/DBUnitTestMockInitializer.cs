using APICatalogo.Context;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiCatalogoxUnitTests
{
    public class DBUnitTestMockInitializer
    {
        public DBUnitTestMockInitializer()
        {

        }

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
                (new Categoria { CategoriaId = 8 , Nome = "Doces", ImagemUrl = "doces.jpg"});

            context.Categorias.Add
                (new Categoria { CategoriaId = 9, Nome = "Salgados", ImagemUrl = "salgados.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 10, Nome = "Sucos", ImagemUrl = "sucos.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 11, Nome = "Lanches", ImagemUrl = "lanches.jpg" });

            context.SaveChanges();
        }
    }
}
