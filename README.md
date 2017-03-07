# Popcorn

<img src="http://popcorn-slack.azurewebsites.net/badge.svg">

An application which aims to provide a simple interface to watch any movie.

![Screenshot1](https://github.com/bbougot/Popcorn/blob/master/Screenshots/Screen1.png)

![Screenshot2](https://github.com/bbougot/Popcorn/blob/master/Screenshots/Screen2.png)

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

## Tasks
- [ ] Seekable movie stream
- [ ] Add series/anime
- [ ] Add cast page (actor, director,...)
- [ ] DLNA broadcasting
- [x] Subtitles support
- [x] List movies (Popular, Greatest, Recent)
- [x] Search (Movie Title/IMDb Code, Actor Name/IMDb Code, Director Name/IMDb Code)
- [x] Trailer support
- [x] Streaming movies
- [x] Quality selection (720p/1080p)
- [x] Favorites/Seen movies history
- [x] Player features : Play, Pause, Stop, Volume, Fullscreen
- [x] Settings features : Language (English/French), Broadband limit

## Installer
Download full installer [here](https://github.com/bbougot/Popcorn/releases/download/1.6.0/Setup.exe)
