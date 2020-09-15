using System.ComponentModel.DataAnnotations;

namespace Penguinium.Entities
{
    public class Test
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
