namespace AppLikeSteam
{
    public class GameInformation
    {
        public int Appid { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Score_rank { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Userscore { get; set; }
        public string Owners { get; set; }
        public int Average_forever { get; set; }
        public int Average_2weeks { get; set; }
        public int Median_forever { get; set; }
        public int Median_2weeks { get; set; }
        public string Price { get; set; }
        public string Initialprice { get; set; }
        public string Discount { get; set; }
        public string Ianguages { get; set; }
        public string Genre { get; set; }
        public int Ccu { get; set; }        
        public Information Informations { set; get; }
    }
}