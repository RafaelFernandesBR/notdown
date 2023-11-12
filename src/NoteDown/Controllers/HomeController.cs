﻿using Microsoft.AspNetCore.Mvc;
using NoteDown.Data.IModels;
using NoteDown.Data.Models;
using NoteDown.Models;
using System.Diagnostics;

namespace NoteDown.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataModel _dataModel;

        public HomeController(ILogger<HomeController> logger, IDataModel dataModel)
        {
            _logger = logger;
            _dataModel = dataModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("nots/{id}")]
        public IActionResult GeteNots(string id)
        {
            var nota = _dataModel.GetOne(id);

            if (nota == null)
            {
                _logger.LogInformation($"Nem uma nota encontrada para o ID {id}");

                return NotFound();
            }

            ViewBag.Nota = nota;
            return View();
        }

        [HttpPatch]
        [Route("update/{id}")]
        public IActionResult UpdateNots(string id, [FromBody] string nota)
        {
            try
            {
                var updatedNota = _dataModel.UpdateNots(id, nota);
                return Json(updatedNota);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { error = "Ocorreu um erro ao atualizar a nota." });
            }
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("save")]
        public IActionResult SaveNots(string nota)
        {
            var notInclud = new NotsInput();
            notInclud.Conteudo = nota;

            try
            {
                var notaIncluida = _dataModel.AddNots(notInclud);

                // Redirecione para a action GeteNots com o ID da nota recém-incluída
                return RedirectToAction("GeteNots", new { id = notaIncluida.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                // Lide com erros, por exemplo, exibindo uma mensagem de erro
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar a nota.";
                return View("Error"); // Pode criar uma view específica para erros
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 50, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
