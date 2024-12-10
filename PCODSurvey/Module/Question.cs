using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCODSurvey.Module
{
    public class QuestionViewModel
    {
        public int questionid { get; set; }

        public int optionid { get; set; }
        public string questiontype { get; set; }
        public string questionnum { get; set; }
        public int optionvalue { get; set; }
        public string questionname { get; set; }
        public string userage { get; set; }

    }
}