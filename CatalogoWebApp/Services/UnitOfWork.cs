﻿using CatalogoWebApp.Models;
using Microsoft.Extensions.Options;
using Autor = CatalogoWebApp.Models.NoSQL.Autor;
using Carrera = CatalogoWebApp.Models.NoSQL.Carrera;
using Facultad = CatalogoWebApp.Models.NoSQL.Facultad;
using Tipo = CatalogoWebApp.Models.NoSQL.Tipo;
using TrabajoDeGraduacion = CatalogoWebApp.Models.NoSQL.TrabajoDeGraduacion;

namespace CatalogoWebApp.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IStoreServices<Autor> Autores { get; }
        public IStoreServices<TrabajoDeGraduacion> TrabajosDeGraduacion { get; }
        public IStoreServices<Tipo> Tipos { get; }
        public IStoreServices<Carrera> Carreras { get; }
        public IStoreServices<Facultad> Facultades { get; }

        public UnitOfWork(IOptions<StoreDatabaseSettings> settings)
        {
            Autores = new StoreService<Autor>(settings);
            TrabajosDeGraduacion = new StoreService<TrabajoDeGraduacion>(settings);
            Tipos = new StoreService<Tipo>(settings);
            Carreras = new StoreService<Carrera>(settings);
            Facultades = new StoreService<Facultad>(settings);
        }
    }
}
