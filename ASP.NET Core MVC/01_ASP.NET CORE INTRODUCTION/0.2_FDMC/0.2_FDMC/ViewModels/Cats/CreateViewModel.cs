using System.ComponentModel.DataAnnotations;

namespace _0._2_FDMC.ViewModels.Cats
{
    public class CreateViewModel
    {
        public class CatCreateViewModel
        {
            [Required]
            [MinLength(2)]
            public string Name { get; set; }

            public string Breeds { get; set; }

            [Required]
            [Range(0, 20)]
            public int Age { get; set; }

            [Required]
            [DataType(DataType.ImageUrl)]
            public string ImageUrl { get; set; }
        }
    }
}
