using KeyControl2.Util;
using PlayifyUtility.Windows.Features.Hooks;
using PlayifyUtility.Windows.Features.Interact;

namespace KeyControl2.Features.Hotkeys;

[InitOnLoad]
public static class KeepDown{
	private static HashSet<Keys>? _set;

	static KeepDown(){
		GlobalKeyboardHook.KeyDown+=KeyDown;
		GlobalKeyboardHook.KeyUp+=KeyUp;
	}

	private static void KeyDown(KeyEvent e){
		if(_set!=null){
			_set.Add(e.Key);
			GlobalKeyboardHook.OnRelease[e.Key]=null;
		}

		if(e.Key!=Keys.K) return;
		if(!Modifiers.Win||!Modifiers.Ctrl) return;
		e.Handled=true;
		_set??=new HashSet<Keys>();
	}

	private static void KeyUp(KeyEvent e){
		if(_set?.Contains(e.Key)??false)
			_set=null;
	}
}