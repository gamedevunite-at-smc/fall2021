# fall2021

Game Dev Unite Fall 2021 prototype project

## Installation

Download and Install Unity 2020 LTS (e.g. `2020.3.21f1`), preferably through Unity Hub

Then, download the project, either through the [GUI](https://docs.unity3d.com/2020.3/Documentation/Manual/upm-ui-giturl.html), or through the [command line](https://docs.unity3d.com/2020.3/Documentation/Manual/CommandLineArguments.html), like so

```powershell
git clone 'https://github.com/gamedevunite-at-smc/fall2021'
Set-Location fall2021
& "C:\Program Files\Unity\path\to\executable\Unity.exe" -projectPath .
```

### Note

This repository uses [Git LFS](https://git-lfs.github.com) to store large files. Make sure to use LFS or some of the repository's files may be corrupted on your disk.

## Automated Bulding

```powershell
& "C:\Program Files\Unity\path\to\executable\Unity.exe" -projectPath . -quit -batchmode -logfile Logs/AutomatedBuild.log -executeMethod 'GameBuilder.MyBuild'
```

## Attributions

- [Rogue knight](https://darkpixel-kronovi.itch.io/rogue-knight)

> you can use this on any commercial or non-commercial project, credits are not required but it's so cool if you do.
