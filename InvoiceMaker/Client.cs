using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMaker
{
    class Client
    {
        public string fullname;

        public Client() { }

        public Client(string name)
        {
            fullname = name;
        }
    }

    class InvoiceLine
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string providerID { get; set; }
        public string weekCommencing { get; set; }
        public string asdReference { get; set; }
        public int minutesThisCall { get; set; }
        public string comment { get; set; }
        public string hospitalAdmissionDate { get; set; }
        public int customer { get; set; }
    }
    class InputLine
    {
        public string booking_date { get; set; }
        public string time_from { get; set; }
        public string time_to { get; set; }
        public string client_title { get; set; }
        public string client_fname { get; set; }
        public string client_sname { get; set; }
        public string client_house { get; set; }
        public string client_street { get; set; }
        public string client_town { get; set; }
        public string client_county { get; set; }
        public string client_postcode { get; set; }
        public string staff_title { get; set; }
        public string staff_fname { get; set; }
        public string staff_sname { get; set; }
        public string staff_house { get; set; }
        public string staff_street { get; set; }
        public string staff_town { get; set; }
        public string staff_county { get; set; }
        public string staff_postcode { get; set; }
        public string description { get; set; }
        public string our_ref { get; set; }
        public string task_name { get; set; }
    }
}
