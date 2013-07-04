﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoPro.Hero.Api.Commands;

namespace GoPro.Hero.Api
{
    public interface IHeroCamera
    {
        IHeroCamera Shutter(bool open);
        IHeroCamera Command(CommandRequest<IHeroCamera> command);
        IHeroCamera Command(CommandRequest<IHeroCamera> command,out CommandResponse commandResponse,bool checkStatus=true);
        IHeroCamera PrepareCommand<T>(out T command) where T : CommandRequest<IHeroCamera>;
        IHeroCamera Power(bool on);
        T PrepareCommand<T>() where T : CommandRequest<IHeroCamera>;
        CommandResponse Command(CommandRequest<IHeroCamera> command,bool checkStatus=true);

        IHeroCamera SetName(string name);
        IHeroCamera GetName(out string name);
        string GetName();

        CameraSettings Settings { get; }
        CameraExtendedSettings ExtendedSettings {get;}
  
        BacpacStatus BacpacStatus { get; }
        BacpacInformation BacpacInformation { get; }
        CameraInformation Information { get; }

    }
}
