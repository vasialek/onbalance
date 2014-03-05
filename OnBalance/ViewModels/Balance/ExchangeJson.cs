using System.Collections.Generic;

namespace OnBalance.ViewModels.Balance
{

    public class ExchangeJson
    {
        public string uid { get; set; }
        public string code { get; set; }
        public int pr { get; set; }
        public int posid { get; set; }
        public string name { get; set; }
        public Dictionary<string, string> sizes { get; set; }
    }

}