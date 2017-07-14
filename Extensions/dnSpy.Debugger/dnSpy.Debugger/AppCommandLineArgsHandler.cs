using dndbg.Engine;
using dnSpy.Contracts.App;
using dnSpy.Debugger.Dialogs;
using System;
using System.ComponentModel.Composition;
using System.IO;

namespace dnSpy.Debugger {
	[Export(typeof(IAppCommandLineArgsHandler))]
	sealed class AppCommandLineArgsHandler : IAppCommandLineArgsHandler {
		private readonly Lazy<DebugService> debugService;

		[ImportingConstructor]
		AppCommandLineArgsHandler(Lazy<DebugService> debugService) {
			this.debugService = debugService;
		}

		public double Order => 1;

		public void OnNewArgs(IAppCommandLineArgs args) {
			if (!File.Exists(args.DebugProcess))
				return;
			string file = Path.GetFullPath(args.DebugProcess);
			string dir = Path.GetDirectoryName(file);

			//DebugProcessOptions opts = new DebugProcessOptions(new DesktopCLRTypeDebugInfo())
			//{
			//	CurrentDirectory = dir,
			//	Filename = file,
			//	CommandLine = args.DebugProcessArgs,
			//	BreakProcessKind = BreakProcessKind.ModuleCctorOrEntryPoint
			//};
			//opts.DebugOptions.IgnoreBreakInstructions = debugService.Value.DebuggerSettings.IgnoreBreakInstructions;
			//opts.DebugMessageDispatcher = WpfDebugMessageDispatcher.Instance;
			//debugService.Value.DebugAssembly(opts);

			var debugProcVM = new DebugProcessVM() {
				CurrentDirectory = dir,
				Filename = file,
				CommandLine = args.DebugProcessArgs,
				BreakProcessKind = BreakProcessKind.ModuleCctorOrEntryPoint
			};
			debugService.Value.DebugAssembly(debugProcVM, false);
		}
	}
}
