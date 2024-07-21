using AutoMapper;
using BlackoutSchedule.Models.Dto;
using BlackoutSchedule.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Update(int groupId)
        {
            var scheduleList = _scheduleRepo.GetAll(u => u.GroupId == groupId && u.DeleteTime == null);

            var scheduleDto = _mapper.Map<ScheduleDto>(scheduleList);

            return View(scheduleDto);
        }
    }
}
