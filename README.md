# Popcorn

<a href="https://popcorn-slack.azurewebsites.net" target="_blank">
  <img alt="Slack" src="http://popcorn-slack.azurewebsites.net/badge.svg">
</a> [![Build status](https://ci.appveyor.com/api/projects/status/mjnfwck6otg9c5wj?svg=true)](https://ci.appveyor.com/project/bbougot/popcorn) [![Coverage Status](https://coveralls.io/repos/github/bbougot/Popcorn/badge.svg?branch=master)](https://coveralls.io/github/bbougot/Popcorn?branch=master)

An application which aims to provide a simple interface to watch any movie.

![Screenshot1](https://github.com/bbougot/Popcorn/blob/master/Screenshots/Screen1.jpg)

![Screenshot2](https://github.com/bbougot/Popcorn/blob/master/Screenshots/Screen2.jpg)

![Screenshot3](https://github.com/bbougot/Popcorn/blob/master/Screenshots/Screen3.jpg)

## What does it use?
.NET Framework 4.6.2 and C# for the backend. WPF/XAML for the interface.

### Dependencies
* MVVM framework: [MVVM Light](https://mvvmlight.codeplex.com) 
* UI framework: [MahApps](https://github.com/MahApps/MahApps.Metro)
* Media Player: [Meta.Vlc](https://github.com/higankanshi/Meta.Vlc)
* libtorrent wrapper: [torrent.NET](https://github.com/bbougot/torrent.NET) **Self made**
* ORM: [Entity Framework](https://github.com/aspnet/EntityFramework)
* Database storage: [SqlServer Compact](https://www.nuget.org/packages/Microsoft.SqlServer.Compact/)
* JSON Deserialization: [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* REST management: [RestSharp](https://github.com/restsharp/RestSharp)
* Logging: [NLog](https://github.com/NLog/NLog)
* Unit testing: [NUnit](https://github.com/nunit/nunit) & [AutoFixture](https://github.com/AutoFixture/AutoFixture)
* IMDb data: [TMDbLib](https://github.com/LordMike/TMDbLib/)
* Downloadable Youtube videos: [YoutubeExtractor](https://github.com/flagbug/YoutubeExtractor)
* Localization: [WpfLocalizeExtension](https://github.com/SeriousM/WPFLocalizationExtension)

### API
* Using own private [API](https://popcornapi.azurewebsites.net/), hosted in Azure as a ASP.NET Core web app (source [here](https://github.com/bbougot/PopcornApi)).

## Supported platforms
At this time, only Windows 7+ is supported (Windows 7, 8, 8.1, 10).

## Can I help you?
Of course yes! Any pull-request will be considered.

## Installer
Download full installer [here](https://github.com/bbougot/Popcorn/releases/download/v1.9.2/Setup.exe)
