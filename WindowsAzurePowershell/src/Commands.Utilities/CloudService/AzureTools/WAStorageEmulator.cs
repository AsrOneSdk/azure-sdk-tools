﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.Utilities.CloudService.AzureTools
{
    using System;
    using System.IO;
    using Microsoft.WindowsAzure.Commands.Utilities.Properties;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    public class WAStorageEmulator
    {
        private string _emulatorPath;

        public WAStorageEmulator(string emulatorDirectory)
        {
            _emulatorPath = Path.Combine(emulatorDirectory, Resources.StorageEmulatorExe);
        }

        internal ProcessHelper CommandRunner { get; set; } 

        public void Start(out string standardOutput, out string standardError)
        {
            ProcessHelper runner = GetCommandRunner();
            runner.StartAndWaitForProcess(_emulatorPath, "start");
            standardOutput = CommandRunner.StandardOutput;
            standardError = CommandRunner.StandardError;
        }

        public void Stop(out string standardOutput, out string standardError)
        {
            ProcessHelper runner = GetCommandRunner();
            runner.StartAndWaitForProcess(_emulatorPath, "stop");
            standardOutput = CommandRunner.StandardOutput;
            standardError = CommandRunner.StandardError;
        }

        private ProcessHelper GetCommandRunner()
        {
            if (CommandRunner == null)
            {
                CommandRunner = new ProcessHelper();
            }
            return CommandRunner;
        }
    }
}
