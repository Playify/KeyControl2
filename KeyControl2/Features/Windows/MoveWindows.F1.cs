using PlayifyUtility.Windows.Features.Hooks;
using PlayifyUtility.Windows.Features.Interact;
using PlayifyUtility.Windows.Win;
using PlayifyUtility.Windows.Win.Native;

namespace KeyControl2.Features.Windows;

public static partial class MoveWindows{
	private static bool _lWin;
	private static bool _rWin;
	private static bool _f1Down;

	private static void KeyDown(KeyEvent e){
		if(e.Key!=Keys.F1&&!(e.Key==Keys.Packet&&EnableVive.Value&&e.ScanCode=='£')) return;
		_f1Down=e.Key==Keys.F1;
		if(!EnableF1.Value) return;

		var mods=Modifiers.Combined;

		//If only windows is pressed, then do normal F1
		if(mods==ModifierKeys.Windows){

			KeyEvent.CancelWindowsKey();
			var send=new Send();
			if(Modifiers.IsKeyDown(Keys.LWin)){
				_lWin=true;
				send.Key(Keys.LWin,false);
			}
			if(Modifiers.IsKeyDown(Keys.RWin)){
				_rWin=true;
				send.Key(Keys.RWin,false);
			}
			send.SendNow();

			return;
		}
		//Only move window if no additional modifier is pressed
		if(mods!=ModifierKeys.None) return;

		RunF1();
		e.Handled=true;
	}

	private static void KeyUp(KeyEvent e){
		if(e.Key!=Keys.F1) return;
		_f1Down=false;
		if(_lWin||_rWin){
			var send=new Send();
			if(_lWin) send.Key(Keys.LWin,true);
			if(_rWin) send.Key(Keys.RWin,true);
			send.SendNow();
			KeyEvent.CancelWindowsKey();
			_lWin=_rWin=false;
		}
	}

	public static void RunF1(){
		var window=WinWindow.Foreground;

		//Desktop or Taskbar should not be moved
		if(window.Class is "Progman" or "Shell_TrayWnd" or "Shell_SecondaryTrayWnd") return;

		if(Path.GetFileName(window.ProcessExe)=="paintdotnet.exe"&&(window.ExStyle&ExStyle.ToolWindow)!=0)
			return;

		if(!WinCursor.TryGetCursorPos(out var cursorPos)) return;

		Rectangle rectangle=window.WindowRect;
		var beforeScreen=Screen.FromRectangle(rectangle);
		var afterScreen=Screen.FromPoint(cursorPos);

		var maximize=false;
		var isWin11=Environment.OSVersion.Version.Build>22000;//Win11 doesn't allow MoveWindow on already maximized windows, therefore a workaround is needed
		if(isWin11&&beforeScreen.Bounds!=afterScreen.Bounds&&window.Maximized){
			maximize=true;
			window.Maximized=false;
			rectangle=window.WindowRect;
		}

		if(window.Fullscreen||window.Maximized) rectangle=afterScreen.Bounds;
		else{
			rectangle.Location+=new Size(afterScreen.Bounds.Location-new Size(beforeScreen.Bounds.Location));
			rectangle.Size+=afterScreen.Bounds.Size-beforeScreen.Bounds.Size;
			if(!maximize)
				maximize=Maximize.Value&&!window.Maximized;//Only for non fullscreen windows
		}


		window.MoveWindow(rectangle,!maximize);
		if(maximize) window.Maximized=true;//maximizing redraws, therefore MoveWindow doesn't need to
	}
}