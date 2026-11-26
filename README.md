# DrawnUI For .NET MAUI Demo

Demo of a [rendering engine](https://github.com/taublast/DrawnUi.Maui) to draw your UI on a Skia canvas, with gestures and animations, designed to draw pixel-perfect custom controls instead of using native ones. 

Supports **iOS**, **MacCatalyst**, **Android**, **Windows**.

* To use inside a usual Maui app, consume drawn controls here and there inside `Canvas` views.
* Create a totally drawn app with just one `Canvas` as root view, `SkiaShell` is provided for navigation.
 * Drawn controls are totally virtual, these are commands for the engine on what and how to draw on a skia canvas. 
* Free to use under the MIT license, a nuget package is available. 

## What's New

* .NET 11 and DrawnUI 1.11: both apps target .NET 11 (Android, iOS, Mac Catalyst, Windows).
* Ready for the iOS 27 SDK: scene lifecycle on iOS and Mac Catalyst.
* Mac Catalyst runs as a real Mac app, full size, with mouse input.
* 120 Hz on ProMotion iPhones.

To build against DrawnUI sources (a DrawnUi checkout next to this repo) instead of NuGet:
`dotnet build -p:UseNuget=false -p:UseSource=true -p:DrawnUiSourceDir=<DrawnUi checkout>`

[Full change log here](https://github.com/taublast/DrawnUi).

## Engine repo
https://github.com/taublast/DrawnUi.Maui

___Please star ⭐ if you like it helps very much!___

## Screenshots

### _Draw your recycled cells on a canvas_

https://github.com/taublast/DrawnUi.Maui.Demo/assets/25801194/ed70eb30-0489-4121-b93b-d95ba4d6bc4b

### _Scroll, navigate and switch virtual views_

https://github.com/taublast/DrawnUi.Maui.Demo/assets/25801194/092833ea-ef96-4fda-b4e6-3b706cbab79e

### _Create your drawn controls_

https://github.com/taublast/DrawnUi.Maui.Demo/assets/25801194/84b1274a-94f5-4844-b6a0-c9f5bf76ea92

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



