using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;

namespace HPstats
{
    public partial class FindStats : Form
    {
        public FindStats()
        {
            InitializeComponent();
        }

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };

    }
}
