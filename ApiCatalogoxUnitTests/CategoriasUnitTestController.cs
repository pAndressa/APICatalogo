using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=ANDRESSA\\SQLEXPRESS;Database=CatalogoDb;Integrated Security=True;";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
        }
        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile(new MappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            repository = new UnitOfWork(context);
        }

        //testes unitários
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }

        //get -id
        [Fact]
        public void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var catId = 201;

            //Act
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<NotFoundResult>(data.Result);
        }
    }
}
