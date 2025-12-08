using System.Collections.Generic;

namespace FamilyTree.Models.Common
{
    public class CommonSearchModel
    {
        public string? SearchValue { get; set; }
        public Dictionary<string, string>? FilterList { get; set; }
    }
}
