﻿using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "ObtenerRoot")]
        public ActionResult<IEnumerable<DatoHATEOAS>> Get()
        {
            var datosHateoas = new List<DatoHATEOAS>();
            datosHateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new {}), 
                descripcion: "self", metodo: "GET"));

            return datosHateoas;
        }
    }
}
