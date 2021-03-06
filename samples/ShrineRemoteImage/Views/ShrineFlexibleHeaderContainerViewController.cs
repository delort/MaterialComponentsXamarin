﻿//
// ShrineFlexibleHeaderContainerViewController.cs
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
                
using System;
using MaterialComponents.MaterialFlexibleHeader;
using UIKit;
using System.Runtime.CompilerServices;
using Foundation;

namespace ShrineRemoteImage.iOS.View
{
    public class ShrineFlexibleHeaderContainerViewController  : MDCFlexibleHeaderContainerViewController
    {

        public static ShrineFlexibleHeaderContainerViewController InitializeWithController() {

            var layout = new UICollectionViewFlowLayout();
            var sectionInset = 10f;

            layout.SectionInset = new UIEdgeInsets(sectionInset, sectionInset,
                                                   sectionInset, sectionInset);

            var collectionVC = new ShrineCollectionViewController(layout);

            var result = new ShrineFlexibleHeaderContainerViewController(collectionVC);

            collectionVC.headerViewController = result.HeaderViewController;
            collectionVC.SetupHeaderView();

            return result;    
        }

        protected ShrineFlexibleHeaderContainerViewController(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }

        public ShrineFlexibleHeaderContainerViewController()
        {
            
        }

        [Export("initWithContentViewController:")]
        public ShrineFlexibleHeaderContainerViewController(UIViewController controller) : base(controller)
        {
            Console.WriteLine("Initialise with Controller");
        }
    }
}
