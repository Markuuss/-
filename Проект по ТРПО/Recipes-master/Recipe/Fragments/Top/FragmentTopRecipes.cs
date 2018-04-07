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
    public class FragmentTopRecipes : Android.Support.V4.App.Fragment
    {
        private View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_top_recipes, container, false);
            var viewPager = view.FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.viewpager1);
            if (viewPager != null)
                SetupViewPager(viewPager);
            //var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            //tabLayout.SetupWithViewPager(viewPager);
            //tabLayout.TabSelected += TabLayout_TabSelected;
            return view;
        }

        private void SetupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            var adapter = new Adapter(((AppCompatActivity)Activity).SupportFragmentManager);
            var recipeLinks = new RecipeLinks();
            var newFragment = new RecipeListFragment(recipeLinks.links[0].Item1, LoadType.LoadTop, view);
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