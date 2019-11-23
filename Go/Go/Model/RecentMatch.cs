using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Model
{
    public class RecentMatch
    {
        public DateTime Date { get; set; }
        public string PlayedAgainst { get; set; }
        public int PlayerScore { get; set; }
        public int OpponentScore { get; set; }
    }
}
