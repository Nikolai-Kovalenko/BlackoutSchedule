using AutoMapper;
using BlackoutSchedule.Models;
using BlackoutSchedule.Models.Dto;
using BlackoutSchedule.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BlackoutSchedule.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly IScheduleRerository _scheduleRepo;
        public IMapper _mapper { get; set; }

        public SchedulesController(IScheduleRerository scheduleRepo, IMapper mapper)
        {
            _scheduleRepo = scheduleRepo;
            _mapper = mapper;
        }

        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var scheduleList = _scheduleRepo.GetAll(u => u.GroupId == id && u.DeleteTime == null);

            if (scheduleList == null || !scheduleList.Any())
            {
                return NotFound();
            }

            var stringBuilder = new StringBuilder();

            var timeRanges = scheduleList
                .Select(s => $"{s.StartTime:hh\\:mm}-{s.FinishTime:hh\\:mm}")
                .Aggregate((current, next) => $"{current}; {next}");

            stringBuilder.Append($" {timeRanges}");

            var result = stringBuilder.ToString();

            var dto = new UnitUpdateDto()
            {
                groupId = id,
                schedule = result
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UnitUpdateDto dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                TempData["Message"] = "Данные не валидны";
                return View("EditString", dto);
            }

            var timeRanges = dto.schedule.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(range => range.Trim())
                .ToList();

            var scheduleList = new List<Schedules>();

            foreach (var range in timeRanges)
            {
                // Разбиваем временной диапазон на StartTime и FinishTime
                var times = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (times.Length != 2)
                {
                    TempData["Message"] = "Неправильный формат временного диапазона.";
                    return View(dto);
                }

                if (!TimeSpan.TryParse(times[0].Trim(), out var startTime) ||
                    !TimeSpan.TryParse(times[1].Trim(), out var finishTime))
                {
                    TempData["Message"] = "Не удалось распарсить время.";
                    return View(dto);
                }

                if (startTime >= finishTime)
                {
                    TempData["Message"] = "Время начала не может быть больше или равно времени конца.";
                    return View(dto);
                }

                scheduleList.Add(new Schedules
                {
                    GroupId = dto.groupId,
                    StartTime = startTime,
                    FinishTime = finishTime
                });
            }

            // Обновляем записи в репозитории
            _scheduleRepo.UnitUpdate(scheduleList, dto.groupId);
            _scheduleRepo.Save();

            return RedirectToAction("Index", "Home");
        }
    }
}
