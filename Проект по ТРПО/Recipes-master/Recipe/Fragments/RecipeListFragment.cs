using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using HtmlAgilityPack;
using JP.Wasabeef.Recyclerview.Adapters;

namespace Recipe
{
    class RecipeListFragment : Android.Support.V4.App.Fragment
    {
        public List<Recipe> mRecipe = new List<Recipe>();
        private string Link;
        public RecyclerView recyclerView;
        public Thread loadRecipesThread;
        private View parentView;
        public RecipeListFragment(string link, LoadType LoadType, View view, List<Tuple<string, int>> selectedIngrForSearchRecipe = null)
        {
            parentView = view;
            Link = link;
            if (LoadType == LoadType.LoadCategories)
                mRecipe.Add(new Recipe());//заголовок
            else if (LoadType == LoadType.LoadTop)
                UpdateAdapter("http://www.povarenok.ru/recipes/~1/");
            else if (LoadType == LoadType.LoadSearchRecipes)
            {
                UpdateAdapter("http://www.povarenok.ru/recipes/~1/", selectedIngrForSearchRecipe);
                parentView.FindViewById<Android.Support.Design.Widget.FloatingActionButton>(Resource.Id.fab).Visibility = ViewStates.Invisible;
            }
        }
        public void UpdateAdapter(string url, List<Tuple<string, int>> selectedIngrForSearchRecipe = null)
        {
            loadRecipesThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    HttpWebRequest request;
                    HttpWebResponse response;
                    HtmlDocument htmlDocument = new HtmlDocument();
                    StreamReader streamFormater;//для перекодировки текста
                    try
                    {
                        if (parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility == ViewStates.Invisible || parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility == ViewStates.Gone)
                        {
                            Activity.RunOnUiThread(() => parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility = ViewStates.Visible);
                        }
                    }
                    catch (NullReferenceException)
                    { }
                    for (int i = 0; i < 10; i++)
                    {
                        url = url.Replace(("~" + i), ("~" + (i + 1)));
                        request = (HttpWebRequest)WebRequest.Create(url);
                        response = (HttpWebResponse)request.GetResponse();
                        htmlDocument.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                        for (int j = 0; j < 15; j++)
                        {
                            try
                            {
                                var recipeHtml = htmlDocument.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "uno_recipie");
                                //блок с рецептoм
                                var descHtml = recipeHtml.Descendants("td").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-list-right-col");
                                //категории рецепта
                                var categoriesHtml = descHtml.Descendants("nobr").ToList();
                                List<string> mCategories = new List<string>();
                                foreach (var item in categoriesHtml)
                                {
                                    streamFormater = new StreamReader(GenerateStreamFromString(item.FirstChild.InnerText.Trim()), Encoding.UTF8);
                                    string categorie = streamFormater.ReadLine();
                                    mCategories.Add(categorie);
                                }
                                //тэги рецепта
                                var tagHtml = recipeHtml.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipie_tags").Descendants("span").ToList();
                                List<string> mTags = new List<string>();
                                foreach (var item in tagHtml)
                                {
                                    streamFormater = new StreamReader(GenerateStreamFromString(item.FirstChild.InnerText.Trim()), Encoding.UTF8);
                                    string tag = streamFormater.ReadLine();
                                    mTags.Add(tag);
                                }
                                //название
                                string name = recipeHtml.Descendants("a").FirstOrDefault(x => x.Attributes.Contains("href") && x.Attributes.Contains("title")).InnerText.Trim();
                                name = name.Replace("&quot;", "");
                                //создается поток с кодировкой utf-8
                                streamFormater = new StreamReader(GenerateStreamFromString(name), Encoding.UTF8);
                                name = streamFormater.ReadLine();
                                //описание
                                string description = descHtml.ChildNodes[0].InnerText.Trim();
                                streamFormater = new StreamReader(GenerateStreamFromString(description), Encoding.UTF8);
                                description = streamFormater.ReadLine();
                                //ссылка
                                string recipeLink = recipeHtml.Descendants("a").FirstOrDefault(x => x.Attributes.Contains("href")).Attributes["href"].Value;
                                //изображение
                                string imageUrl = null;
                                imageUrl = GetImageFromUrl(recipeLink);
                                //время приготовления
                                string time = GetTimeRecipeFromUrl(recipeLink);
                                //ингридиенты
                                List<string> _listIngr = new List<string>();
                                var html = recipeHtml.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "ingr_fast");
                                var ingrHtml = html.FirstChild.FirstChild.FirstChild;
                                foreach (var item in ingrHtml.ChildNodes)
                                {
                                    //Console.WriteLine(item.Name);
                                    foreach (var item2 in item.ChildNodes)
                                    {
                                        foreach (var item3 in item2.ChildNodes)
                                        {
                                            streamFormater = new StreamReader(GenerateStreamFromString(item3.InnerText.Trim()), Encoding.UTF8);
                                            _listIngr.Add(streamFormater.ReadLine());
                                        }
                                    }
                                }
                                //поиск на совпадение ингредиентов
                                if(selectedIngrForSearchRecipe != null)
                                {
                                    foreach (var ingr in _listIngr)
                                    {
                                        if(selectedIngrForSearchRecipe.Any(c => c.Item1 == ingr))
                                        {
                                            mRecipe.Add(new Recipe
                                            {
                                                Name = name,
                                                Link = recipeLink,
                                                ImageUrl = imageUrl != null ? imageUrl : "https://pp.userapi.com/c604426/v604426563/5a9d/xplUnWS7EFA.jpg",
                                                Description = description,
                                                Category = mCategories,
                                                Tags = mTags,
                                                Time = time != null ? time : "-",
                                                IngrList = _listIngr
                                            });
                                            //Activity.RunOnUiThread(() => recyclerView.GetAdapter().NotifyItemInserted(mRecipe.Count));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    mRecipe.Add(new Recipe
                                    {
                                        Name = name,
                                        Link = recipeLink,
                                        ImageUrl = imageUrl != null ? imageUrl : "https://pp.userapi.com/c604426/v604426563/5a9d/xplUnWS7EFA.jpg",
                                        Description = description,
                                        Category = mCategories,
                                        Tags = mTags,
                                        Time = time != null ? time : "-",
                                        IngrList = _listIngr
                                    });
                                    
                                }
                                htmlDocument.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "uno_recipie").Remove();
                                Activity.RunOnUiThread(() => recyclerView.GetAdapter().NotifyItemInserted(mRecipe.Count));
                                if (parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility == ViewStates.Visible || parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility == ViewStates.Gone)
                                {
                                    Activity.RunOnUiThread(() => parentView.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility = ViewStates.Invisible);
                                }
                            }
                            catch (System.NullReferenceException)
                            {
                                continue;
                            }
                        }
                    }
                }
                //если не установлено интернет соединение
                catch (System.Net.WebException)
                { }
            }));
            loadRecipesThread.Start();
        }
        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        private string GetImageFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
            var recipeText = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-text");
            string imageUrl = null;
            if (recipeText.ChildNodes.Any(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "bbimg"))
            {
                imageUrl = recipeText.Descendants("img").LastOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "bbimg"
                    && !x.Attributes["src"].Value.Contains("smiles")).Attributes["src"].Value;
                imageUrl = imageUrl.Insert(0, "http://www.povarenok.ru/");
            }
            else
            {
                var recipeImg = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-img");
                imageUrl = recipeImg.ChildNodes[1].Attributes["src"].Value;
            }
            return imageUrl;
        }
        private string GetTimeRecipeFromUrl(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                var recipeInfo = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-time-peaces");
                return recipeInfo.ChildNodes[1].ChildNodes[3].InnerText.Trim();
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //элемент recycleView
            var view = inflater.Inflate(Resource.Layout.recipe_list, container, false);
            var recyclerView = view.JavaCast<RecyclerView>();
            recyclerView.SetLayoutManager(new LinearLayoutManager(container.Context));
            //устанавливается содержимое через пользовательский адаптер
            recyclerView.SetAdapter(new RecyclerViewAdapter(Activity, mRecipe, Context, this));
            //сглаживание прокрутки
            recyclerView.NestedScrollingEnabled = true;
            this.recyclerView = recyclerView;
            return recyclerView;
        }
        public void Search(string text)
        {
           // var t =
                //обработка принятого текста - поиск совпадений в списке рецептов - обновление адаптера
        }
    }
}