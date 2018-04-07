using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Recipe
{
    class RecyclerViewAdapter : RecyclerView.Adapter
    {
        TypedValue typedValue = new TypedValue();
        int background;
        List<Recipe> values;
        Android.App.Activity parent;
        Context context;
        RecipeListFragment recipeListFragment;

        public RecyclerViewAdapter(Android.App.Activity activity, List<Recipe> items, Context context, RecipeListFragment recipeListFragment)
        {
            this.recipeListFragment = recipeListFragment;
            parent = activity;
            this.context = context;
            activity.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, typedValue, true);
            background = typedValue.ResourceId;
            values = items;
        }



        public override int ItemCount
        {
            get { return values.Count; }
        }

        public async override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            switch (GetItemViewType(position))
            {
                case CardType.Recipe:
                    {
                        var _holder = holder as ViewRecipeHolder;
                        _holder.BoundString = values[position];
                        _holder.header.Text = values[position].Name;
                        _holder.descr.Text = values[position].Description;
                        _holder.time.Text = " \u2022 Время приготовления: " + values[position].Time;
                        //асинхронная загрузка изображения
                        Bitmap image = await GetImageBitmapFromUrlAsync(values[position].ImageUrl);
                        _holder.imageView.SetImageBitmap(image);

                        _holder.view.Click += new EventHandler((sender, e) =>
                        {
                            var context = _holder.view.Context;
                            var intent = new Intent(context, typeof(DetailRecipeActivity));
                            //передается информация в активити
                            intent.PutExtra("imageUrl", values[position].ImageUrl);
                            intent.PutExtra("name", values[position].Name);
                            intent.PutExtra("recipeUrl", values[position].Link);
                            context.StartActivity(intent);
                        });
                        break;
                    }
                case CardType.Description:
                    {
                        var _holder = holder as ViewDescHolder;
                        _holder.BoundString = values[position];
                        break;
                    }
                default:
                    break;
            }
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

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            switch (viewType)
            {
                case CardType.Recipe:
                    {
                        var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_item, parent, false);
                        view.SetBackgroundResource(background);
                        return new ViewRecipeHolder(view);
                    }
                case CardType.Description:
                    {
                        var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.header_categories_item, parent, false);
                        view.SetBackgroundResource(background);
                        ViewDescHolder viewDescHolder = new ViewDescHolder(view, context, recipeListFragment);
                        return viewDescHolder;
                    }
                default:
                    {
                        var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_item, parent, false);
                        view.SetBackgroundResource(background);
                        return new ViewRecipeHolder(view);
                    }
            }
        }

        public override int GetItemViewType(int position)
        {
            int viewType;
            if (values[position].Name != null)
            {
                viewType = CardType.Recipe;

            }
            else
            {
                viewType = CardType.Description;
            }
            return viewType;
        }
    }
}