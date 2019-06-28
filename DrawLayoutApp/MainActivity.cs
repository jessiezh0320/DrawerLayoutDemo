using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Util;
using Android.Webkit;
using System;

namespace DrawLayoutApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawer;

        NavigationView navigationView;

        private string mDrawerTitle;
        private string[] mContentTitles;

        Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            mDrawerTitle = this.Title;
            mContentTitles= this.Resources.GetStringArray(Resource.Array.contents_array);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); 
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            // Get our button from the layout resource,
            // and attach an event to it
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (navigationView != null)
                setupDrawerContent(navigationView);

            ActionBarDrawerToggle toggle = new MyActionBarDrawerToggle(this, drawer, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            if (savedInstanceState == null) //first launch
            {
                toolbar.Title = mContentTitles[0];
                var fragment = WebviewFragment.NewInstance(0);

                var fragmentManager = this.FragmentManager;
                var ft = fragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_frame, fragment);
                ft.Commit();
            }
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    // set the menu layout on Main Activity  
        //    MenuInflater.Inflate(Resource.Menu.menu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            drawer.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
        //            return true;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}


        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) => {

                int ItemId = e.MenuItem.ItemId;

                e.MenuItem.SetChecked(true);

                int index = 0;
                if (ItemId == Resource.Id.nav_home) {
                    index = 0;
                } else if (ItemId == Resource.Id.nav_messages) {
                    index = 1;
                }
                else if (ItemId == Resource.Id.nav_about)
                {
                    index = 2;
                }

                // update the main content by replacing fragments
                var fragment = WebviewFragment.NewInstance(index);

                var fragmentManager = this.FragmentManager;
                var ft = fragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_frame, fragment);
                ft.Commit();

                // update selected item title, then close the drawer
                mDrawerTitle = mContentTitles[index];


                drawer.CloseDrawers();
            };
        }

        

        internal class WebviewFragment : Fragment
        {
            public const string ARG_NUMBER = "number";

            public WebviewFragment()
            {
                // Empty constructor required for fragment subclasses
            }

            public static Fragment NewInstance(int position)
            {
                Fragment fragment = new WebviewFragment();
                Bundle args = new Bundle();
                args.PutInt(WebviewFragment.ARG_NUMBER, position);
                fragment.Arguments = args;
                return fragment;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                               Bundle savedInstanceState)
            {
                View rootView = inflater.Inflate(Resource.Layout.fragment_content2, container, false);
                var i = this.Arguments.GetInt(ARG_NUMBER);
                var url = this.Resources.GetStringArray(Resource.Array.weburls_array)[i];

                var title = this.Resources.GetStringArray(Resource.Array.contents_array)[i];

                var web_view = rootView.FindViewById<WebView>(Resource.Id.webview);
                web_view.Settings.JavaScriptEnabled = true;
                web_view.SetWebViewClient(new HelloWebViewClient());
                web_view.LoadUrl(url);


                this.Activity.Title = title;
                return rootView;
            }
        }

        internal class MyActionBarDrawerToggle : ActionBarDrawerToggle
        {
            MainActivity owner;

            public MyActionBarDrawerToggle(MainActivity activity, DrawerLayout layout, Toolbar toolbar, int openRes, int closeRes)
                : base(activity, layout, toolbar, openRes, closeRes)
            {
                owner = activity;
            }

            public override void OnDrawerClosed(View drawerView)
            {
                owner.toolbar.Title = owner.Title;
                owner.InvalidateOptionsMenu();
            }

            public override void OnDrawerOpened(View drawerView)
            {
                owner.toolbar.Title = owner.mDrawerTitle;
                owner.InvalidateOptionsMenu();
            }
        }
    }
}