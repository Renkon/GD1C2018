﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Habitacion
    {
        public int? Id { get; private set; }
        public Hotel Hotel { get; set; }
        public int Número { get; set; }
        public int Piso { get; set; }
        public string Ubicación { get; set; }
        public TipoHabitacion TipoHabitación { get; set; }
        public string Descripción { get; set; }

        public Habitacion(int? Id, Hotel Hotel, int Número, int Piso,
            string Ubicación, TipoHabitacion TipoHabitación, string Descripción)
        {
            this.Id = Id;
            this.Hotel = Hotel;
            this.Número = Número;
            this.Piso = Piso;
            this.Ubicación = Ubicación;
            this.TipoHabitación = TipoHabitación;
            this.Descripción = Descripción;
        }

        public Habitacion(int? Id, int Número)
        {
            this.Id = Id;
            this.Número = Número;
        }

        public Habitacion(int? Id)
        {
            this.Id = Id;
        }

        public override string ToString()
        {
            if (Id == -1)
                return " - Seleccione una habitación - ";
            return "Habitación número " + this.Número + ". Piso: " + this.Piso;
        }

        public override bool Equals(object obj)
        {
            var hab = obj as Habitacion;
            if (hab == null)
                return false;

            return hab.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
