﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppH2014.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SalesItem> Items { get; set; }
    }
}