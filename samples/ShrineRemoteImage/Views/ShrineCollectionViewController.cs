﻿//
// ShrineCollectionViewController.cs
//
// Author:
//       Pal Dorogi "ilap" <pal.dorogi@gmail.com>
//
// Copyright (c) 2017 Pal Dorogi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using UIKit;
using CoreGraphics;
using MaterialComponents.MaterialFlexibleHeader;
using MaterialComponents.MaterialCollections;
using Foundation;

namespace ShrineRemoteImage.iOS.View
{
    [Register ("ShrineCollectionViewController")]
    public class ShrineCollectionViewController : UICollectionViewController
    {

        public MDCFlexibleHeaderViewController headerViewController;
        ShrineHeaderContentView headerContentView = new ShrineHeaderContentView (CGRect.Empty);
        ShrineData shrineData;

        static readonly NSString cellId = new NSString("ShrineCollectionViewCell");

        [Export("initWithCollectionViewLayout:")]
        public ShrineCollectionViewController(UICollectionViewLayout layout) : base (layout)
        {
            shrineData = new ShrineData();
            shrineData.readJSON();
            CollectionView.RegisterClassForCell(typeof(ShrineCollectionViewCell), cellId);
            CollectionView.BackgroundColor = new UIColor(0.97f, 1f);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return shrineData.titles.Count;
        }


        [Export("collectionView:cellForItemAtIndexPath:")]
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ShrineCollectionViewCell)collectionView.DequeueReusableCell(cellId, indexPath);
            if (cell == null)
            {
                return new UICollectionViewCell(CGRect.Empty);
            }

            var itemNum = indexPath.Row;
            var title = shrineData.titles[itemNum];
            var imageName = shrineData.imageNames[itemNum];
            var avatar = shrineData.avatars[itemNum];
            var shopTitle = shrineData.shopTitles[itemNum];
            var price = shrineData.prices[itemNum];

            cell.PopulateCell(title, imageName, avatar, shopTitle, price);

            return cell;
        }


        [Export("collectionView:didSelectItemAtIndexPath:")]
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var itemNum = indexPath.Row;
            var detailVC = new ShrineDetailViewController();
            detailVC.productTitle = shrineData.titles[itemNum];
            detailVC.desc = shrineData.descriptions[itemNum];
            detailVC.imageName = shrineData.imageNames[itemNum];

            PresentViewController(detailVC, true, null);
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize SizeForItemAtIndexPath(UICollectionView collectionView,
                                             UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var cellWidth = ((collectionView.Frame.Width - (2 * 5)) / 2) - (2 * 5);
            var cellHeight = cellWidth * 1.2;
            return new CGSize(cellWidth, cellHeight);
        }

        public override void ViewWillAppear(bool animated)
        {
            SizeHeaderView();
            CollectionView.CollectionViewLayout.InvalidateLayout();
        }

        public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            SizeHeaderView();
            CollectionView.CollectionViewLayout.InvalidateLayout();
        }

        void SizeHeaderView() {
            var hv = headerViewController.HeaderView;

            var bounds = UIScreen.MainScreen.Bounds;
            if (bounds.Size.Width < bounds.Size.Height) {
                hv.MinimumHeight = 440;
            } else {
                hv.MinimumHeight = 72;    
            }
            hv.MinimumHeight = 72;
        }

        public void SetupHeaderView() {
            
            var hv = headerViewController.HeaderView;

            hv.TrackingScrollView = this.CollectionView;
            hv.MaximumHeight = 440;
            hv.MinimumHeight = 72;
            hv.BackgroundColor = UIColor.White;
            hv.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | 
                UIViewAutoresizing.FlexibleHeight;

            headerContentView.Frame = hv.Frame;
            headerContentView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
                UIViewAutoresizing.FlexibleHeight;
            hv.AddSubview(headerContentView);
        }


        public override void DecelerationEnded(UIScrollView scrollView)
        {
            if (scrollView == headerViewController.HeaderView.TrackingScrollView)
            {
                headerViewController.HeaderView.TrackingScrollViewDidEndDecelerating();
            }
        }

        public override void WillEndDragging(UIScrollView scrollView, CoreGraphics.CGPoint velocity, ref CoreGraphics.CGPoint targetContentOffset)
        {
            var hv = headerViewController.HeaderView;

            if (scrollView == hv.TrackingScrollView)
            {
                hv.TrackingScrollViewWillEndDraggingWithVelocity(velocity, targetContentOffset);
            }
        }

        [Export("scrollViewDidScroll:")]
        public override void Scrolled(UIScrollView scrollView)
        {

            headerViewController.Scrolled(scrollView);

            var scrollOffsetY = scrollView.ContentOffset.Y;
            var duration = 0.5;
            var opacity = 1f;
            var logoTextImageOpacity = 0f;

            if (scrollOffsetY > -240)
            {
                opacity = 0f;
                logoTextImageOpacity = 1f;
            }

            UIView.Animate(duration, () => {
                this.headerContentView.scrollView.Alpha = opacity;
                this.headerContentView.pageControl.Alpha = opacity;
                this.headerContentView.logoImageView.Alpha = opacity;
                this.headerContentView.logoTextImageView.Alpha = logoTextImageOpacity;
            });
        }
    }
}
