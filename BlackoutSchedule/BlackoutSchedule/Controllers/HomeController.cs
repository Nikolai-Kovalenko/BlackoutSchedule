using BlackoutSchedule.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BlackoutSchedule.Data;
using BlackoutSchedule.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BlackoutSchedule.Repository.IRepository;
using Newtonsoft.Json;



namespace BlackoutSchedule.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        private readonly IGroupsReporitory _groupRepo;
        private readonly IScheduleRerository _scheduleRepo;
        

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext db,
            IGroupsReporitory groupRepo,
            IScheduleRerository scheduleRepo)
        {
            _logger = logger;
            _db = db;
            _groupRepo = groupRepo;
            _scheduleRepo = scheduleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _db.Groups.ToListAsync();
            var groupSchedules = groups.Select(group => new HomeVM
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Schedules = _db.Schedules
                    .Where(schedule => schedule.GroupId == group.Id && schedule.DeleteTime == null)
                    .ToList(),
                Addresses = _db.Addresses
                    .Where(address => address.GroupId == group.Id)
                    .Select(address => address.Address)
                    .ToList()
            }).ToList();

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
            }

            return View(groupSchedules);
        }

        [HttpPost]
        public async Task<IActionResult> ImportFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["Message"] = "Файл не выбран.";
                return RedirectToAction(nameof(Index));
            }

            if (file.Length < 1)
            {
                TempData["Message"] = "Файл пуст";
                return RedirectToAction(nameof(Index));
            }

            var schedules = new List<Schedules>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string content = await reader.ReadToEndAsync();
                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    var parts = line.Split('.');
                    if (parts.Length != 2)
                    {
                        TempData["Message"] = "Неправильный формат данных.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (!int.TryParse(parts[0], out var groupId))
                    {
                        TempData["Message"] = "Неправильный формат данных.";
                        return RedirectToAction(nameof(Index));
                    }

                    var groupDbContext = _groupRepo.Find(groupId);
                    if (groupDbContext == null)
                    {
                        TempData["Message"] = $"Неправильный формат данных. Группы {groupId} не найденно";
                        return RedirectToAction(nameof(Index));
                    }

                    var timeParts = parts[1].Split(';');
                    foreach (var timePart in timeParts)
                    {
                        var timeRange = timePart.Trim().Split('-');
                        if (timeRange.Length != 2 || !TimeSpan.TryParse(timeRange[0], out var startTime) || !TimeSpan.TryParse(timeRange[1], out var endTime))
                        {
                            TempData["Message"] = "Неправильный формат данных.";
                            return RedirectToAction(nameof(Index));
                        }

                        schedules.Add(new Schedules
                        {
                            GroupId = groupId,
                            StartTime = startTime,
                            FinishTime = endTime,
                        });
                    }
                }

            }

            _scheduleRepo.Update(schedules);
            _scheduleRepo.Save();

            TempData["Message"] = "Импорт данных успешно завершен.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportData(int? id)
        {
            var schedulesQuery = _db.Schedules.AsQueryable();

            if (id.HasValue)
            {
                schedulesQuery = schedulesQuery.Where(u => u.GroupId == id.Value);
            }

            var schedules = await schedulesQuery
                .Where(u => u.DeleteTime ==null)
                .Select(u => new
                {
                    u.GroupId,
                    u.StartTime,
                    u.FinishTime
                })
                .ToListAsync();

            var json = JsonConvert.SerializeObject(schedules, Formatting.Indented);

            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", "schedules.json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}