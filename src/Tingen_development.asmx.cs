﻿// ================================================================ 24.6.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240604 =====

// u240604.1308

/* PLEASE READ
 * -----------
 * This is the development version of Tingen, and should not be used in production environments.
 *
 * For stable releases of Tingen: https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments: https://github.com/spectrum-health-systems/Tingen-Community-Release
 *
 * For more information about Tingen: https://github.com/spectrum-health-systems/Tingen-Documentation
 *
 * For more information about web services and Avatar: https://github.com/myAvatar-Development-Community
 */

using System.Reflection;
using System.Web.Services;
using Outpost31.Core.Configuration;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>The entry class for Tingen.</summary>
    /// <remarks>
    ///  <para>
    ///   - This class is designed to be static, and should not be modified.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - The version number is the current development version in <b>YY.MM</b> format (ex: "<b>24.5</b>").
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => "VERSION 24.6";

        /// <summary>The starting method for Tingen.</summary>
        /// <param name="sentOptionObject">The OptionObject sent from myAvatar.</param>
        /// <param name="sentScriptParameter">The Script Parameter sent from myAvatar.</param>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - Should <i>not be modified</i>, since the heavy lifting is done elsewhere.<br/>
        ///  </para>
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* The assembly name, defined here for use in all log files.*/
            string Asm = Assembly.GetExecutingAssembly().GetName().Name;

            /* For debugging */
            //LogEvent.Primeval(asm);

            string configFilePath = TingenConfiguration.BuildFilePath("UAT");

            TingenSession tnSession = TingenSession.BuildNew(configFilePath, sentOptionObject, sentScriptParameter);

            TingenSession.InitializeNew(tnSession);

            LogEvent.Trace(1, tnSession, Asm);

            switch (tnSession.TingenMode)
            {
                case "disabled":
                    LogEvent.Trace(2, tnSession, Asm);
                    Outpost31.Module.Admin.Service.AllUpdate(tnSession);
                    Outpost31.Core.Avatar.OptionObjects.CloneSentToReturn(tnSession);
                    break;

                default:
                    LogEvent.Trace(2, tnSession, Asm);
                    Outpost31.Core.Roundhouse.Parse(tnSession);
                    break;
            }

            return tnSession.AvData.ReturnOptionObject;
        }
    }
}