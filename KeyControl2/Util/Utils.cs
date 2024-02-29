using PlayifyUtility.Windows;
using PlayifyUtility.Windows.Features.Interact;
using PlayifyUtility.Windows.Win;

namespace KeyControl2.Util;

public static class Utils{
	public static readonly UiThread UiThread=UiThread.Create(nameof(Utils)+"."+nameof(UiThread));

	public static WinWindow GetCurrentWindow()
		=>Modifiers.Shift&&WinCursor.TryGetCursorPos(out var cursorPos)
			  ?WinWindow.GetWindowAt(cursorPos)
			  :WinWindow.Foreground;
}