using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Response
{
    public class CardResponse
    {
        public string CardNumber { get; set; }
        public string MaskedCard { get; set; }

        public string NameOnCard { get; set; }

    }
}
