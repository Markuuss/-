using System;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.App;
using Android.Graphics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;

namespace Recipe
{
    [Activity (Label = "Details", Theme = "@style/ThemeDesignDemoYellow")]
    class DetailRecipeActivity : AppCompatActivity
    {
        RecyclerView recyclerView;
        List<Step> instrycts = new List<Step>();
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_detail);

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var cheeseName = Intent.GetStringExtra("name");
            var collapsingToolbar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            collapsingToolbar.SetTitle(cheeseName);

            recyclerView = FindViewById<Android.Support.V7.Widget.RecyclerView>(Resource.Id.recyclerviewIngr);
            recyclerView.SetLayoutManager(new LinearLayoutManager(BaseContext));
            var imageu = Intent.GetStringExtra("imageUrl");
            Bitmap image = await GetImageBitmapFromUrlAsync(imageu);
            var imageView = FindViewById<ImageView>(Resource.Id.backdrop);
            imageView.SetImageBitmap(image);

            //устанавливается содержимое через пользовательский адаптер

            recyclerView.SetAdapter(new InstryctRecyclerViewAdapter(instrycts));
            LoadInstryct(Intent.GetStringExtra("recipeUrl"));
            //recyclerView.NestedScrollingEnabled = true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.sample_actions, menu);
            return true;
        }

        private void LoadInstryct(string url)
        {
            new Thread(new ThreadStart(() =>
            {
                List<Step> steps = new List<Step>();
                try
                {
                    HttpWebRequest request;
                    HttpWebResponse response;
                    HtmlDocument htmlDocument = new HtmlDocument();
                    StreamReader streamFormater;//для перекодировки текста
                    request = (HttpWebRequest)WebRequest.Create("http://www.povarenok.ru/recipes/show/150772/");
                    response = (HttpWebResponse)request.GetResponse();
                    htmlDocument.Load(response.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                    //шаги
                    List<Step> mStep = new List<Step>();
                    var stepDiv = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-steps");
                    while (stepDiv.ChildNodes.Any(x => x.Name == "table"))
                    {
                        var stepHtml = stepDiv.ChildNodes.FirstOrDefault(x => x.Name == "table");
                        var imageUrl = stepHtml.FirstChild.ChildNodes[1].ChildNodes.FirstOrDefault(x => x.Name == "td").ChildNodes[1].Attributes["href"].Value;
                        stepHtml.FirstChild.ChildNodes[1].ChildNodes.FirstOrDefault(x => x.Name == "td").Remove();
                        var textStep = stepHtml.FirstChild.ChildNodes[1].ChildNodes.FirstOrDefault(x => x.Name == "td").ChildNodes[0].InnerText.Trim();
                        textStep = textStep.Replace("&quot;", "");
                        streamFormater = new StreamReader(GenerateStreamFromString(textStep), Encoding.UTF8);
                        textStep = streamFormater.ReadLine();
                        instrycts.Add(new Step()
                        {
                            ImageLink = imageUrl,
                            StepText = textStep
                        });
                        stepDiv.Descendants("table").FirstOrDefault().Remove();
                        RunOnUiThread(() => recyclerView.GetAdapter().NotifyItemInserted(instrycts.Count));
                    }
                    //ингридиенты
                    List<string> _listIngr = new List<string>();
                    var ingrHtml = htmlDocument.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "recipe-ing");
                    while (ingrHtml.ChildNodes[1].ChildNodes.Any(x => x.Name == "li"))
                    {
                        var ingr = ingrHtml.ChildNodes[1].ChildNodes.FirstOrDefault(x => x.Name == "li");
                        _listIngr.Add(ingr.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerHtml.Trim() + " - " + ingr.ChildNodes[1].ChildNodes[3].InnerHtml.Trim());
                        ingrHtml.ChildNodes[1].ChildNodes.FirstOrDefault(x => x.Name == "li").Remove();
                    }
                    //RunOnUiThread(() =>
                    //{
                        //instrycts = mStep;
                    //});
                }
                catch { }
            })).Start();
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
        private async Task<Bitmap> GetImageBitmapFromUrlAsync(string url)
        {
            Bitmap imageBitmap = null;
            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch
            { }
            return imageBitmap;
        }
    }
}