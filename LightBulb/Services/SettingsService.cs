﻿using System;
using System.Collections.Generic;
using LightBulb.Internal;
using LightBulb.Models;
using Tyrrrz.Settings;

namespace LightBulb.Services
{
    public class SettingsService : SettingsManager
    {
        public event EventHandler? SettingsReset;

        public bool IsFirstTimeExperienceEnabled { get; set; } = true;

        // General

        public ColorConfiguration NightConfiguration { get; set; } = new ColorConfiguration(3900, 0.85);

        public ColorConfiguration DayConfiguration { get; set; } = new ColorConfiguration(6600, 1);

        public TimeSpan ConfigurationTransitionDuration { get; set; } = TimeSpan.FromMinutes(30);

        // Location

        public bool IsManualSunriseSunsetEnabled { get; set; } = true;

        public TimeSpan ManualSunriseTime { get; set; } = new TimeSpan(07, 20, 00);

        public TimeSpan ManualSunsetTime { get; set; } = new TimeSpan(16, 30, 00);

        public GeoLocation? Location { get; set; }

        // Advanced

        public bool IsAutoStartEnabled { get; set; } = false;

        public bool IsAutoUpdateEnabled { get; set; } = true;

        public bool IsDefaultToDayConfigurationEnabled { get; set; } = false;

        public bool IsConfigurationSmoothingEnabled { get; set; } = true;

        public bool IsPauseWhenFullScreenEnabled { get; set; } = false;

        public bool IsGammaPollingEnabled { get; set; } = false;

        // Application whitelist

        public bool IsApplicationWhitelistEnabled { get; set; } = false;

        public IReadOnlyList<ExternalApplication>? WhitelistedApplications { get; set; }

        // Hotkeys

        public HotKey ToggleHotKey { get; set; }

        public SettingsService()
        {
            // If we have write access to application directory - store configuration file there
            if (DirectoryEx.CheckWriteAccess(App.ExecutableDirPath))
            {
                Configuration.SubDirectoryPath = "";
                Configuration.StorageSpace = StorageSpace.Instance;
            }
            // Otherwise - store settings in roaming app data directory
            else
            {
                Configuration.SubDirectoryPath = "LightBulb";
                Configuration.StorageSpace = StorageSpace.SyncedUserDomain;
            }

            Configuration.FileName = "Settings.json";
            Configuration.ThrowIfCannotLoad = false;
            Configuration.ThrowIfCannotSave = true;

            // Auto-start by default in release builds
#if !DEBUG
            IsAutoStartEnabled = true;
#endif
        }

        public override void Reset()
        {
            base.Reset();
            SettingsReset?.Invoke(this, EventArgs.Empty);
        }
    }
}