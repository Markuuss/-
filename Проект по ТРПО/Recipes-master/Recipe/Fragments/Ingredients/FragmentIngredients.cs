using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V4ViewPager = Android.Support.V4.View.ViewPager;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Android.Widget;
using Android.Support.V7.Widget;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text;
using System.Linq;

namespace Recipe
{
    public class FragmentIngredients : Android.Support.V4.App.Fragment
    {
        public RecyclerView recyclerView;
        public RecyclerView.Adapter adapter;
        public RecyclerView.LayoutManager layoutManager;
        public List<Recipe> mRecipe = new List<Recipe>();
        public V4ViewPager v4ViewPager;
        private static List<Tuple<string, int>> mChecked = new List<Tuple<string, int>>();
        private View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_ingredients, container, false);
            recyclerView = view.FindViewById<Android.Support.V7.Widget.RecyclerView>(Resource.Id.recyclerView);
            layoutManager = new LinearLayoutManager(Activity);
            recyclerView.SetLayoutManager(layoutManager);
            v4ViewPager = view.FindViewById<V4ViewPager>(Resource.Id.viewpager2);
            LoadIngredients();

            var bt = view.FindViewById<Android.Support.Design.Widget.FloatingActionButton>(Resource.Id.fab);
            bt.ViewAttachedToWindow += (s, e) =>
            {
                Snackbar.Make(bt, "Нажмите на кнопку в углу, после выбора ингридиентов", Snackbar.LengthLong).Show();
            };
            bt.Click += (s, e) =>
            {
                recyclerView.Visibility = ViewStates.Invisible;
                v4ViewPager.Visibility = ViewStates.Visible;
                SetupViewPager(v4ViewPager);
                var b = mChecked;
            };

            v4ViewPager = view.FindViewById<V4ViewPager>(Resource.Id.viewpager2);
            v4ViewPager.Visibility = ViewStates.Invisible;

