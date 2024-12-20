﻿using System.ComponentModel.DataAnnotations;

namespace XRFID.Server.Demo.V2.ViewModels
{
    public class AddMultipleProductsModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{1,12}$", ErrorMessage = "Code prefix must be a valix HEX representation between 1 and 12 digits")]
        public int InitialCode { get; set; }

        [Required]
        [RegularExpression(@"^[0-9a-fA-F]{4}$", ErrorMessage = "EPC Prefix must be a valix HEX rapresentation of 4 charachers")]
        public string EpcPrefix { get; set; } = string.Empty;

        //[Required]
        //[Range(0, 99980000)]
        //public int StartIncremental { get; set; }

        [Required]
        [Range(2, 9999)]
        public int NProd { get; set; }

        [Required]
        public Guid SkuId { get; set; }
    }
}
