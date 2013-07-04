﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoPro.Hero.Api.Commands;
using GoPro.Hero.Api.Commands.CameraCommands;
using GoPro.Hero.Api.Exceptions;
using GoPro.Hero.Api.Utilities;

namespace GoPro.Hero.Api
{
    public class Camera:IHeroCamera
    {
        private Bacpac _bacpac;

        private CameraInformation _information;
        private CameraExtendedSettings _extendedSettings;
        private CameraSettings _settings;

        public CameraInformation Information
        {
            get
            {
                this.GetInformation();
                return _information;
            }
        }
        public CameraExtendedSettings ExtendedSettings
        {
            get
            {
                this.GetExtendedSettings();
                return _extendedSettings;
            }
        }
        public CameraSettings Settings
        {
            get
            {
                this.GetSettings();
                return _settings;
            }
        }
        public BacpacStatus BacpacStatus
        {
            get { return this._bacpac.Status; }
        }
        public BacpacInformation BacpacInformation
        {
            get { return this._bacpac.Information; }
        }

        public string GetName()
        {
            var request = this.PrepareCommand<CommandCameraGetName>();
            var response = request.Send();

            var raw = response.RawResponse;
            var length=response.RawResponse[1];
            var name= Encoding.UTF8.GetString(raw, 1, length);
            if (!string.IsNullOrEmpty(name)) return name;
            return this.Information.Name.Substring(4);
        }
        public IHeroCamera GetName(out string name)
        {
            name = this.GetName();

            return this;
        }
        public IHeroCamera SetName(string name)
        {
            name = name.UrlEncode();

            var request = this.PrepareCommand<CommandCameraSetName>();
            request.Name = name;

            request.Send();

            return this;
        }

        private void GetInformation()
        {
            var request = this.PrepareCommand<CommandCameraInformation>();
            var response = request.Send();

            var stream = response.GetResponseStream();
            this._information.Update(stream);
        }
        private void GetSettings()
        {
            var request = this.PrepareCommand<CommandCameraSettings>();
            var response = request.Send();

            var stream = response.GetResponseStream();
            this._settings.Update(stream);
        }
        private void GetExtendedSettings()
        {
            var request = this.PrepareCommand<CommandCameraExtendedSettings>();
            var response = request.Send();

            var stream = response.GetResponseStream();
            _extendedSettings.Update(stream);
        }

        public IHeroCamera Shutter(bool open)
        {
            _bacpac.Shutter(open);
            return this;
        }
        public IHeroCamera Power(bool on)
        {
            _bacpac.Power(on);
            return this;
        }

        public IHeroCamera Command(CommandRequest<IHeroCamera> command)
        {
            var response = command.Send();
            return this;
        }
        public IHeroCamera Command(CommandRequest<IHeroCamera> command,out CommandResponse commandResponse,bool checkStatus=true)
        {
            commandResponse = this.Command(command,checkStatus);
            return this;
        }
        public CommandResponse Command(CommandRequest<IHeroCamera> command, bool checkStatus = true)
        {
            return command.Send(checkStatus);
        }

        public T PrepareCommand<T>() where T : CommandRequest<IHeroCamera>
        {
            return CommandRequest<IHeroCamera>.Create<T>(this._bacpac.Address, passPhrase: this._bacpac.Password);
        }
        public IHeroCamera PrepareCommand<T>(out T command) where T : CommandRequest<IHeroCamera>
        {
            command = this.PrepareCommand<T>();
            return this;
        }

        public Camera(Bacpac bacpac)
        {
            _information = new CameraInformation();
            _extendedSettings = new CameraExtendedSettings();
            _settings = new CameraSettings();

            _bacpac = bacpac;
        }

        public static T Create<T>(Bacpac bacpac) where T : Camera,IHeroCamera
        {
            var camera = Activator.CreateInstance(typeof(T), bacpac) as T;
            return camera;
        }
    }
}
