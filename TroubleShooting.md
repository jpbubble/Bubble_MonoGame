A few things MUST be noted before you start reporting bugs in my issue tracker, as your issue might be discared unhandled when I cannot deal with them properly.

# I get the error message ".NET Error"

Nothing I can do about that UNLESS you describe in detail what you were trying to do! The error ".NET Error" doesn't make sense at all, as there are millions of things that could have gone wrong, and I really hate it that the error thrown by some underlying dependencies I used are not more specific than this.
But if you can make fully clear what you were trying to do, it becomes easier for me to guess what could have happened, and to replicate the issue hopefully leading to pinning the source of the problem.

# Some error messages I got are in Dutch/Spanish/Finnish/Fill in your language here, in stead of English

That will be because your Windows version (or whatever OS you are using in combination with Mono) is not configured to run in English.
Some errors are thrown by .NET itself and properly caught and converted into a proper error screen. And the thing is .NET basically always runs in the same language as your underlying OS (at least in Windows it does, as I do know know how Mono deals with this).
Errors thrown by Bubble itself will always be in English, errors thrown by .NET will take over your system language.
Nothing I can do about it, so don't even bother to report it!

# I tried to make a game in Bubble, but all I got is that silly console

This is normal behavior and means the engine is not used the way it should be.
The file init.lua or init.nil should only be used to prepare the engine for action, and perhaps even load a few things. After that it must be instructed to load a flow and to start that flow, and then things will work.
If you wanna to keep things in one flow, or if you want to make over one thousand of them is up to you, but that start up procedure must at least be followed.

# Bubble moans about JCR compression methods not being supported

At the moment I wrote this document, I only managed to get compression drivers for lzma, zlib and jxsrcca working in C# in which this Bubble engine was written.
The compression utility for JCR6 does indeed also support 'flate' and 'lzw', but that is solely because that utility was written in Go where libraries for these algorithms were provided with the Go standard libraries, so all I had to do was create a quick driver and boom. In C# I was not that lucky. I did find out that Igor Pavlov created a C# class for lzma himself (thanks Igor), and that class was also used in the C# version of JCR6. Zlib was already a harder story, and for lzw I didn't find anything at all (yet). Now I know that 'flate' and 'zlib' are technically the same algorithm, but if they are fully compatible was something I could not yet really study, so I didn't wanna take much chances here.
For the time being I recommend lzma, as that's the most stable driver I could set up for JCR6 in C# and thus also for Bubble (jxsrcca is only meant for DOS export project in order not to use too much RAM in DOS).

# Must I use Lua or NIL in order to code in Bubble?

Depends on what you like best. You can even use MoonScript if you like, although you'll have to install MoonScript into your project manually if you prefer that. NIL requires Lua itself, and has been provided as some basic instructions and API links have been written in NIL, but Lua can easily take these all up without any serious issues.
When it comes to your own scripts, if you prefer NIL, use NIL, if you prefer Lua, use Lua. Simple as that.

# I got a Bubble project working fine on Windows, but not in MacOS and Linux

At the present time I can only give technical support for Windows. I am running (Manjaro) Linux in Virtual Box and my Mac is too old to handle things. 
### Mac
Note that since Mojave OpenGL has been deprecated and knowing Apple they'll remove it completely soon, rendering the Bubble unrunnable on a Mac in any way, and no I have NO interest to use METAL. You gotta complain to Apple about that stupid act and not to me. This is their stupidity and not mine.
### Linux
When it comes to Linux I must ask for patience, as a VM is NOT a proper way to test things in, and it will take some time before I can start setting things up on a "real" Linux machine, and no ETA has been set up for that, as I really don't know when I can get some help to do this without destroying my Windows installation (which has happened to me a few times in the past)


