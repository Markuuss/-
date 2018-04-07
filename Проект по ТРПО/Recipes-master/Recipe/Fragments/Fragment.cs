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
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Recipe
{
    public class Fragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment, container, false);
            HasOptionsMenu = true;//для вызова OnCreateOptionsMenu
            var toolbar = view.FindViewById<V7Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            var viewPager = view.FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
                SetupViewPager(viewPager);
            return view;
        }

        #region SearchView region
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.top_menu, menu);
            //не скрытая панель поиска
            var searchManager = (SearchManager)((AppCompatActivity)Activity).GetSystemService(Context.SearchService);
            var searchView = (Android.Support.V7.Widget.SearchView)menu.FindItem(Resource.Id.search).ActionView;
            var searchableInfo = searchManager.GetSearchableInfo(((AppCompatActivity)Activity).ComponentName);
            searchView.SetSearchableInfo(searchableInfo);
            searchView.SetIconifiedByDefault(false);
            

            base.OnCreateOptionsMenu(menu, inflater);
        }
        #endregion

        private void SetupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            var adapter = new Adapter(((AppCompatActivity)Activity).SupportFragmentManager);
            var recipeLinks = new RecipeLinks();
            var newFragment = new RecipeListFragment(recipeLinks.links[0].Item1, Type.FragmentCard.Top);
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