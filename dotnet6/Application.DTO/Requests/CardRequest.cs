﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Requests
{
    public class CardRequest
    {
        [Required, MinLength(6)]

        public string CustomerID { get; set; }

        [Required, MinLength(10)]

        public string UniqueIdentifier { get; set; }
    }
}
