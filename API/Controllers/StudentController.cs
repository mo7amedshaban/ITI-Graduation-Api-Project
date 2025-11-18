using Microsoft.AspNetCore.Mvc;
using Application.Common.Exceptions;
using Core.Entities;
using Core.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            throw new Exception("Global exception occurred");
          
        }
        

    }
}
