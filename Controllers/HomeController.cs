using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendGridNet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SendGridNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly SendGridRepository repository;
        private static readonly Mutex mut = new Mutex();
        private readonly Email email = new Email();
        private readonly Config config = new Config();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            repository = new SendGridRepository();
            config = repository.GetConfig();
        }

        public IActionResult Index()
        {
            return View();
        }

        private async void EnviarEmail(object o)
        {
            try
            {
                ObjetoGenerico resultado = (ObjetoGenerico)o;
                var html = resultado.html;

                var textoAsustituir = "{nombre},{codigo}";
                var htmlTemp = html;
                //var maximo = (resultado.rango - resultado.rango);
                var min = resultado.listPersona.Min(x => x.id) - 1;
                var max = min + (resultado.rango + 1);
                var resultSet = resultado.listPersona.Where(x => x.id > min && x.id < max);
                foreach (var data in resultSet)
                {
                    if (textoAsustituir != null && textoAsustituir != "")
                    {
                        var sustitutos = textoAsustituir.Split(',');

                        int key = 0;
                        foreach (string sustituto in sustitutos)
                        {
                            var valor = data.nombre;

                            if (key == 1)
                            {
                                valor = data.codigo;
                            }

                            htmlTemp = htmlTemp.Replace(sustituto, valor);
                            key += 1;
                        }
                    }

                    await email.SendEmail(data.correo, new List<string>(), "TU CÓDIGO DE PREINSCRIPCIÓN PARA LA REGULARIZACIÓN", htmlTemp,config);
                    htmlTemp = html;
                    repository.Actualizar(data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult refrescar() 
        {
            var resultado = repository.GetEstado();
            return Ok(resultado);
        }
        public void procesar() {
            try {
                string path = Constante.html;
                var data = new ObjetoGenerico
                {
                    html = System.IO.File.ReadAllText(path)
                };

                int rango = 1;
                repository.Asignar(rango, Util.GetMac());

                int nucleos = 1;

                data.listPersona = repository.Seleccionar(rango, Util.GetMac());

                var totalRegistros = data.listPersona.Count;
                data.rango = totalRegistros / nucleos;

                if (1 == 0)
                {
                    mut.WaitOne();
                    for (var i = 0; i < nucleos; i++)
                    {
                        if (data.listPersona == null || data.listPersona.Count == 0)
                        {
                            throw new Exception("No quedan notificaciones por enviar. Hilo " + i);
                        }

                        var hilo = new Thread(EnviarEmail);
                        hilo.Start(data);
                        data.rango += data.rango;
                    }

                    mut.ReleaseMutex();
                }
                else
                {
                    EnviarEmail(data);
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
