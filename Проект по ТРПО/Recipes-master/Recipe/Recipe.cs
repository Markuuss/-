using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HtmlAgilityPack;

namespace Recipe
{
    public class Recipe
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public List<string> Tags;
        public List<string> Category;
        public List<string> IngrList;

        public Recipe() { }

        public Recipe(string Link, string Name, string ImageUrl, string Description, List<string> Tags, List<string> Category, string Time, List<string> IngrList)
        {
            this.Link = Link;
            this.Name = Name;
            this.ImageUrl = ImageUrl;
            this.Description = Description;
            this.Tags = Tags;
            this.Category = Category;
            this.Time = Time;
            this.IngrList = IngrList;
        }
    }
}