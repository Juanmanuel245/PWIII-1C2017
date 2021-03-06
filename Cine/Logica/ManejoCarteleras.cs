﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace Logica
{
    public class ManejoCarteleras
    {

        PersistenciaCartelera pCartelera = new PersistenciaCartelera();

        public void GuardarCartelera(Carteleras c)
        {
            pCartelera.AlmacenarCartelera(c);
        }

        public bool ValidarCartelera(Carteleras c)
        {
            // Traigo todas las carteleras donde la sede, la sala, Pelicula y la version sean iguales
            List<Carteleras> carteleras = pCartelera.ObtenerCarteleraPorSedeSalaVersion(c.IdSede, c.NumeroSala, c.IdVersion, c.IdPelicula, c.IdCartelera);

            // Traigo todas las carteleras en donde coincida Sede, Sala y Rango de Fechas
            List<Carteleras> cartelerasSedeSalaFecha = pCartelera.ObtenerCartelerasSedeSalaFecha(c.IdSede, c.NumeroSala, c.FechaInicio, c.FechaFin, c.IdCartelera);

            // Verifico que no haya llegado vacia, si llego vacia es por que no hay salas con esas caracteristicas asi que es valida
            if (carteleras.Count() == 0 && cartelerasSedeSalaFecha.Count() == 0)
            {
                
                return true;
            }

            // Si tengo una cartelera en la misma Sede, Sala y Version verifico que no se crucen las fechas
            foreach (Carteleras cartelera in carteleras)
            {
                // Averiguo si la fecha de inicio se pisa con alguna cartelera
                if (c.FechaInicio >= cartelera.FechaInicio && c.FechaInicio <= cartelera.FechaFin)
                {
                    
                    return false;
                }

                // Averiguo si la fecha de fin se pisa con alguna cartelera
                if (c.FechaFin <= cartelera.FechaFin && c.FechaFin >= cartelera.FechaInicio)
                {
                    return false;
                }
            }

         // Ahora verifico si ninguna otra pelicula se pisa en horarios con la mia

            // Recorro cada cartelera y me fijo si se pisa algun dia

            foreach (Carteleras cartelera in cartelerasSedeSalaFecha)
            {
                // Verifico si mi nueva cartelera solapa algun dia de las viejas carteleras, si esto es verdadero devuelvo false
                if(SolapaDias(c, cartelera))
                {
                    return false;
                }
            }
            return true;
        }

        public bool SolapaDias(Carteleras nueva, Carteleras vieja)
        {
            // Si devuelve FALSE es por que no se solapan
            if (vieja.Lunes == true) { if (nueva.Lunes == true) { return true; } }
            if (vieja.Martes == true) { if (nueva.Martes == true) { return true; } }
            if (vieja.Miercoles == true) { if (nueva.Miercoles == true) { return true; } }
            if (vieja.Jueves == true) { if (nueva.Jueves == true) { return true; } }
            if (vieja.Viernes == true) { if (nueva.Viernes == true) { return true; } }
            if (vieja.Sabado == true) { if (nueva.Sabado == true) { return true; } }
            if (vieja.Domingo == true) { if (nueva.Domingo == true) { return true; } }

            return false;
        }

        public bool DiaValidoCartelera(Carteleras c)
        {
            // Chequeo si todos los dias estan falsos, es decir, que no fueron seleccionados
            if(c.Lunes == false && c.Martes == false && c.Miercoles == false && c.Jueves == false && c.Viernes == false && c.Sabado == false && c.Domingo == false)
            {
                return false;
            }

            return true;
        }
        
        public List<Carteleras> TraerCarteleras()
        {
            return pCartelera.ObtenerCarteleras();
        }

        public List<Carteleras> TraerCartelerasPorFecha(DateTime fecha)
        {
            return pCartelera.OBtenerCartelerasPorFecha(fecha);
        }

        public void BorrarCartelera(int id)
        {
            pCartelera.EliminarCartelera(id);

        }

        public Carteleras TraerCartelera(int id)
        {
            return pCartelera.ObtenerCartelera(id);
        }

        public bool ActualizarCartelera(Carteleras c)
        {
            
            
            if (ValidarCartelera(c))
            {
                pCartelera.ActualizarCartelera(c);
                return true;
            }
            return false;
        }


        public string FormatoDia(DateTime fecha)
        {
            if (fecha.Day.ToString().Length == 1)
                return fecha.Day.ToString("D2");

            return fecha.Day.ToString();
        }

        public string FormadoMes(DateTime fecha)
        {
            if(fecha.Month.ToString().Length == 1)
                return fecha.Month.ToString("D2");

            return fecha.Month.ToString();
        }

        public List<Carteleras> TraerCarteleraPorPeliculaYFecha(int id, DateTime limite, DateTime hoy)
        {

            return pCartelera.ObtenerCartelerasPorPeliculaFecha(id, limite, hoy);
        }

        public List<Carteleras> TraerCartelerasPorVersionYPelicula(int version, int pelicula)
        {
            return pCartelera.ObtenerCartelerasVersionPelicula(version, pelicula);
        }

    }
}
