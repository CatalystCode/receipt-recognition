﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace InvoiceIt
{
    public class CardViewCell : ViewCell
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var ctrl = (CardViewCell)bindable;
                ctrl.Text = (string)newValue;
            });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); textLabel.Text = value; }
        }

               public static readonly BindableProperty DetailProperty =
            BindableProperty.Create(nameof(Detail), typeof(string), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var ctrl = (CardViewCell)bindable;
                ctrl.Detail = (string)newValue;
            });

        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); detailTextLabel.Text = value; }
        }


        public static readonly BindableProperty Detail1Property =
            BindableProperty.Create(nameof(Detail1), typeof(string), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var ctrl = (CardViewCell)bindable;
                ctrl.Detail1 = (string)newValue;
            });

        public string Detail1
        {
            get { return (string)GetValue(Detail1Property); }
            set { SetValue(Detail1Property, value); detail1TextLabel.Text = value; }
        }

        public static readonly BindableProperty Detail2Property =
         BindableProperty.Create(nameof(Detail2), typeof(string), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
         {
             var ctrl = (CardViewCell)bindable;
             ctrl.Detail2 = (string)newValue;
         });

        public string Detail2
        {
            get { return (string)GetValue(Detail2Property); }
            set { SetValue(Detail2Property, value); detail2TextLabel.Text = value; }
        }

        public static readonly BindableProperty Detail3Property =
                 BindableProperty.Create(nameof(Detail3), typeof(string), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
                 {
                     var ctrl = (CardViewCell)bindable;
                     ctrl.Detail3 = (string)newValue;
                 });

        public string Detail3
        {
            get { return (string)GetValue(Detail3Property); }
            set { SetValue(Detail3Property, value); detail3TextLabel.Text = value; }
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CardViewCell), null, BindingMode.OneWay, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
            {
                var ctrl = (CardViewCell)bindable;
                ctrl.ImageSource = (ImageSource)newValue;
            });

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); image.Source = value; }
        }


        StackLayout layout;
        Image image;
        Label textLabel;
        Label detailTextLabel;
        private Label detail1TextLabel;
        private Label valueLabel3;
        private Label valueLabel2;
        private Label detail2TextLabel;
        private Label detail3TextLabel;

        public CardViewCell()
        {
            image = new Image
            {
                Aspect = Device.OnPlatform<Aspect>(Aspect.AspectFill, Aspect.AspectFill, Aspect.AspectFit),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 220,
                HeightRequest = 200,
                Source = ImageSource.FromUri(new Uri("https://avatars3.githubusercontent.com/u/1091304?v=3&s=460"))
            };

            textLabel = new Label
            {
                FontSize = Device.OnPlatform<double>(15, 15, 15),
                TextColor = Device.OnPlatform<Color>(Color.FromHex("030303"), Color.FromHex("030303"), Color.FromHex("030303")),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = Device.OnPlatform<Thickness>(new Thickness(12, 10, 12, 4), new Thickness(20, 10, 20, 5), new Thickness(20, 10, 20, 5)),
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                Text = "Pierce Boggan"
            };


            detail1TextLabel = new Label
            {
                FontSize = Device.OnPlatform<double>(15, 15, 15),
                TextColor = Device.OnPlatform<Color>(Color.FromHex("030303"), Color.FromHex("030303"), Color.FromHex("030303")),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = Device.OnPlatform<Thickness>(new Thickness(12, 10, 12, 4), new Thickness(20, 10, 20, 5), new Thickness(20, 10, 20, 5)),
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                Text = "Pierce Boggan"
            };


            detail2TextLabel = new Label
            {
                FontSize = Device.OnPlatform<double>(15, 15, 15),
                TextColor = Device.OnPlatform<Color>(Color.FromHex("030303"), Color.FromHex("030303"), Color.FromHex("030303")),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = Device.OnPlatform<Thickness>(new Thickness(12, 10, 12, 4), new Thickness(20, 10, 20, 5), new Thickness(20, 10, 20, 5)),
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                Text = "Pierce Boggan"
            };

            detailTextLabel = new Label
            {
                FontSize = Device.OnPlatform<double>(13, 13, 13),
                TextColor = Device.OnPlatform<Color>(Color.FromHex("8F8E94"), Color.FromHex("8F8E94"), Color.FromHex("8F8E94")),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = Device.OnPlatform<Thickness>(new Thickness(12, 0, 10, 12), new Thickness(20, 0, 20, 20), new Thickness(20, 0, 20, 20)),
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                Text = "pierce@xamarin.com"
            };


            detail3TextLabel = new Label
            {
                FontSize = Device.OnPlatform<double>(15, 15, 15),
                TextColor = Device.OnPlatform<Color>(Color.FromHex("030303"), Color.FromHex("030303"), Color.FromHex("030303")),
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = Device.OnPlatform<Thickness>(new Thickness(12, 10, 12, 4), new Thickness(20, 10, 20, 5), new Thickness(20, 10, 20, 5)),
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                Text = "Pierce Boggan"
            };

            layout = new StackLayout
            {
                BackgroundColor = Color.White,
                Spacing = 0,
                Children = { image, detail1TextLabel, detail2TextLabel, detail3TextLabel, textLabel }
            };

            View = layout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            View.BindingContext = BindingContext;
        }
    }
}
