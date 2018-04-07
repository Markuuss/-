using System;
using System.Collections.Generic;
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
    enum TypeRecipe
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
    }
}