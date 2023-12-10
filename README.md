# AppoMobi.Maui.DrawnUi DEMO

Using rendering engine for .Net MAUI to draw your UI on a Skia canvas, with gestures and animations.

Supports **iOS**, **MacCatalyst**, **Android**, **Windows**.

* To use inside a usual Maui app, consume drawn controls here and there inside `Canvas` views.
* Create a totally drawn app with just one `Canvas` as root view and consume controls inside, `SkiaShell` is provided for navigation.

_The current development state is __PRE-ALPHA__, some features remain to be implemented, the project is active._

The rendering engine is free to use, the goal is to provide an easy way to create and draw custom-made ui elements.

Library repo will go public at Alpha stage, Pre-Alpha nuget package is already available to be consumed. 

## What's new

__1.0.1.30-pre__
[nuget](https://www.nuget.org/packages/AppoMobi.Maui.DrawnUi/1.0.1.30-pre)
* Demo: ⛳ added __WheelPicker__ control, example opening popups from the center of a tapped control. Caching improved here and there.
* SkiaMarkdownLabel and Skialabel: __Colored Emojies__  with skin tones 🏻🏼🏽🏾🏿
* Performance improvements for SkiaImage and internal layout measurements __more fps__ 🤩!
* Fixes for: Windows unicode, ZIndex in Row/Column layout and more..

__Previously..__
* Demo cells `UseCache="DoubleBufferedImage"` fixes smooth scrolling.
* `SkiaControl` now derives from `VisualElement`. Full support for xaml styles, states, triggers, hotreload now works better, will be improved more.
* SkiaShell breaking changes, it is now a reworked ContentPage, many methods renamed to be more inline with Maui Shell.

## Development Notes

* All files to be consumed (images etc) must be placed inside the maui app Resources/Raw folder, subfolders allowed.
* If you use the LiveTree toolbar in VS it would crash while debugging at some point please do not use it. 
* GC.Collect might create sudden lag spikes during animations.

## Screenshots

### _Draw recycled cells on a canvas!_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/257eed8a-d96f-4640-bf01-b4c798e52abb

### _Scroll, navigate and switch virtual views!_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/e86200de-16ba-4e24-a7f5-ec7ba944a9a9

### _Build your drawn controls_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/2182de51-929e-47f6-a560-e9d97ad16e52

### _Draw rich and tappable text_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/a2efa080-f234-4d04-a7f5-aab8af462fb2

### _Feel free to create!_

https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo/assets/25801194/b2ac6dfb-f3c0-44a0-80dc-e45a028b7ca6


## Features

* __Draw your UI using SkiaSharp with hardware acceleration__
* __Easily create your controls and animations__
* __Design in Xaml or code-behind__
* __2D and 3D Transforms__
* __Animations__ targeting max fps
* __Gestures__ support for panning, scrolling and zooming (_rotation incoming_)
* __Caching system__ for elements and images
* __Optimized for performance__, rendering only visible elements, recycling templates etc
* __Prebuilt Basic Ui Elements__	
	* __SkiaShape__ (Rounded rectangle, Circle, Gauge, _more to come_) can wrap other elements
	* __SkiaLabel__, multiline with many options
	* __SkiaImage__ with options and filters
	* __SkiaSvg__ with many options
	* __SkiaLottie__ with tint customization
	* __SkiaRive__ (actually Windows only)
	* __SkiaLayout__ (Absolute, Grid, Vertical stack, Horizontal stack, _todo Masonry_) with templates support
	* __SkiaScroll__ (Horizonal, Vertical, Both) with header, footer, zoom support and adjustable inertia, bounce, snap and much more. Can act like a collectionview with custom refresh indicator, load more etc
	* __SkiaHotspot__ to handle gestures in a easy way
	* __SkiaMauiElement__ for when skia is not enough

* __Derived custom controls__
	* __SkiaButton__ include anything inside, text, images etc
	* __SkiaScrollLooped__ for neverending scrolls
    * __SkiaDrawer__ to swipe in and out your controls
	* __SkiaCarousel__ swipe and slide controls inside a carousel
	* __SkiaHoverMask__ to overlay a clipping shape
	* __SkiaLabelFps__ for developement
	* __SkiaDecoratedGrid__ to draw shapes between rows and columns
	* __ScrollPickerWheel__ for creating wheel pickers
	* __RefreshIndicator__ can use lottie and anything for your scroll refresh view
	* __SkiaTabsSelector__ create top and bottom tabs
	* __SkiaViewSwitcher__ switch your views, pop, push and slide	
	* __Create your own!__ 

* Animated Effects
	* __Ripple__
	* __Shimmer__
	* __BlinkColors__
	* (_todo Pulse, Shake etc_)
	* __Commit yours!__

* Animators
	
	* _todo add info

* Transforms
	* TranslationX
	* TranslationY
	* ScaleX
	* ScaleY
	* Rotation
	* CameraAngleX
	* CameraAngleY
	* CameraAngleZ
	* SkewX
	* SkewY
	* Perspective1
	* Perspective2
	
## Installation

Install the package __AppoMobi.Maui.DrawnUi__ from NuGet. Check the pre-release option if you don't see the package.

After that initialize the library inside your MauiProgram.cs file:

```csharp
builder.UseDrawnUi<App>();
```

## Quick Start

You will be mainly using Maui view `Canvas` that will wrap your SkiaControls.
Anywhere in your existing Maui app you can include a `Canvas` and start drawing your UI.
The `Canvas` control is aware of its children size and will resize accordingly.
At the same time you could set a fixed size for the `Canvas` and its children will adapt to it.

#### Xaml
Import the namespace:
```xml  
  xmlns:draw="http://schemas.appomobi.com/drawnUi/2023/draw"
```
Consume:

```xml  
<draw:Canvas>
     <draw:SkiaSvg
        Source="Svg/dotnet_bot.svg"
        LockRatio="1"
		HorizontalOptions="Center"
        TintColor="White"
        WidthRequest="44" />
</draw:Canvas>
```

As you can see in this example the Maui view `Canvas` will adapt it's size to drawn content and should take 44x44 pts.

#### Code behind

```csharp
	_todo_
```

Please check the demo app, it contains many examples of usage.

### Gestures

To make your root `Canvas` catch gestures you need to attach a `TouchEffect` to it.
After that skia controls can process gestures in multiple ways:
* At low level by implementing an `ISkiaGestureListener` interface and overriding `OnGestureReceived`.
* At control level by overriding `ProcessGestures`, recommended for custom controls.
* Attaching a `HandleGestures` effect that has properties similar `SkiaHotspot`.
* Including a `SkiaHotspot` as a child.
* Using a `SkiaButton`.

Parent controls have full control over gestures and passing them to chidlren. 
In a base scenarion a gesture would be passed all along to the ends of a view tree to its ends for every top-level control.
If a gesture is marked as consumed (by returning `true`) a control would typically stop processing gestures at this level. 

By overriding `ProcessGestures` any control might process gestures with or without passing them to children.

When creating a custom control the standart code for the override would be to pass gestures below by caling `base` then processing at the current level. You might choose to do it differently acording your needs.

The engine is designed to pass the ending gestures to thoses who already returned "consumed" for preceding gestures even if following gestures are out of their hitbox.

When the DOWN gestures is received the engine will try to find the topmost control that can handle it. The overridable `AutoHitbox` would be checked for intersection with gesture, in base this hitbox is checking versus `DrawingRect`.

Avoid trying to handle gastures below the cached level, as DrawingRect might still point to older position in the screen if the cached parent already moved.


### Caching System

_!_ Without caching animations going beyond simple wouldn't be possible. 
Caching makes complicated processing needed just once for layout calculation 
and then the caching result is redrawn on every frame.

Caching is controlled using a property `UseCache` of the following type:

```csharp
public enum SkiaCacheType
{
    /// <summary>
    /// True and old school
    /// </summary>
    None,

    /// <summary>
    /// Create and reuse SKPicture. Try this first for labels, svg etc. 
    /// Do not use this when dropping shadows or with other effects, better use Bitmap. 
    /// </summary>
    Operations,

    /// <summary>
    /// Will use simple SKBitmap cache type, will not use hardware acceleration.
    /// Slower but will work for sizes bigger than graphics memory if needed.
    /// </summary>
    Image,

    /// <summary>
    /// Using `Image` cache type with double buffering. Best for fast animated scenarios, this must be implemented by a specific control, not all controls support this, will fallback to 'Image' if anything.
    /// </summary>
    ImageDoubleBuffered,

    /// <summary>
    /// The way to go when dealing with images surrounded by shapes etc.
    /// The cached surface will use the same graphic context as your canvas.
    /// If hardware acceleration is enabled will try to cache as Bitmap inside graphics memory. Will fallback to simple Bitmap cache type if not possible. If you experience issues using it, switch to Memory cache type.
    /// </summary>
    GPU,
}

```

You should tweak your design caching to avoid unnecessary re-drawing of elements. 
The basic approach here is to cache small elements at some level. 
For example you would cache small children like cells inside a SkiaScroll as `Image`. 
At the same time avoid using bitmaps where just `Operations` type is enough, for example for __SkiaSvg__ results.
You cannot include controls cached with type GPU inside controls cached with some different type, think of it like you can't include graphics memory inside cpu memory, will get a crash otherwise.

When you start using any kind of animations you should start using caching to max your FPS. You can check the __DemoApp__ for such examples.

#### Loaded Images

We are using the __EasyCaching.InMemory__ library for caching loaded bitmaps. It's impact can much be seen when using recycled cells inside a scroll. 
_todo add options and link to ImageLoader and SkiaImage docs_

_!_ When using images inside dynamic scene, like a a templated stack with scroll or other 
you should try to set the image cache to `Image` this would most probably climb your fps.
This is due the fact that image sources are usually of the wrong size and they need processing 
before being drawn. When using `Image` cache the image would be processed only once and 
then just redrawn.

### Transforms

* TranslationX
* TranslationY
* ScaleX
* ScaleY
* Rotation
* CameraAngleX
* CameraAngleY
* CameraAngleZ
* SkewX
* SkewY
* Perspective1
* Perspective2

### Animations

One would create animations and effects using animators. Animators are attached to controls, but technically register themselves at the root canvas level and their code is executed on every rendering frame. If the canvas is not redrawing then animators will not be executed.
When the canvas has a registered animator running it would constantly force re-drawing itsself until all animators are stopped.

There are two types of animators: 
* __Animators__ are executed before the drawing, so you can move and transform elements before the are rendered on the canvas.
* __PostAnimators__ are executed after their parent element was drawn, so you can paint an effect over the existing result, or execute any other post-drawing logic.

### Layout

For initial items positionning you would be using `SkiaLayout`. 
Its `Absolute` layout type is already buil-it in every skia control..

You can position your children using familiar properties like
`HorizonalOptions`, `VerticalOptions`, `Margin`, parent `Padding`,
`WidthRequest`, `HeightRequest`,`MinimumWidthRequest`, `MinimumHeightRequest` 
and additional `MaximumWidthRequest`,  `MaximumHeightRequest`, `HorizontalFillRatio`, `VerticalFillRatio` and `LockRatio` properties.

* `LockRatio`will be used to calculate the width when the height is set or vice versa. If it's above 0 the max value will be applied, if it's below 0 the min value will be applied. If it's 0 then the ratio will be ignored.
* `HorizontalFillRatio`, `VerticalFillRatio` will let you take a percent and the available size if you don't set a numeric request. For example if `HorizontalFillRatio` is set to 0.5 you will take 50% of the available width, at the same time being able to align the control at start, center or at the and in a usual way.

For dynamic positionning or other precise cases use `TranslationX` and `TranslationY`.

Whn you need to layout children in a more arranged way you will want to wrap them with a `SkiaLayout`
of different `LayoutType` : Grid, Colum, Row and others.

Layout supports `ItemTemplate` for most of layout types.

Some differences from Xamarin.Forms/Maui to notice:

* Layouts do not implement the Expand feature, in order to limit number of calculation passes, so if your children do not request a size, 
the parent layout will not take any space if you do not ask it to `Fill`.

* In Column and Row layouts we do not support Fill, End and Center for children, only Start, for the same reasons. When you need Fill, End and Center please use Absolute or Grid types of layout.

_!_ Layouts `Column` and `Row`, watever templated or not, 
will always check if child out of the visible scrren bounds and avoid rendering it in that case.
That is especialy useful when the layout is inside a SkiaScroll, this way we always render 
only the visible part.

_!_ You can absolutely use the `Margin` property as usual in a Maui way. In case if you would need to bind a specific margin you can switch to 
using `MarginLeft`, `MarginTop`, `MarginRight`, `MarginBottom` properties, -1.0 by default, 
if set they will override specific value from `Margin`, and the result would be accessible via `Margins` readonly bindable static property.
When designing custom controls please use `Margins` property to read the final margin value.

#### Loading sources

SkiaImage, SkiaLottie and other controls that have a `Source` property, can load data from web, from bundled resources and from native file system.
The conventions is the following:
- if a web url is detected the source is loaded from web
- if a file path starts with file:// it will be loaded from native file system
- otherwise will try to load from bundled resources with root folder 'Resources\Raw'. 

Example below will load animation from `Resources\Raw\Lottie\Loader.json`.

```xml  
            <draw:SkiaLottie
                InputTransparent="True"
                AutoPlay="True"
                ColorTint="{StaticResource ColorPrimary}"
                HorizontalOptions="Center"
                ="1"
                Opacity="0.85"
                Repeat="-1"
                Source="Lottie/Loader.json"
                Tag="Loader"
                VerticalOptions="Center"
                WidthRequest="56" />
```


#### Enhanced usage

When dealing with subviews in code behind you could typically use two ways. 

Example for adding a subview:
 
* Use method `SetParent` passing new parent. In this case parent layout will not be invalidated, use this for optimized logic when you know what you are doing. You can mainly use this way when just constructing parent, knowing it will be measured at start anyway.
* Call parent's method `AddSubView` passing subview.Parent's layout will be invalidated, and OnChildAdded will be called on parent.
* When working with `Children` property use `Add` method, it will set `Views` to a new instance of appropriate collection, and call `AddSubView` for each item.

For removing a subview the usual options would be:

* Call `SetParent` passing null, for soft removal.
* Call parent's method `RemoveSubView` passing subview. Parent's layout will be invalidated, and OnChildRemoved will be called on parent.	
* When working with `Children` property use `Remove` method, it will set `Views` to a new instance of appropriate collection, and call `RemoveSubView` for each item.
 
#### In Deep

When XAML would be constructed it would fill `Children` property with views, this property is for high-level access. `Views` 

Will be then filled internally. When you add or remove items in `Children` methods `AddSubView` and `RemoveSubView` will be called for managing `Views`.


### Enhanced Usage

#### Draw a line or rectangle or reserve space

Use a simple `SkiaControl`. For complex shapes use `SkiaShape` or `SkiaPath`.

#### Simulate Maui Grid

`SkiaLayout` of `Grid` type, set children properties as usual (Grid.something)

 
#### Simulate Maui CollectionView

`SkiaScroll` + `SkiaLayout` (ItemTemplate=...). Set cache of the cell to Bitmap or Operations depending on your needs.


#### Simulate Maui StackLayout with a BidableLayout.ItemTemplate 

`SkiaScroll` (Virtualisation=false) + `SkiaLayout` (ItemTemplate=..., UseCache=CacheType.Operations) 



#### *Styles*

When you want to dynamically change properties in Xaml you might want to use conditional styles.
They look like regular Maui styles, but with some nuances:
* When defining style is resources you must set a unique `Class` attibute
* They are selected at runtime upon `Condition` or `State` bindable properties. `State` is like Maui `VisualState`but you can have several of them applied at same time.

Define a style inside `ResourceDictionary`:

```xml  
    <Style
        x:Key="SkiaLabelDefaultStyle"
        Class="SkiaLabelDefaultStyle"
        TargetType="draw:SkiaLabel">
        <Setter Property="TextColor" Value="#E8E3D7" />
        <Setter Property="FontFamily" Value="FontText" />
        <Setter Property="FontSize" Value="15" />
    </Style>
```

Apply a style:

```xml  
<draw:SkiaLabel
    Style="{StaticResource SkiaLabelStyle}"
    Text="Simple Styled Label" />
```
Apply styles upon conditions:

```xml  
   <views:SkiaShape
    LockRatio="1"
    Type="Circle"
    WidthRequest="16">
    <views:SkiaControl.Styles>
        <views:ConditionalStyle
            State="Normal"
            Style="{x:StaticResource StyleCameraDot}" />
        <views:ConditionalStyle
            Condition="{Binding .}"
            Style="{x:StaticResource StyleCameraDotOn}" />
    </views:SkiaControl.Styles>

</views:SkiaShape>
```

Same as:

```xml  
   <views:SkiaShape
    Style="{StaticResource StyleCameraDot}"
    LockRatio="1"
    Type="Circle"
    WidthRequest="16">
    <views:SkiaControl.Styles>
        <views:ConditionalStyle
            Condition="{Binding .}"
            Style="{x:StaticResource StyleCameraDotOn}" />
    </views:SkiaControl.Styles>

</views:SkiaShape>
```
When the BindingContext (x:Boolean) is True the style `StyleCameraDotOn` will be applied, otherwise `StyleCameraDot` will be applied.

To apply styles upon states you will be using standart Maui *VisualStates* mechanism, or you can even have serveral states at the same time, every `SkiaControl` has a `string[]  States` bindable property to server this purpose:

```xml  
   <views:SkiaShape
    States="{Binding VisualStates}"
	LockRatio="1"
	Type="Circle"
	WidthRequest="16">
	<views:SkiaControl.Styles>
		<views:ConditionalStyle
			State="Normal"
			Style="{x:StaticResource StyleCameraDot}" />
		<views:ConditionalStyle
			State="IsOn"
			Style="{x:StaticResource StyleCameraDotOn}" />
	</views:SkiaControl.Styles>
```

### Hints

_todo_

For SkiaStack of type Row or Column avoid using 
Fill for children ?? that would require a second measurement pass.

## Maui Views

### `Canvas`

#### _Events_:

__`WillDraw`__ 

__`WillFirstTimeDraw`__ 

__`WasDrawn`__ 

#### _Methods_: 

__`TakeScreenShot(Action<SKImage> callback)`__ Takes screenshot on next draw. Passes an `SKImage`, can be null. If you need an `SKBitmap` use `SKBitmap.FromImage` on it.

#### _Properties_:

__`Surface`__ 


### `SkiaShell`

The usage is almost the same as the standart Maui Shell, 
with some extra features.

`SkiaShell` is derived from `FastShell` that uses maui interfaces 
and implements methods for standart maui navigation, 
then adds features to be able to navigate inside the Canvas.

Some additional features to be mentionned are actions that can be executed for specific routes.
code example:

```csharp  
RegisterRoute("profile", typeof(ScreenUserProfile));

RegisterActionRoute("settings", () =>
{
    //select settings tab
    this.NavigationLayout.SelectedIndex = 4;
});

```

#### Usage

Please see demo.

## Drawn Controls

### `SkiaControl`

Base drawn element, derived from Maui `Element` to assure basic Maui Xaml compatibility, could derive from a `BindableObject` if Maui would provide public interfaces for that matter.

#### _Properties_:

__`LockRatio`__ a numeric value that will be used to calculate the width when the height is set or vice versa. If it's above 0 the max value will be applied, if it's below 0 the min value will be applied. If it's 0 then the ratio will be ignored.

Example 1:
```xml  
  <draw:SkiaShape
                        LockRatio="1"
                        WidthRequest="40" />
```
HeightRequest wasn't specified but this control will request 40 by 40 pts.

Example 2:
```xml  
  <draw:SkiaShape
	LockRatio="-1"
	HeightRequest="30"
	WidthRequest="40" />
```
This control will request 30 by 30 pts.

### `SkiaScroll`

`SkiaScroll` is a scrollable container that supports virtualization and recycling of its children.

If you include a `SkiaLayout` inside a `SkiaScroll` only visible on screen items will be rendered.

If the include a `SkiaLayout` that uses `ItemTemplate` this combination will automatically become virtualized and you will get sort of a CollectionView with recycled cells at your disposal. It is a good practice to use it for long lists of items.

#### _Properties_:

__`Orientation`__ a value of type `ScrollOrientation` that can be `Vertical` or `Horizontal`.

__`FrictionScrolled`__  Use this to control how fast the scroll will decelerate. Values 0.1f - 0.3f are the best, default is 0.1f.

__`IgnoreWrongDirection`__  Will ignore gestures of the wrong direction, for example if this Orientation is Horizontal will ignore gestures with vertical direction velocity. Might want to set to true when you have a horizontal scroll inside a vertical scroll, this will let the parent scroll start scrolling vertically ven if gesture started inside its horizontal sroll child.

### `SkiaLayout`

`SkiaLayout` is a container that supports various layout types: `Absolute`, `Grid`, `Row`, `Column` and others. 

It also supports virtualization and recycling of its children with `ItemTemplate` property.

Controls inside templated `SkiaLayout` can implement `ISkiaCell` interface to eventually receive information about their state:
* `OnAppearing`
* `OnDisapearing`
* `OnScrolled`

This lets one to create custom controls that can react to scrolling and other events with animations etc.

### `SkiaShape`

`SkiaShape` is a base class for all shapes. You could fill it, stroke, drop shadows, apply gradients and even clip other controls with it.

### `SkiaImage`

`SkiaImage` is a control that renders images. It cant apply filters and transformations.



### `SkiaSvg`

`SkiaSvg` is a control that renders svg files. It cant tint the svg with a color or gradient, and apply some transforms to it.


### `SkiaLabel`

A multi-line label fighting for his place under the sun.

#### _Properties_:

__`FontWeight`__ a numeric value used in case you have properly registered your fonts to support weights. 
You can use your font the usual Maui way but in case of custom font files used from resources you might want to register them, using the following example:
```csharp
.ConfigureFonts(fonts =>
{
   fonts.AddFont("Gilroy-Regular.ttf", "FontText", FontWeight.Regular);
   fonts.AddFont("Gilroy-Medium.ttf", "FontText", FontWeight.Medium);
});
```
Now if you set the `FontWeight` to `500` the control will use the `Gilroy-Medium.ttf` file.
This might come very handy when your Figma design shows you to use this weight and you want just to pass it over to SkiaLabel.

 __`HorizontalTextAlignment`__  : 
 ```csharp
public enum DrawTextAlignment
{
	Start,
	Center,
	End,
	FillWords,
	FillWordsFull,
	FillCharacters,
	FillCharactersFull,
}
```


### `SkiaLottie`

`SkiaLottie` is a control that renders lottie files. It can even tint some colors inside your animation!

### `SkiaRive`

Actually for Windows only, this plays and controls Rive animation files. Other platforms will be added soon, poke if you would like to help biding some c++;

### SkiaHoverMask

A control deriving from SkiaShape that can be used to create hover effects. 
It will render a mask over its children when hovered, think of it as an inverted shape.

### Docs under construction
 
