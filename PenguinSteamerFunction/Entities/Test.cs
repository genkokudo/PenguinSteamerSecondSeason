using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerFunction.Entities
{
    public class Test
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
