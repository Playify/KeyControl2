using KeyControl2.Util;
using PlayifyUtility.Windows.Features.Hooks;
using PlayifyUtility.Windows.Features.Interact;
using PlayifyUtility.Windows.Win;

namespace KeyControl2.Features.Windows;

public static partial class MoveWindows{
	private static int _moveX;
	private static int _moveY;
	private static WinWindow _move;
	private static WinWindow _resize;

	private static void MouseDown(MouseEvent e){
		if(!EnableMouse.Value||!Modifiers.Win||!Modifiers.Ctrl) return;
		switch(e.Button){
			case MouseButtons.Left:
				_moveX=e.X;
				_moveY=e.Y;
				_move=Utils.GetCurrentWindow();
				e.Handled=true;
				break;
			case MouseButtons.Right:
				_moveX=e.X;
				_moveY=e.Y;
				_resize=Utils.GetCurrentWindow();
				e.Handled=true;
				break;
		}
	}

	private static void MouseUp(MouseEvent e){
		switch(e.Button){
			case MouseButtons.Left when _move:
				_move=WinWindow.Zero;
				e.Handled=true;
				break;
			case MouseButtons.Right when _resize:
				_resize=WinWindow.Zero;
				e.Handled=true;
				break;
		}
	}

	private static void MouseMove(MouseEvent e){
		if(EnableF1.Value&&_f1Down/*&&Modifiers.IsKeyDown(Keys.F1)*/) RunF1();
		else _f1Down=false;


		if(_move){
			var rect=_move.WindowRect;
			rect.Left+=e.X-_moveX;
			rect.Top+=e.Y-_moveY;
			if(!_resize){
				rect.Right+=e.X-_moveX;
				rect.Bottom+=e.Y-_moveY;
			}
			_moveX=e.X;
			_moveY=e.Y;
			_move.SetWindowPos(0,rect.Left,rect.Top,rect.Right-rect.Left,rect.Bottom-rect.Top,0);
		} else if(_resize){
			var rect=_resize.WindowRect;
			rect.Right+=e.X-_moveX;
			rect.Bottom+=e.Y-_moveY;
			_moveX=e.X;
			_moveY=e.Y;
			_resize.SetWindowPos(0,rect.Left,rect.Top,rect.Right-rect.Left,rect.Bottom-rect.Top,0);
		}
	}
}