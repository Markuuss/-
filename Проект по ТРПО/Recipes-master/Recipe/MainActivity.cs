using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using BottomNavigationBar.Listeners;
using BottomNavigationBar;
using System.Threading;
using System.Collections.Generic;
using Android.Support.Design.Widget;

namespace Recipe
{
    [Activity(Label = "Recipe", MainLauncher = true, Theme = "@style/ThemeDesignDemoStandart")]
    public class MainActivity : AppCompatActivity, IOnMenuTabClickListener
    {
        private BottomBar _bottomBar;

        private FragmentRecipesCategories fragmentRecipesCategories;
        private FragmentTopRecipes fragmentTopRecipes;
        private FragmentIngredients fragmentIngredients;
        private Android.Support.V4.App.Fragment currentFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            fragmentRecipesCategories = new FragmentRecipesCategories();
            fragmentTopRecipes = new FragmentTopRecipes();
            fragmentIngredients = new FragmentIngredients();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragmentContainer, fragmentRecipesCategories, "RecipesCategories").Commit();
            currentFragment = fragmentRecipesCategories;
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragmentContainer, fragmentTopRecipes, "TOP").Hide(fragmentTopRecipes).Commit();
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragmentContainer, fragmentIngredients, "Ingredients").Hide(fragmentIngredients).Commit();

            _bottomBar = BottomBar.Attach(this, savedInstanceState);
            _bottomBar.SetItems(Resource.Menu.bottombar_menu);
            _bottomBar.SetOnMenuTabClickListener(this);
            // Setting colors for different tabs when there's more than three of them.
            // You can set colors for tabs in two different ways as shown below.
            _bottomBar.MapColorForTab(0, "#5D4037");
            _bottomBar.MapColorForTab(1, "#5D4037");
            _bottomBar.MapColorForTab(2, "#7B1FA2");
            //_bottomBar.MapColorForTab(3, "#FF5252");
            //_bottomBar.MapColorForTab(4, "#FF9800");
            //_bottomBar.Hide(false);//скрытие нижней панели
        }

        #region buttomBar Region
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            // Necessary to restore the BottomBar's state, otherwise we would
            // lose the current tab on orientation change.
            _bottomBar.OnSaveInstanceState(outState);
        }

        #region IOnMenuTabClickListener implementation

        public void OnMenuTabSelected(int menuItemId)
        {
            switch (menuItemId)
            {
                case Resource.Id.bb_menu_recents:
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Hide(currentFragment).Commit();
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Show(fragmentRecipesCategories).Commit();
                    currentFragment = fragmentRecipesCategories;
                    break;
                case Resource.Id.bb_menu_favorites:
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Hide(currentFragment).Commit();
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Show(fragmentTopRecipes).Commit();
                    currentFragment = fragmentTopRecipes;
                    break;
                case Resource.Id.bb_menu_nearby:
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Hide(currentFragment).Commit();
                    SupportFragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_slide_in_top, Resource.Animation.design_bottom_sheet_slide_out).Show(fragmentIngredients).Commit();
                    currentFragment = fragmentIngredients;
                    break;
                //case Resource.Id.bb_menu_friends:
                //    //SupportFragmentManager.BeginTransaction().Hide(fragment2).Commit();
                //    //SupportFragmentManager.BeginTransaction().Show(fragment1).Commit();
                //    break;
                //case Resource.Id.bb_menu_food:
                //    //SupportFragmentManager.BeginTransaction().Hide(fragment1).Commit();
                //    //SupportFragmentManager.BeginTransaction().Show(parceRecipesFragment).Commit();
                //    break;
            }
        }

        public void OnMenuTabReSelected(int menuItemId)
        {
            if(menuItemId == Resource.Id.bb_menu_nearby)
            {
                fragmentIngredients.RecelectedFragment();
            }
        }

        #endregion
        #endregion
    }
}

