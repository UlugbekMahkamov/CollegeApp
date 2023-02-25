using CollageApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollageApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDtoClass>> GetStudents()
        {
            _logger.LogInformation("GetStudents method started");
            var students = CollageRepositoryClass.Students.Select(s => new StudentDtoClass()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Adress = s.Adress,
                Email = s.Email
            }).ToList();
            //OK - 200 - Success
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDtoClass> GetStudentById(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = CollageRepositoryClass.Students.Where(n => n.Id == id).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"The student with id {id} not found");
            }

            var studentDTO = new StudentDtoClass
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Adress = student.Adress
            };
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDtoClass> GetStudentByName(string name)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = CollageRepositoryClass.Students.Where(n => n.StudentName == name).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with name {name} not found");
            var studentDTO = new StudentDtoClass
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Adress = student.Adress
            };
            //OK - 200 - Success
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        //api/student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDtoClass> CreateStudent([FromBody] StudentDtoClass model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (model == null)
                return BadRequest();

            //if(model.AdmissionDate < DateTime.Now)
            //{
            //    //1. Directly adding error message to modelstate
            //    //2. Using custom attribute
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
            //    return BadRequest(ModelState);
            //}    

            int newId = CollageRepositoryClass.Students.LastOrDefault().Id + 1;
            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Adress = model.Adress,
                Email = model.Email
            };
            CollageRepositoryClass.Students.Add(student);

            model.Id = student.Id;
            //Status - 201
            //https://localhost:7185/api/Student/3
            //New student details
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDtoClass model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existingStudent = CollageRepositoryClass.Students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Adress = model.Adress;

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDtoClass> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existingStudent = CollageRepositoryClass.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            var studentDTO = new StudentDtoClass
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Adress = existingStudent.Adress
            };

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Adress = studentDTO.Adress;

            //204 - NoContent
            return NoContent();
        }


        [HttpDelete("Delete/{id}", Name = "DeleteStudentById")]
        //api/student/delete/1
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            //BadRequest - 400 - Badrequest - Client error
            if (id <= 0)
                return BadRequest();

            var student = CollageRepositoryClass.Students.Where(n => n.Id == id).FirstOrDefault();
            //NotFound - 404 - NotFound - Client error
            if (student == null)
                return NotFound($"The student with id {id} not found");

            CollageRepositoryClass.Students.Remove(student);

            //OK - 200 - Success
            return Ok(true);
        }
    }
}