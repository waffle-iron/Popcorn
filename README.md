# Popcorn
An application which aims to provide a simple interface to watch any movie.

[![Build status](https://ci.appveyor.com/api/projects/status/2tijq4sm43r87lmf?svg=true)](https://ci.appveyor.com/project/bbougot/popcorn)

![Screenshot1](https://cloud.githubusercontent.com/assets/8962802/9370674/46d0d9dc-46d2-11e5-9627-26db73f56efd.jpg)

![Screenshot2](https://cloud.githubusercontent.com/assets/8962802/9288484/e6f0a25e-4348-11e5-9317-f8c0dee12729.jpg)

![Screenshot3](https://cloud.githubusercontent.com/assets/8962802/9288481/dee26ee4-4348-11e5-972e-b7609bd07ca8.jpg)

## It reminds me of something
Well, you may be thinking about Popcorn Time, don't you? If so, yes. The concept is not new, and this project is clearly inspired from it. Also, my primary goal was to make an alternative, using .NET framework.

## What does it use?
I use .NET Framework 4.6 and C# for the backend. WPF/XAML for the interface. Libtorrent-rasterbar for torrent downloading.

## Supported platforms
At this time, only Windows 7+ is supported (Windows 7, 8, 8.1, 10 | 32/64 Bits).

## Can I help you?
Of course yes! It's open-source so feel free to submit pull-request, I promise I'll consider them.

## Visual Studio .sln
The solution is built using Visual Studio 2015.

## Popcorn VS Popcorn Time?
Well, there's some points.

* Cross-platform. Popcorn Time is based on Node-WebKit, hence, on NodeJS. Consequently, it's cross-platform. At the opposite, Popcorn is fully built using .NET/C#. So, multi-platform does not come out of the box. However, Mono works well and is aimed to support multi-platform, so that future support for multiple OS is conceivable.

* Performances. Since NodeJS is based on V8 VM, it is fast. Really fast. But compared to a fully native app, it's a slow rabbit. That's why Popcorn Time compares bad to Popcorn, whose performances are great.

* Features. Since Popcorn is a recent project, Popcorn Time supports more features. But it's just a matter of time.

* API. It's simple: Popcorn uses the YTS and The Movie Database APIs, same ones that Popcorn Time uses.

## How do I compile the solution?
Just open the .sln with VS 2015, right-click on the solution, click "Enable NuGet Package Restore" and run F5. 

## What if I don't have VS 2015/.NET 4.6 yet?
This project is using some of the new features of C# 6, such as null-propagation. So, keep in mind this project won't compile on earlier versions without making some code change.

