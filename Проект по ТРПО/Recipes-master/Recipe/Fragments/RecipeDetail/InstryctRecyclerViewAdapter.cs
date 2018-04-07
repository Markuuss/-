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
    class InstryctRecyclerViewAdapter : RecyclerView.Adapter
    {
        TypedValue typedValue = new TypedValue();
        int background;
        List<Step> values;

        public InstryctRecyclerViewAdapter(List<Step> instrycts)
        {
            values = instrycts;
        }

        public override int ItemCount
        {
            get { return values.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var _holder = holder as ViewInstryctHolder;
            _holder.text.Text = values[position].StepText;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_view_item_recipecIngr, parent, false);
            view.SetBackgroundResource(background);
            return new ViewInstryctHolder(view);
        }

        private class ViewInstryctHolder : RecyclerView.ViewHolder
        {
            public Recipe BoundString { get; set; }
            public View view { get; set; }
            public TextView text { get; set; }

            public ViewInstryctHolder(View view) : base(view)
            {
                this.view = view;
                text = view.FindViewById<TextView>(Android.Resource.Id.Title);
                Animation animation = AnimationUtils.LoadAnimation(view.Context, Resource.Animation.item_animation_from_left);
                view.StartAnimation(animation);
            }
        }
    }
}