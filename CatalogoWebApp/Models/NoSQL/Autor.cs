﻿
namespace CatalogoWebApp.Models.NoSQL
{
    public class Autor : Documento
    {
        private string _nombres;
        private string _apellidos;
        public string Codigo { get; set; }
        public string Nombres
        {
            get => _nombres?.Trim();
            set => _nombres = value;
        }
        public string Apellidos
        {
            get => _apellidos?.Trim();
            set => _apellidos = value;
        }
        public string NombreCompleto => Nombres + " " + Apellidos;
        public string CarreraCodigo { get; set; }
        public Carrera Carrera { get; set; }
    }
}
