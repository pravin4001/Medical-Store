
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace MedicalProject.Models
{
    [DataContract]
    public class dashboard
    {
        public dashboard(string label, int y)
        {
            this.Label = label;
            this.Y = y;
        }

        DateTime startdate { get; set; }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";



        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}