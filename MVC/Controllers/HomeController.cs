using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDataRepository _dataRepository;

    public HomeController(ILogger<HomeController> logger, IDataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
    }

    public IActionResult Data()
    {
        return Json(_dataRepository.GetAllData());
    }
    public IActionResult Index()
    {
        return View();
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
