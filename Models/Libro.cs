using System;
using System.Collections.Generic;

namespace APIDeBiblioteca.Models;

public partial class Libro
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public DateOnly? FechaPublicacion { get; set; }

    public string? Isbn { get; set; }

    public bool? Disponible { get; set; }

    public int? NumeroPaginas { get; set; }

    public string? Genero { get; set; }

    public string? Imagen { get; set; }
}
