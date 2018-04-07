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

namespace Recipe
{
    class RecipeLinks
    {
        public List<Tuple<string, string>> links = new List<Tuple<string, string>>
        {
            Bouillons_and_soups,
            Hot_dishes,
            Salads,
            Snacks,
            Beverages,
            Sauces,
            Bakery_products,
            Dessert,
            Blanks,
            Dishes_from_pita_bread,
            Cooking_in_aerogril,
            Kashi,
            Decorations_for_dishes,
            Cooking_in_a_double_boiler,
            Dairy,
            Cooking_in_a_multivariate,
            Marinade_breaded
        };
        protected static readonly Tuple<string, string> Bouillons_and_soups = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/2/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Hot_dishes = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/6/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Salads = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/12/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Snacks = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/15/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Beverages = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/19/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Sauces = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/23/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Bakery_products = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/25/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Dessert = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/30/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Blanks = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/35/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Dishes_from_pita_bread = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/228/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Cooking_in_aerogril = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/227/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Kashi = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/55/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Decorations_for_dishes = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/187/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Cooking_in_a_double_boiler = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/247/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Dairy = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/289/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Cooking_in_a_multivariate = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/308/~0/?sort=date_create&order=desc", "Бульоны и супы");
        protected static readonly Tuple<string, string> Marinade_breaded = new Tuple<string, string>(@"http://www.povarenok.ru/recipes/category/356/~0/?sort=date_create&order=desc", "Бульоны и супы");
    }
}