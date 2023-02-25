using CollageApp.Models;

namespace CollageApp.Models
{
    public class CollageRepositoryClass
    {
        public static List<Student> Students { get; set; } = new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    StudentName = "jahongir",
                    Email = "Jahongir@gmail.com",
                    Adress = "Fergana, Kokand"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "sama",
                    Email = "Sama@gmail.com",
                    Adress = "Fergana, Kokand"
                },
                new Student
                {
                    Id = 3,
                    StudentName = "eshmat",
                    Email = "Eshmat@gmail.com",
                    Adress = "Fergana, Kokand"
                }
            };
    }
}
