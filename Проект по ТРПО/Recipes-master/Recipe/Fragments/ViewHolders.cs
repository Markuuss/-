using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Recipe
{
    class ViewRecipeHolder : RecyclerView.ViewHolder
    {
        public Recipe BoundString { get; set; }
        public View view { get; set; }
        public TextView header { get; set; }
        public TextView descr { get; set; }
        public TextView time { get; set; }
        public ImageView imageView { get; set; }
        //public EventHandler ClickHundler { get; set; }

        public ViewRecipeHolder(View view) : base(view)
        {
            this.view = view;
            imageView = view.FindViewById<ImageView>(Android.Resource.Id.Icon);
            header = view.FindViewById<TextView>(Android.Resource.Id.Title);
            descr = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            time = view.FindViewById<TextView>(Android.Resource.Id.Text2);
            Animation animation = AnimationUtils.LoadAnimation(view.Context, Resource.Animation.item_animation_from_left);
            view.StartAnimation(animation);
        }
    }
    class ViewDescHolder : RecyclerView.ViewHolder
    {
        public Recipe BoundString { get; set; }
        public View view { get; set; }
        public Spinner spinner { get; set; }
        public TextView textView { get; set; }
        public Spinner spinner1 { get; set; }
        public TextView textView1 { get; set; }
        Context context;
        RecipeListFragment recipeListFragment;//копия объекта листа
        //public EventHandler ClickHundler { get; set; }

        public ViewDescHolder(View view, Context context, RecipeListFragment recipeListFragment) : base(view)
        {
            this.view = view;
            this.context = context;
            this.recipeListFragment = recipeListFragment;
            spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            var adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.recipes_array, Android.Resource.Layout.SimpleSpinnerItem);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            textView = view.FindViewById<TextView>(Resource.Id.textView);

            spinner1 = view.FindViewById<Spinner>(Resource.Id.spinner2);
            adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.recipes_criter, Android.Resource.Layout.SimpleSpinnerItem);
            spinner1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelectedSorted);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner1.Adapter = adapter;
            textView1 = view.FindViewById<TextView>(Resource.Id.textView2);
            Animation animation = AnimationUtils.LoadAnimation(view.Context, Resource.Animation.item_animation_from_left);
            view.StartAnimation(animation);
        }

        private void Spinner_ItemSelectedSorted(object sender, AdapterView.ItemSelectedEventArgs e)
        {

        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            //остановка потока загрузки старых рецептов
            //
            //сделать корректнее проверку
            //
            if (recipeListFragment.mRecipe.Count > 1)
            {
                recipeListFragment.loadRecipesThread.Abort();
            }
            //удаление данных с адаптера и листа
            recipeListFragment.recyclerView.GetAdapter().NotifyItemRangeRemoved(1, recipeListFragment.mRecipe.Count);
            recipeListFragment.mRecipe.RemoveRange(1, recipeListFragment.mRecipe.Count - 1);
            //запуск потока с новыми рецептами
            var recipeLinks = new RecipeLinks();
            recipeListFragment.UpdateAdapter(recipeLinks.links[e.Position].Item1);
            //сделать событие завершения асинхронного кода и обнуления листа с рецептами, при выборе категории

            string toast = string.Format("Выбрана категория {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(context, toast, ToastLength.Long).Show();
        }
    }
}