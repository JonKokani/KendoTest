using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
//using Kendo.Mvc.UI;

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
        List<tblData> data = _dataRepository.GetAllData();
        return Json(data);
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AddData()
    {
        ViewBag.course = _dataRepository.GetAllCourse();
        return View();
    }
    [HttpPost]
    public IActionResult AddData(tblData data)
    {
        _dataRepository.AddData(data);
        return Json(new { success = true, message = "Data Inserted successfully." });
    }

    public IActionResult UpdateData(int id)
    {
        ViewBag.course = _dataRepository.GetAllCourse();
        tblData data = _dataRepository.GetOneData(id);
        return View(data);
    }
    [HttpPost]
    public IActionResult UpdateData(tblData data)
    {
        ViewBag.course = _dataRepository.GetAllCourse();
        _dataRepository.UpdateData(data);
        return RedirectToAction("Index");

    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _dataRepository.DeleteData(id);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult MDelete(List<int> id)
    {
        foreach (var d in id)
        {
            _dataRepository.DeleteData(d);
        }
        return RedirectToAction("Index");
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
