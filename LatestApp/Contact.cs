using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LatestApp
{
   public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }

        //[Newtonsoft.Json.JsonIgnore]
        //try to don't push it at the backend
       // public string NameDisplay { get { return NameUtc.ToLocal ==lökfl} }

        public Contact(string name, string location)
        {
            Name = name;
            Location = location;

        }

        public Contact()
        {

        }
        public override string ToString() { return Name + " " + Location; }
    }
}