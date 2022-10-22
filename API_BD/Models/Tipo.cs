﻿using System;
using System.Collections.Generic;

namespace API_BD.Models
{
    public partial class Tipo
    {
        public Tipo()
        {
            Contactos = new HashSet<Contacto>();
        }

        public int IdTipo { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Contacto> Contactos { get; set; }
    }
}
