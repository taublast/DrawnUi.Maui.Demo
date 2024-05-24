# DrawnUI For .NET MAUI Demo

Demo of a [rendering engine](https://github.com/taublast/DrawnUi.Maui) to draw your UI on a Skia canvas, with gestures and animations, designed to draw pixel-perfect custom controls instead of using native ones. 

Supports **iOS**, **MacCatalyst**, **Android**, **Windows**.

* To use inside a usual Maui app, consume drawn controls here and there inside `Canvas` views.
* Create a totally drawn app with just one `Canvas` as root view, `SkiaShell` is provided for navigation.
 * Drawn controls are totally virtual, these are commands for the engine on what and how to draw on a skia canvas. 
* Free to use under the MIT license, a nuget package is available. 

## What's New

Added Xaml2Pdf example, some controls and fixes, and app window is now resizable again to be able to check adaptive layouts.
Using nuget 1.2.2.5

## Screenshots

### _Draw your recycled cells on a canvas_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/f562e126-1586-4a4f-a08c-fb6437c0df27

### _Scroll, navigate and switch virtual views_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/e86200de-16ba-4e24-a7f5-ec7ba944a9a9

### _Create your drawn controls_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/2182de51-929e-47f6-a560-e9d97ad16e52

### _Draw rich and tappable text_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/a2efa080-f234-4d04-a7f5-aab8af462fb2

### _Play with the Canvas_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/00e9c876-c529-40d2-b67d-6bbc39794109

https://github.com/taublast/AppoMobi.Maui.DrawnUi.SpaceShooter

## Features

* __Draw your UI using SkiaSharp with hardware acceleration__
* __Easily create your controls and animations__
* __Design in Xaml or code-behind__
* __2D and 3D Transforms__
* __Animations and transforms__ targeting max fps
* __Gestures__ support for panning, scrolling and zooming (_rotation on the roadmap_)
* __Caching system__ for elements and images
* __Optimized for performance__, rendering only visible elements, recycling templates etc
* __Navigate__ on canvas using MAUI familiar Shell techniques 

## Engine repo
https://github.com/taublast/DrawnUi.Maui

___Please star ⭐ if you like it helps very much!___