            return view;
        }
        //для перевыбора фрагмента
        public void RecelectedFragment()
        {
            mRecipe.Clear();
            recyclerView.Visibility = ViewStates.Visible;
            v4ViewPager.Visibility = ViewStates.Invisible;
        }

        #region Ингредиенты
        private void LoadIngredients()
        {
            try
            {
                view.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility = ViewStates.Visible;
                new Thread(new ThreadStart(() =>
                {
                    HttpWebRequest request;
                    HttpWebResponse response;
                    HtmlDocument htmlDocument = new HtmlDocument();
                    StreamReader streamFormater;//для перекодировки текста
                    List<string> _listIngr = new List<string>();

                    for (int i = 0; i < 10; i++)
                    {
                        request = (HttpWebRequest)WebRequest.Create("http://www.povarenok.ru/recipes/");
                        response = (HttpWebResponse)request.GetResponse();
                        htmlDocument.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                        for (int j = 0; j < 15; j++)
                        {
                            try
                            {
                                var recipeHtml = htmlDocument.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "uno_recipie");
                                //ингридиенты
                                var html = recipeHtml.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "ingr_fast");
                                var ingrHtml = html.FirstChild.FirstChild.FirstChild;
                                foreach (var item in ingrHtml.ChildNodes)
                                {
                                    foreach (var item2 in item.ChildNodes)
                                    {
                                        foreach (var item3 in item2.ChildNodes)
                                        {
                                            streamFormater = new StreamReader(GenerateStreamFromString(item3.InnerText.Trim()), Encoding.UTF8);
                                            _listIngr.Add(streamFormater.ReadLine());
                                        }
                                    }
                                }
                                _listIngr = _listIngr.GroupBy(c => c).Select(c => c.First()).ToList();
                                _listIngr = _listIngr.OrderBy(c => c).ToList();
                                htmlDocument.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "uno_recipie").Remove();
                            }
                            catch (System.NullReferenceException)
                            {
                                continue;
                            }
                        }
                    }
                    Activity.RunOnUiThread(() => view.FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility = ViewStates.Invisible);
                    Activity.RunOnUiThread(() => adapter = new CustomAdapter(_listIngr));
                    Activity.RunOnUiThread(() => recyclerView.SetAdapter(adapter));
                })).Start();
               
            }
            catch
            { }
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
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public class CustomAdapter : RecyclerView.Adapter
        {
            public const string TAG = "CustomAdapter";
            private List<string> dataSet;
            

            // Provide a reference to the type of views that you are using (custom ViewHolder)
            public class ViewHolder : RecyclerView.ViewHolder
            {
                private Android.Support.V7.Widget.AppCompatCheckBox checkBox;

                public Android.Support.V7.Widget.AppCompatCheckBox CheckBox
                {
                    get { return checkBox; }
                }

                public ViewHolder(View v) : base(v)
                {
                    checkBox = (Android.Support.V7.Widget.AppCompatCheckBox)v.FindViewById(Resource.Id.checkBox);
                    checkBox.Checked = false;
                }
            }

            // Initialize the dataset of the Adapter
            public CustomAdapter(List<string> dataSet)
            {
                this.dataSet = dataSet;
            }

            // Create new views (invoked by the layout manager)
            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup viewGroup, int position)
            {
                View v = LayoutInflater.From(viewGroup.Context)
                    .Inflate(Resource.Layout.ingr_recipe_item, viewGroup, false);
                ViewHolder vh = new ViewHolder(v);
                return vh;
            }

            // Replace the contents of a view (invoked by the layout manager)
            public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                // Get element from your dataset at this position and replace the contents of the view
                // with that element
                (viewHolder as ViewHolder).CheckBox.SetText(dataSet[position], TextView.BufferType.Normal);
                if (mChecked.Any(c => c.Item2 == position))
                    (viewHolder as ViewHolder).CheckBox.Checked = true;
                else
                    (viewHolder as ViewHolder).CheckBox.Checked = false;
                (viewHolder as ViewHolder).CheckBox.CheckedChange += (s, e) =>
                {
                    if(e.IsChecked == true)
                        mChecked.Add(new Tuple<string, int>((viewHolder as ViewHolder).CheckBox.Text, (viewHolder as ViewHolder).AdapterPosition));
                    else
                        mChecked.Remove(new Tuple<string, int>((viewHolder as ViewHolder).CheckBox.Text, (viewHolder as ViewHolder).AdapterPosition));
                };
            }

            // Return the size of your dataset (invoked by the layout manager)
            public override int ItemCount
            {
                get { return dataSet.Count; }
            }
        }
        #endregion

        private void SetupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            var adapter = new Adapter(((AppCompatActivity)Activity).SupportFragmentManager);
            var recipeLinks = new RecipeLinks();
            var newFragment = new RecipeListFragment(recipeLinks.links[0].Item1, LoadType.LoadSearchRecipes, view, mChecked);
            adapter.AddFragment(newFragment, recipeLinks.links[0].Item2);
            viewPager.Adapter = adapter;
            //adapter.AddFragment(new RecipeListFragment(), "Category 1");
            //adapter.AddFragment(new RecipeListFragment(), "Category 3");
            //adapter.AddFragment(new RecipeListFragment(), "Category 3");
            //((AppCompatActivity)Activity).RunOnUiThread(() => viewPager.Adapter = adapter);
        }

        private class Adapter : FragmentPagerAdapter
        {
            List<V4Fragment> mFragments = new List<V4Fragment>();
            List<string> fragmentTitles = new List<string>();

            public Adapter(V4FragmentManager v4FragmentManager) : base(v4FragmentManager)
            {

            }

            public void AddFragment(V4Fragment v4Fragment, String titleFragment)
            {
                mFragments.Add(v4Fragment);
                fragmentTitles.Add(titleFragment);
            }

            public override int Count
            {
                get { return mFragments.Count; }
            }

            public override V4Fragment GetItem(int position)
            {
                return mFragments[position];
            }

            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(fragmentTitles[position]);
            }
        }
    }
}