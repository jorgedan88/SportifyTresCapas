using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportify_Back.Models;
using Sportify_Back.Services;

[Authorize(Policy = "AdministradorOnly")] // Solo accede el Admin
public class ReportController : Controller
{
    private readonly IEntityReportService _reportService;

    public ReportController(IEntityReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult Index()
    {
        return View();
    }

public IActionResult ClassesReport()
{
    var monthlyReports = _reportService.GenerateClassReport(); // Cambiado para usar el nuevo método

    return View("ReportView", monthlyReports); //le paso el modelo a la vista
}

[HttpGet]
public IActionResult GetTopClasses()
{
    var report = _reportService.GetClassesWithMostUsers();
    return View("ReportMoreConcurrence", report);
}


}
