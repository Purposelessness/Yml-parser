using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Application.MVP.View
{
    public class OfferHolder : RecyclerView.ViewHolder
    {
        public TextView OfferId { get; }

        public OfferHolder(Android.Views.View itemView, Action<int> listener) :
            base(itemView)
        {
            OfferId = itemView.FindViewById<TextView>(Resource.Id.OfferId)!;
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }

    public class OfferAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick = delegate { };
        public List<string> Offers { get; } = new List<string>();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder,
            int position)
        {
            if (!(holder is OfferHolder offerHolder))
            {
                return;
            }

            offerHolder.OfferId.Text = Offers[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(
            ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)!
                .Inflate(Resource.Layout.OfferItem, parent, false)!;
            return new OfferHolder(itemView, (position) =>
            {
                ItemClick(this, position);
            });
        }

        public override int ItemCount => Offers.Count;
    }
}